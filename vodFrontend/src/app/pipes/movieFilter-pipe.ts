import { Pipe, PipeTransform } from '@angular/core';
import { Result } from '../model/result';

@Pipe({
    name: 'movieFilter',
    pure: false
})
export class MovieFilter implements PipeTransform {
    transform(items: any[], filter: Result): any {
        if (!items || !filter) {
            return items;
        }
        var results = items;

        if (filter.title) {
            results = results.filter(item => item.title != null && item.title.toLowerCase().indexOf(filter.title.toLowerCase()) !== -1 ||
                                             item.filmwebTitle != null && item.filmwebTitle.toLowerCase().indexOf(filter.filmwebTitle.toLowerCase()) !== -1 );
        }

        if (filter.filmwebFilmType && filter.filmwebFilmType != "Wszystkie")
            results = results.filter(item => {
                if (!item.filmwebFilmType) return true;
                return item.filmwebFilmType.indexOf(filter.filmwebFilmType) !== -1;
            });

        if (filter.providerName && filter.providerName != "Wszystkie")
            results = results.filter(item => item.providerName.indexOf(filter.providerName) !== -1);

        if (filter.production && filter.production != "Wszystkie")
            results = results.filter(item => item.production && item.production.indexOf(filter.production) !== -1);

        if(filter.isNew)
            results = results.filter(item => item.isNew == true);
            
        if(filter.isAlreadyWatched === false)
            results = results.filter(item => item.isAlreadyWatched === false);
            
        return results;
    }
}