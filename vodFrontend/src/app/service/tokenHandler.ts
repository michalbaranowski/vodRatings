import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
    providedIn: 'root',
})
export class TokenHandler {

    constructor(private cookieService: CookieService) { }

    private tokenName = "vodSearcher-token";

    setToken(token: string) {
        this.cookieService.set(this.tokenName, token);
    }

    getToken() {
        return this.cookieService.get(this.tokenName);
    }

    removeToken() {
        this.cookieService.delete(this.tokenName);
    }
}