import { Component } from '@angular/core';
import { AuthService } from 'src/app/service/authService';
import { RegisterData } from 'src/app/model/registerData';

@Component({
    selector: 'app-register-form',
    templateUrl: 'register-form-component.html',
    styleUrls: ['register-form-component.css']
})
export class RegisterFormComponent{

    constructor(private authService: AuthService) {}

    registerData: RegisterData = new RegisterData();
    passwordRepeat: string;

    register() {
        if(this.passwordRepeat != this.registerData.password) {
            return;
        }

        this.authService.register(this.registerData).subscribe(registerResult => {
            if(registerResult.statusCode === 200) {
                
            }
        });
    }
}