import { Component, Input } from '@angular/core';
import { Result } from 'src/app/model/result';

@Component({
    selector: 'app-movie',
    templateUrl: 'movie-component.html'
})
export class MovieComponent {
    @Input() item: Result;
}