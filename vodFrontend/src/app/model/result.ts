export class Result {
    constructor() {}

    id: number;
    title: string;
    filmwebRating: number;
    filmwebRatingCount: number;
    year: number;
    providerName: string;
    imageUrl: any;
    filmwebFilmType: string;
    production: string;
    storedDate: Date;
    filmDescription: string;
    isNew: boolean;
    movieUrl: string;
    cast: Array<string>;
    isAlreadyWatched: Boolean;
}