import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root',
})

export class MovieTypeHelper {
    getMovieType(typeToUpdate: number) {
        switch (typeToUpdate) {
            case 0: return 'Thriller'
            case 1: return 'Komedia'
            case 2: return 'Akcja'
            case 3: return 'Bajki'
            default: return 'Nieznany typ filmu: ' + typeToUpdate
        }
    }
}