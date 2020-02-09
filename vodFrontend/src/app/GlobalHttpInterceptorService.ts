import { Injectable } from "@angular/core";
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from "rxjs";
import { catchError } from 'rxjs/operators';
import { NotifierService } from 'angular-notifier';

@Injectable()
export class GlobalHttpInterceptorService implements HttpInterceptor {
  constructor(private notifyService: NotifierService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler):
    Observable<HttpEvent<any>> {

    return next.handle(req).pipe(
      catchError((error) => {
        console.log(error);
        switch (error.status) {
          case 401:
            this.notifyService.notify("warning", "Aby przejść dalej należy się zalogować", error.status);
            return throwError(error);
        }
        this.notifyService.notify("error", "Error " + error.status + ": " + error.statusText);
        return throwError(error);
      })
    );
  }
}