import { Component, Output, EventEmitter } from "@angular/core";

@Component({
    selector: 'app-navbar',
    templateUrl: 'navbar-component.html'
})

export class NavbarComponent {
    @Output() typeChanged = new EventEmitter<Number>();

    constructor() {
        this.initMenu();
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
                debugger;
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
}