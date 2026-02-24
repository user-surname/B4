import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { Tabs } from './core/components/tabs/tabs';
import { Header } from './core/components/header/header';
import { Data } from './pages/data/data';
import { CommonModule } from '@angular/common';
import { JwtInterceptor } from './core/interceptors/jwt.interceptor';
import { Home } from './pages/home/home';
import { FormsModule } from '@angular/forms';
import { LkComponent } from './pages/lk/lk';

@NgModule({
  declarations: [
    App,
    Tabs,
    Header,
    Data,
    Home,
    LkComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CommonModule,
    FormsModule,       // 👈 aquí
    HttpClientModule,  // 👈 aquí
    
  ],
  providers: [
    provideBrowserGlobalErrorListeners(), { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
  ],
  bootstrap: [App]
})
export class AppModule { }
