import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ResultsComponent } from './component/results-component/results-component';
import { HttpClientModule } from '@angular/common/http'; 
import { MovieFilter } from './pipes/movieFilter-pipe';
import { FormsModule } from '@angular/forms';
import { MovieComponent } from './component/movie-component/movie-component';
import { NavbarComponent } from './component/navbar-component/navbar-component';
import { FiltersComponent } from './component/filters-component/filters-component';
import { UpdateModalComponent } from './component/update-modal-component/update-modal-component';
import { registerLocaleData } from '@angular/common';
import localePl from '@angular/common/locales/pl';
import localePlExtra from '@angular/common/locales/extra/pl';

registerLocaleData(localePl, 'pl-PL', localePlExtra);

@NgModule({
  declarations: [
    AppComponent,
    ResultsComponent,
    MovieComponent,
    NavbarComponent,
    FiltersComponent,
    MovieFilter,
    UpdateModalComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CommonModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [{provide: LOCALE_ID, useValue: 'pl-PL' }],
  bootstrap: [AppComponent]
})
export class AppModule { }
