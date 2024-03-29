import { Component, Output, EventEmitter, Input, OnChanges, SimpleChanges, SimpleChange } from "@angular/core";
import { AuthService } from 'src/app/service/authService';
import { UserData } from 'src/app/model/userData';

@Component({
    selector: 'app-navbar',
    templateUrl: 'navbar-component.html'
})

export class NavbarComponent implements OnChanges {

    @Output() typeChanged = new EventEmitter<Number>();
    @Input() loading: Boolean;
    @Input() typeToChange: number;

    currentUser: UserData;

    constructor(private authService: AuthService) {
        this.initMenu();
        this.checkIfLoggedInAndGetUsername();
    }
    
    ngOnChanges(changes: SimpleChanges): void {
        const typeToChange: SimpleChange = changes.typeToChange;
        if (typeToChange) this.changeType(typeToChange.currentValue);
    }

    changeType(n: Number) {
        this.typeChanged.emit(n);
    }

    initMenu() {
        document.addEventListener('DOMContentLoaded', () => {

            // Get all "navbar-burger" elements
            const $navbarBurgers = Array.prototype.slice.call(document.querySelectorAll('.navbar-burger'), 0);
            // Check if there are any navbar burgers
            if ($navbarBurgers.length > 0) {
                // Add a click event on each of them
                $navbarBurgers.forEach(el => {
                    el.addEventListener('click', () => {
                        // Get the target from the "data-target" attribute
                        const target = el.dataset.target;
                        const $target = document.getElementById(target);

                        // Toggle the "is-active" class on both the "navbar-burger" and the "navbar-menu"
                        el.classList.toggle('is-active');
                        $target.classList.toggle('is-active');

                    });
                });
            }

            const $menuItems = Array.prototype.slice.call(document.querySelectorAll('.navbar-item'), 0);

            if ($menuItems.length > 0) {
                $menuItems.forEach(el => {
                    el.addEventListener('click', () => {
                        $menuItems.forEach(x => x.classList.remove('is-active'));
                        el.classList.toggle('is-active');
                    });
                });
            }
        });
    }

    checkIfLoggedInAndGetUsername() {
        this.authService.authorize().subscribe(result => {
            this.currentUser = result;
            this.changeType(0);
        },
        error => {
            this.currentUser = null;       
            this.changeType(0);     
        });
    }

    openLogin() {
        const loginModal = Array.prototype.slice.call(document.querySelectorAll('.modal.login'), 0)[0];
        loginModal.classList.toggle('is-active');
    }

    closeLogin() {
        const loginModal = Array.prototype.slice.call(document.querySelectorAll('.modal.login'), 0)[0];
        loginModal.classList.remove('is-active');
    }

    loggedIn() {
        this.checkIfLoggedInAndGetUsername();
        this.closeLogin();
    }

    openRegister() {
        const loginModal = Array.prototype.slice.call(document.querySelectorAll('.modal.register'), 0)[0];
        loginModal.classList.toggle('is-active');
    }

    closeRegister() {
        const loginModal = Array.prototype.slice.call(document.querySelectorAll('.modal.register'), 0)[0];
        loginModal.classList.remove('is-active');
    }

    registered() {
        this.closeRegister();
    }

    logout() {
        this.authService.logout();
        this.checkIfLoggedInAndGetUsername();
    }
}