import { ResultsService } from 'src/app/service/resultsService';
import { Component } from '@angular/core';
import { Result } from 'src/app/model/result';
import { AuthService } from 'src/app/service/authService';

@Component({
    selector: 'app-results',
    templateUrl: 'results-component.html'
})
export class ResultsComponent {
    loading: boolean = true;
    results: Result[];
    movieFilterArgs: Result;
    typeToChange: number = 0;
    isLoggedIn: Boolean;

    constructor(private resultsService: ResultsService,
                private authService: AuthService) {

        this.movieFilterArgs = new Result();
        this.authService.authorize().subscribe(result => this.isLoggedIn = result != null)
    }

    getMoviesOfType(type: Number) {
        this.loading = true;
        this.resultsService.getResults(type).subscribe(n => {
            this.results = n as Result[];
            this.loading = false;
            this.authService.authorize().subscribe(
                result => {
                    this.isLoggedIn = result != null
                },
                error => {
                    this.isLoggedIn = false
                });
        });
    }

    onTypeChanged(type: Number) {
        this.getMoviesOfType(type);
    }

    onFilterChanged(newFilter: Result) {
        this.movieFilterArgs = newFilter;
    }

    onUpdate(type: number) {
        this.typeToChange = type;
    }
}