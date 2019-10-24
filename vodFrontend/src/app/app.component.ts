import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  private _hubConnection: HubConnection;

  ngOnInit(): void {
    
    this._hubConnection = new HubConnectionBuilder().withUrl('/updateNotification').build();

    this._hubConnection
    .start()
    .then(() => console.log('Connection started!'))
    .catch(err => console.log('Error while establishing connection :('));

    this._hubConnection.on("NotifyUpdate", function(typeToUpdate) {
      console.log("update! " + typeToUpdate);
    })
  }

  title = 'vodFrontend';
}
