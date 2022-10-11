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
        switch (error.status) {
          case 401:
            if (req.url === "api/authorize") {
              return throwError(error);
            }

            this.notifyService.notify("warning", "Logowanie nie powiodło się", error.status);
            return throwError(error);
          case 499:
            this.notifyService.notify("warning", "Użytkownik isnieje już w systemie", error.status);
            return throwError(error);
          case 498:
            this.notifyService.notify("warning", "Wystąpił błąd podczas rejestracji: " + error.error.description);
            return throwError(error);
        }
        this.notifyService.notify("error", "Error " + error.status + ": " + error.statusText);
        return throwError(error);
      })
    );
  }
}