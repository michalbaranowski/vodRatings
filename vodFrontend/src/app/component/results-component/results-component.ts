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

    constructor(private resultsService: ResultsService) {
        this.loading = true;
        this.resultsService.getResults().subscribe(
            data => {
                this.results = data as Result[];
                this.loading = false;
            });
     }

  }