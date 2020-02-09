import { Component, Output, EventEmitter } from '@angular/core';
import { AuthService } from 'src/app/service/authService';
import { LoginCredentials } from 'src/app/model/loginCredentials';
import { TokenHandler } from 'src/app/service/tokenHandler';
import { NotifierService } from 'angular-notifier';

@Component({
    selector: 'app-login-form',
    templateUrl: 'login-form-component.html',
    styleUrls: ['login-form-component.css']
})
export class LoginFormComponent{
    constructor(private authService: AuthService,
                private tokenHandler: TokenHandler,
                private notifyService: NotifierService) { }

    loginCred: LoginCredentials = new LoginCredentials();

    @Output() loggedIn = new EventEmitter<Boolean>();

    login() {
        this.authService.login(this.loginCred).subscribe(loginResult => {
            this.tokenHandler.setToken(loginResult.token);
            this.notifyService.notify("success", "Zalogowano pomyślnie...");
            this.loggedIn.emit(true);
        });
    }
}