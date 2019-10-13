import { Component, Input, Output, EventEmitter, SimpleChanges, SimpleChange, OnInit } from "@angular/core";
import { Result } from 'src/app/model/result';

@Component({
    selector: 'app-filters',
    templateUrl: 'filters-component.html'
})

export class FiltersComponent implements OnInit {
    
    @Input() results: Result[];
    @Output() filterChanged = new EventEmitter<Result>();
    filmwebTypes: string[];
    providerNames: string[];
    movieFilter: Result = new Result();

    ngOnInit() {
        this.refresh();
    }

    refresh() {
        this.movieFilter.providerName = "Wszystkie";
        this.movieFilter.filmwebFilmType = "Wszystkie";

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

    onFilterChanged() {
        this.filterChanged.emit(this.movieFilter);
    }

    ngOnChanges(changes: SimpleChanges) {
        const results: SimpleChange = changes.results;
        if (results) this.refresh();
    }

}