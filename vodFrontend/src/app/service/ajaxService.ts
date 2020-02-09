import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { RegisterData } from '../model/registerData';
import { LoginCredentials } from '../model/loginCredentials';
import { Observable } from 'rxjs';
import { LoginResult } from '../model/loginResult';
import { ApiResult } from '../model/apiResult';
import { NotifierService } from 'angular-notifier';
import { TokenHandler } from './tokenHandler';

@Injectable({
    providedIn: 'root',
})
export class AjaxService {

    constructor(private http: HttpClient,
                private notifyService: NotifierService,
                private tokenHandler: TokenHandler) { }
    
    doGet<T>(url: string) {
        return this.http.get<T>(url, this.prepareOptions());
    }

    doPost<T>(url: string, data: object) {
        return this.http.post<T>(url, data, this.prepareOptions());
    }

    private prepareOptions() {
        let token = this.tokenHandler.getToken();
        let headers = new HttpHeaders({'Content-Type': 'application/json', 'Authorization': 'Bearer ' + token});  
        return {headers: headers};
    }
}