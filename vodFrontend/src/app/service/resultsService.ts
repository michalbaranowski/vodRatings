import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { TokenHandler } from './tokenHandler';
import { AjaxService } from './ajaxService';
import { Result } from '../model/result';
import { Observable, Subscribable } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class ResultsService {

    constructor(private ajaxService: AjaxService) { }
    
    resultsUrl = 'api/movies';
    watchedMoviesUrl = 'api/watchedMovies';

    getResults(type: Number) {
        if(!type) {
            return this.ajaxService.doGet<Result[]>(this.resultsUrl)
        }
        
        return this.ajaxService.doGet<Result[]>(this.resultsUrl + "?filmType=" + type)
    }

    setAsAlreadyWatched(title: string) {
        return this.ajaxService.doPost(this.watchedMoviesUrl, {title: title});
    }
}