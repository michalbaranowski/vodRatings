import { Injectable } from '@angular/core';
import { Result } from '../model/result';
import { HttpClient } from '@angular/common/http';

@Injectable({
    providedIn: 'root',
})
export class ResultsService {

    constructor(private http: HttpClient) { }
    
    resultsUrl = 'api/movies';

    getResults() {
        return this.http.get(this.resultsUrl);
    }

}