import { Component, Input, OnInit } from '@angular/core';
import { Result } from 'src/app/model/result';
import { AuthService } from 'src/app/service/authService';
import { ResultsService } from 'src/app/service/resultsService';

@Component({
    selector: 'app-movie',
    templateUrl: 'movie-component.html',
    styleUrls: ['movie-component.css']
})
export class MovieComponent{
    
    constructor (private resultsService: ResultsService) {}

    @Input() item: Result;
    @Input() displayWatchedButton: Boolean;

    onClick(event) {
        event.currentTarget.classList.toggle('is-active');
    }

    formatTitle(title, year) {
        let standardTitle = title + " (" + year + ")";
        return standardTitle.length > 40 
            ? standardTitle.substring(0,35) + " (...)" 
            : standardTitle;
    }

    openMovie(movie:Result) {
        var win = window.open(movie.movieUrl, '_blank');
        win.focus();
    }
    
    setAsAlreadyWatched(movie: Result) {
        debugger;
        this.resultsService.setAsAlreadyWatched(movie.title).subscribe(n=>n);
        movie.isAlreadyWatched = true;
    }
}