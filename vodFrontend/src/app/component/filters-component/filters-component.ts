import { Component, Input, Output, EventEmitter, SimpleChanges, SimpleChange, OnInit } from "@angular/core";
import { Result } from 'src/app/model/result';

@Component({
    selector: 'app-filters',
    templateUrl: 'filters-component.html',
    styleUrls: ['filters-component.css']
})

export class FiltersComponent implements OnInit {
    
    @Input() results: Result[];
    @Output() filterChanged = new EventEmitter<Result>();
    filmwebTypes: string[];
    providerNames: string[];
    productionNames: string[];
    movieFilter: Result = new Result();

    ngOnInit() {
        //początkowy filter aby powiązać obiekt movieFilter z komponentem z filmami - todo: zrobic to porządnie
        this.onFilterChanged();
        this.refresh();
    }

    refresh() {
        this.movieFilter.providerName = "Wszystkie";
        this.movieFilter.filmwebFilmType = "Wszystkie";
        this.movieFilter.production = "Wszystkie";
        this.movieFilter.title = "";
        this.movieFilter.isAlreadyWatched = false;

        this.filmwebTypes = [];
        this.results.forEach((x) => {
            if (this.filmwebTypes.indexOf(x.filmwebFilmType) == -1 && x.filmwebFilmType) {
                var types = x.filmwebFilmType.split(', ');
                types.forEach(type => {
                    if(this.filmwebTypes.indexOf(type) == -1)
                        this.filmwebTypes.push(type);
                });
            }
        });

        this.providerNames = [];
        this.results.forEach((x) => {
            if (this.providerNames.indexOf(x.providerName) == -1) this.providerNames.push(x.providerName);
        });

        this.productionNames = [];
        this.results.forEach((x) => {
            if (this.productionNames.indexOf(x.production) == -1 && x.production) this.productionNames.push(x.production);
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