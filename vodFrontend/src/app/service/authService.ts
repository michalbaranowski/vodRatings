import { Injectable } from '@angular/core';
import { RegisterData } from '../model/registerData';
import { LoginCredentials } from '../model/loginCredentials';
import { Observable } from 'rxjs';
import { LoginResult } from '../model/loginResult';
import { AjaxService } from './ajaxService';
import { RegisterResult } from '../model/registerResult';

@Injectable({
    providedIn: 'root',
})
export class AuthService {

    constructor(private ajaxService: AjaxService) { }
    
    loginUrl = 'login';
    registerUrl = 'register';

    login(loginCred: LoginCredentials) : Observable<LoginResult> {
        if(loginCred)
            return this.ajaxService.doPost<LoginResult>(this.loginUrl, loginCred);
    }

    register(registerData: RegisterData) : Observable<RegisterResult> {
        if(registerData)
            return this.ajaxService.doPost<RegisterResult>(this.registerUrl, registerData);
    }
}