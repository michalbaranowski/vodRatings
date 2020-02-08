import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RegisterData } from '../model/registerData';
import { LoginCredentials } from '../model/loginCredentials';
import { Observable } from 'rxjs';
import { LoginResult } from '../model/loginResult';
import { ApiResult } from '../model/apiResult';

@Injectable({
    providedIn: 'root',
})
export class AuthService {

    constructor(private http: HttpClient) { }
    
    loginUrl = 'login';
    registerUrl = 'register';

    login(loginCred: LoginCredentials) : Observable<LoginResult> {
        if(loginCred)
            return this.http.post(this.loginUrl, loginCred) as Observable<LoginResult>;
    }

    register(registerData: RegisterData) : Observable<ApiResult> {
        if(registerData)
            return this.http.post(this.registerUrl, registerData) as Observable<ApiResult>;
    }
}