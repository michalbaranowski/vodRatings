import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

@Component({
    selector: 'app-update-modal',
    templateUrl: 'update-modal-component.html'
})
export class UpdateModalComponent implements OnInit {
    private _hubConnection: HubConnection;
    updateFilmType: number;
    @Output() updateEmitter = new EventEmitter<boolean>();

    ngOnInit(): void {
        this.showUpdateModal(1);
        this._hubConnection = new HubConnectionBuilder().withUrl('/updateNotification').build();

        this._hubConnection
            .start()
            .then(() => console.log('Connection started!'))
            .catch(err => console.log('Error while establishing connection :('));

        this._hubConnection.on("NotifyUpdate", function (typeToUpdate) {
            this.showUpdateModal(typeToUpdate);
        })
    }

    update() {
        this.updateEmitter.emit();
        var el = this.getModalElement();
        el.classList.remove('is-active');
    }

    closeModal() {
        var el = this.getModalElement();
        el.classList.remove('is-active');
    }

    showUpdateModal(typeToUpdate) {
        this.updateFilmType = typeToUpdate;
        //   switch(typeToUpdate){
        //     case 0: this.updateFilmType = 'Thriller'
        //     case 1: this.updateFilmType = 'Komedia'
        //     case 2: this.updateFilmType = 'Akcja'
        //   }

        var el = this.getModalElement();
        el.classList.add('is-active');
    }

    private getModalElement() {
        var modal = Array.prototype.slice.call(document.querySelectorAll('.modal'), 0)[0];
        return modal;
    }
}