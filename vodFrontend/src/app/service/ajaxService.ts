import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { TokenHandler } from './tokenHandler';

@Injectable({
    providedIn: 'root',
})
export class AjaxService {

    constructor(private http: HttpClient,
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