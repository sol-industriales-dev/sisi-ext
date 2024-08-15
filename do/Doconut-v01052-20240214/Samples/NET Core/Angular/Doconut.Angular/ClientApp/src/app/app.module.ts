import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {SharedModule} from '../app/shared/shared.module'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {HomeComponent} from './modules/home/home.component';
import {DocumentComponent} from './modules/document/document.component'

import { CoreModule } from '../app/core/core.module';

@NgModule({
  declarations: [
    AppComponent, HomeComponent, DocumentComponent
  ],
  imports: [
    CoreModule.forRoot(),
    BrowserModule,
    AppRoutingModule,
    SharedModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
