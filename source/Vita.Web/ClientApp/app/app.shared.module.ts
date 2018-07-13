
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
//import { BrowserAnimationsModule } from '@angular/animations';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { SearchComponent } from './components/search/search.component';
import { SearchService } from './components/search/search.service';
import { BankStatementClient } from './vita.api';
import { ChargeClient } from './vita.api';
import { Charge } from "./vita.api";

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        SearchComponent
        
    ],
  providers: [
    BankStatementClient,
    ChargeClient,
    SearchService
    ],
    imports: [
        HttpClientModule,
        FormsModule,
        BrowserAnimationsModule,
        BrowserModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'search', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'search', component: SearchComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ]
})
export class AppModuleShared {
}
