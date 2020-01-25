import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { MovieTypeHelper } from 'src/app/helpers/movie-type-helper';

@Component({
    selector: 'app-refresh-mark',
    styleUrls: ['refresh-mark-component.css'],
    templateUrl: 'refresh-mark-component.html'
})
export class RefreshMarkComponent implements OnInit {
    private _hubConnection: HubConnection;
    showNotification: Boolean;
    movieType: string;

    constructor(private movieTypeHelper: MovieTypeHelper) {    }

    ngOnInit(): void {
        this.showNotification = false;
        this._hubConnection = new HubConnectionBuilder().withUrl('/updateNotification').build();

        this._hubConnection
            .start()
            .then(() => console.log('Connection started (refresh notification)!'))
            .catch(err => console.log('Error while establishing connection :('));

        this._hubConnection.on("NotifyUpdate", (movieType) => {
           this.showNotification = false; 
           this.movieType = this.movieTypeHelper.getMovieType(movieType);
        });

        this._hubConnection.on("RefreshStarted", (movieType) => {
            this.showNotification = true;
            this.movieType = this.movieTypeHelper.getMovieType(movieType);
         });
    }

}