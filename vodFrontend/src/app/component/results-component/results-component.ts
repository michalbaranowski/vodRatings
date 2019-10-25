import { ResultsService } from 'src/app/service/resultsService';
import { Component } from '@angular/core';
import { Result } from 'src/app/model/result';

@Component({
    selector: 'app-results',
    templateUrl: 'results-component.html'
})
export class ResultsComponent {
    loading: boolean;
    results: Result[];
    movieFilterArgs: Result;

    constructor(private resultsService: ResultsService) {
        this.movieFilterArgs = new Result();
        this.getMoviesOfType(0);
    }

    getMoviesOfType(type: Number) {
        this.loading = true;
        this.resultsService.getResults(type).subscribe(n => {
            this.results = n as Result[];
            this.loading = false;
        });
    }

    onTypeChanged(type: Number) {
        this.getMoviesOfType(type);
    }

    onFilterChanged(newFilter: Result) {
        this.movieFilterArgs = newFilter;
    }

    onUpdate(type: number) {
        this.getMoviesOfType(type);
    }
}