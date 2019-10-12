import { Component, Input, Output, EventEmitter, SimpleChanges, SimpleChange, OnInit } from "@angular/core";
import { Result } from 'src/app/model/result';

@Component({
    selector: 'app-filters',
    templateUrl: 'filters-component.html'
})

export class FiltersComponent implements OnInit {
    
    @Input() movieFilterArgs: Result;
    @Input() results: Result[];
    @Output() filterChanged = new EventEmitter<Result>();
    filmwebTypes: string[];
    providerNames: string[];


    ngOnInit() {
        this.refresh();
    }

    refresh() {
        this.filmwebTypes = [];
        this.results.forEach((x) => {
            if (this.filmwebTypes.indexOf(x.filmwebFilmType) == -1 && x.filmwebFilmType)
                this.filmwebTypes.push(x.filmwebFilmType);
        });

        this.providerNames = [];
        this.results.forEach((x) => {
            if (this.providerNames.indexOf(x.providerName) == -1) this.providerNames.push(x.providerName);
        });
    }

    ngOnChanges(changes: SimpleChanges) {
        const movieFilterArgs: SimpleChange = changes.movieFilterArgs;
        const results: SimpleChange = changes.results;

        if(movieFilterArgs) this.filterChanged.emit(movieFilterArgs.currentValue);
        if(results) this.refresh();
    }

}