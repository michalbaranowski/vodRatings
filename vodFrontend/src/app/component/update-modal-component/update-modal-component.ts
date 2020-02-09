import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { MovieTypeHelper } from 'src/app/helpers/movie-type-helper';

@Component({
    selector: 'app-update-modal',
    templateUrl: 'update-modal-component.html'
})
export class UpdateModalComponent implements OnInit {
    private _hubConnection: HubConnection;
    updateFilmType: number;
    @Output() updateEmitter = new EventEmitter<number>();

    constructor(private movieTypeHelper: MovieTypeHelper) {    }
    
    ngOnInit(): void {
        this._hubConnection = new HubConnectionBuilder().withUrl('/updateNotification').build();

        this._hubConnection
            .start()
            .then(() => console.log('Connection started!'))
            .catch(err => console.log('Error while establishing connection :('));

        this._hubConnection.on("NotifyUpdate", (typeToUpdate) => {
            this.showUpdateModal(typeToUpdate);
        });
    }

    update() {
        this.updateEmitter.emit(this.updateFilmType);
        var el = this.getModalElement();
        el.classList.remove('is-active');
    }

    closeModal() {
        var el = this.getModalElement();
        el.classList.remove('is-active');
    }

    showUpdateModal(typeToUpdate) {
        console.log("!! UPDATE: " + typeToUpdate);
        this.updateFilmType = typeToUpdate;
        var el = this.getModalElement();
        el.classList.add('is-active');
    }

    getMovieType(typeToUpdate: number) {
        return this.movieTypeHelper.getMovieType(typeToUpdate);
    }

    private getModalElement() {
        var modal = Array.prototype.slice.call(document.querySelectorAll('.update'), 0)[0];
        return modal;
    }
}