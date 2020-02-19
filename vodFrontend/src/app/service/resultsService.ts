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
    unwatchedMoviesUrl = 'api/unwatchedMovies';

    getResults(type: Number) {
        if(!type) {
            return this.ajaxService.doGet<Result[]>(this.resultsUrl)
        }
        
        return this.ajaxService.doGet<Result[]>(this.resultsUrl + "?filmType=" + type)
    }

    setAsAlreadyWatched(movie: Result) {
        return this.ajaxService.doPost(this.watchedMoviesUrl, movie);
    }

    setAsUnwatched(movie: Result) {
        // return this.ajaxService.doPost(this.unwatchedMoviesUrl, movie);
        return this.ajaxService.doDelete(this.watchedMoviesUrl, movie.id);
    }
}