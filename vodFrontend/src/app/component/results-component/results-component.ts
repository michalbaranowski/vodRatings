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
    filmwebTypes: string[];
    providerNames: string[];
    movieFilterArgs: Result;

    constructor(private resultsService: ResultsService) {
        this.movieFilterArgs = new Result();
        this.loading = true;
        this.resultsService.getResults().subscribe(
            data => {
                this.results = data as Result[];
                this.loading = false;

                this.filmwebTypes = [];
                this.results.forEach((x) => {
                    if(this.filmwebTypes.indexOf(x.filmwebFilmType) == -1) this.filmwebTypes.push(x.filmwebFilmType);
                });

                this.providerNames = [];
                this.results.forEach((x) => {
                    if(this.providerNames.indexOf(x.providerName) == -1) this.providerNames.push(x.providerName);
                });
            });
     }

  }