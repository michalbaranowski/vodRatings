import { Injectable } from '@angular/core';
import { RegisterData } from '../model/registerData';
import { LoginCredentials } from '../model/loginCredentials';
import { Observable } from 'rxjs';
import { LoginResult } from '../model/loginResult';
import { AjaxService } from './ajaxService';
import { RegisterResult } from '../model/registerResult';
import { TokenHandler } from './tokenHandler';
import { UserData } from '../model/userData';

@Injectable({
    providedIn: 'root',
})
export class AuthService {

    constructor(private ajaxService: AjaxService,
                private tokenHandler: TokenHandler) { }
    
    loginUrl = 'api/login';
    registerUrl = 'api/register';
    authorizeUrl = 'api/authorize';

    login(loginCred: LoginCredentials) : Observable<LoginResult> {
        if(loginCred)
            return this.ajaxService.doPost<LoginResult>(this.loginUrl, loginCred);
    }

    register(registerData: RegisterData) : Observable<RegisterResult> {
        if(registerData)
            return this.ajaxService.doPost<RegisterResult>(this.registerUrl, registerData);
    }

    authorize() {
        return this.ajaxService.doGet<UserData>(this.authorizeUrl);
    }

    logout() {
        this.tokenHandler.removeToken();
    }
}