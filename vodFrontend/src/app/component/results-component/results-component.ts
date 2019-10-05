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
        this.initMenu();
        this.movieFilterArgs = new Result();
        this.getMoviesOfType(0);
    }

    getMoviesOfType(type: Number) {
        this.loading = true;
        this.resultsService.getResults(type).subscribe(n => {
            this.results = n as Result[];
            this.loading = false;

            this.filmwebTypes = [];
            this.results.forEach((x) => {
                if (this.filmwebTypes.indexOf(x.filmwebFilmType) == -1 && x.filmwebFilmType) 
                    this.filmwebTypes.push(x.filmwebFilmType);
            });

            this.providerNames = [];
            this.results.forEach((x) => {
                if (this.providerNames.indexOf(x.providerName) == -1) this.providerNames.push(x.providerName);
            });
        });
    }

    initMenu() {
        document.addEventListener('DOMContentLoaded', () => {

            // Get all "navbar-burger" elements
            const $navbarBurgers = Array.prototype.slice.call(document.querySelectorAll('.navbar-burger'), 0);
            // Check if there are any navbar burgers
            if ($navbarBurgers.length > 0) {
                debugger;
                // Add a click event on each of them
                $navbarBurgers.forEach(el => {
                    el.addEventListener('click', () => {
                        // Get the target from the "data-target" attribute
                        const target = el.dataset.target;
                        const $target = document.getElementById(target);

                        // Toggle the "is-active" class on both the "navbar-burger" and the "navbar-menu"
                        el.classList.toggle('is-active');
                        $target.classList.toggle('is-active');

                    });
                });
            }

            const $menuItems = Array.prototype.slice.call(document.querySelectorAll('.navbar-item'), 0);

            if ($menuItems.length > 0) {
                $menuItems.forEach(el => {
                    el.addEventListener('click', () => {
                        $menuItems.forEach(x => x.classList.remove('is-active'));
                        el.classList.toggle('is-active');
                    });
                });
            }
        });
    }

}