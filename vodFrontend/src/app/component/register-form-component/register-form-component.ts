import { Component, Output, EventEmitter } from '@angular/core';
import { AuthService } from 'src/app/service/authService';
import { RegisterData } from 'src/app/model/registerData';
import { NotifierService } from 'angular-notifier';

@Component({
    selector: 'app-register-form',
    templateUrl: 'register-form-component.html',
    styleUrls: ['register-form-component.css']
})
export class RegisterFormComponent{

    constructor(private authService: AuthService,
                private notifyService: NotifierService) {}

    registerData: RegisterData = new RegisterData();
    passwordRepeat: string;

    @Output() registered = new EventEmitter<Boolean>();
    
    register() {
        if(this.passwordRepeat != this.registerData.password) {
            this.notifyService.notify("warning", "Hasła muszą być takie same.");
            return;
        }

        this.authService.register(this.registerData).subscribe(registerResult => {
            this.notifyService.notify("success", "Użytkownik " + registerResult.username + " utworzony pomyślnie.");
            this.registered.emit(true);
        });
    }
}