import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { JsonPipe } from '@angular/common';

import { MaterialModule } from 'src/material/material.module';
import { AgGridModule } from 'ag-grid-angular';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { JwtInterceptor } from './Interceptors/jwt.interceptor';
import { AuthService } from './Auth/auth.service';

import { OAuthModule } from 'angular-oauth2-oidc';
import { HomeComponent } from './Core/home/home.component';
import { NavbarComponent } from './layout/navbar/navbar.component';
import { ProfileComponent } from './Core/profile/profile.component';
import { UserService } from './Services/user.service';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavbarComponent,
    ProfileComponent,
    NavbarComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    OAuthModule.forRoot(),
    HttpClientModule,
    JsonPipe,
    MaterialModule,            //keeping this module separate and then importing here
    AgGridModule,               //for building Ag Grid
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true // ✅ VERY IMPORTANT
    },
    AuthService,
    UserService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
