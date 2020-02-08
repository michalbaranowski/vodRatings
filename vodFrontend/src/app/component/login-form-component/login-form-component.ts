import { Component } from '@angular/core';
import { AuthService } from 'src/app/service/authService';
import { LoginCredentials } from 'src/app/model/loginCredentials';
import { TokenHandler } from 'src/app/service/tokenHandler';

@Component({
    selector: 'app-login-form',
    templateUrl: 'login-form-component.html',
    styleUrls: ['login-form-component.css']
})
export class LoginFormComponent{
    constructor(private authService: AuthService,
                private tokenHandler: TokenHandler) { }

    loginCred: LoginCredentials = new LoginCredentials();

    login() {
        this.authService.login(this.loginCred).subscribe(loginResult => {

            if(loginResult.statusCode === 200) {
                this.tokenHandler.setToken(loginResult.token);
            }
        });
    }
}