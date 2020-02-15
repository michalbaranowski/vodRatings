import { Component, Input, OnInit } from '@angular/core';
import { Result } from 'src/app/model/result';
import { ResultsService } from 'src/app/service/resultsService';

@Component({
    selector: 'app-movie',
    templateUrl: 'movie-component.html',
    styleUrls: ['movie-component.css']
})
export class MovieComponent{
    
    constructor (private resultsService: ResultsService) {}

    @Input() item: Result;
    @Input() displayWatchedSwitch: Boolean;

    onClick(event) {
        event.currentTarget.classList.toggle('is-active');
    }

    formatTitle(title, year) {
        let standardTitle = title + " (" + year + ")";
        return standardTitle.length > 25 
            ? standardTitle.substring(0,25) + " (...)" 
            : standardTitle;
    }

    openMovie(movie:Result) {
        var win = window.open(movie.movieUrl, '_blank');
        win.focus();
    }
    
    setAsAlreadyWatched(movie: Result) {        
        this.resultsService.setAsAlreadyWatched(movie).subscribe(n=>n);
        movie.isAlreadyWatched = true;
    }

    setAsUnwatched(movie: Result) {        
        this.resultsService.setAsUnwatched(movie).subscribe(n=>n);
        movie.isAlreadyWatched = false;
    }

    isWatchedChanged(movie: Result) {
        if(movie.isAlreadyWatched) {
            this.setAsAlreadyWatched(movie);
        } else {
            this.setAsUnwatched(movie);
        }
    }
}