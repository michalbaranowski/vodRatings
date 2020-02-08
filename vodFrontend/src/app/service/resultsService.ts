import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { TokenHandler } from './tokenHandler';

@Injectable({
    providedIn: 'root',
})
export class ResultsService {

    constructor(private http: HttpClient,
                private tokenHandler: TokenHandler) { }
    
    resultsUrl = 'api/movies';

    getResults(type: Number) {
        if(!type)
            return this.http.get(this.resultsUrl, this.prepareOptions());

        return this.http.get(this.resultsUrl + "?filmType=" + type, this.prepareOptions())
    }

    prepareOptions() {
        let token = this.tokenHandler.getToken();
        let headers = new HttpHeaders({'Content-Type': 'application/json', 'Authorization': 'Bearer ' + token});  
        return {headers: headers};
    }
}