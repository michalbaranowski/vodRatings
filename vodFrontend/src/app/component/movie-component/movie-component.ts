import { Component, Input, OnInit } from '@angular/core';
import { Result } from 'src/app/model/result';

@Component({
    selector: 'app-movie',
    templateUrl: 'movie-component.html',
    styleUrls: ['movie-component.css']
})
export class MovieComponent{
    
    @Input() item: Result;

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
}