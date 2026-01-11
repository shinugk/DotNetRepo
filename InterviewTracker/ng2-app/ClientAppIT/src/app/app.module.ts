import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { JsonPipe } from '@angular/common';

import { MaterialModule } from 'src/material/material.module';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { JwtInterceptor } from './Interceptors/jwt.interceptor';
import { AuthService } from './Auth/auth.service';

import { OAuthModule } from 'angular-oauth2-oidc';
import { HomeComponent } from './Core/home/home.component';
import { NavbarComponent } from './layout/navbar/navbar.component';
import { ProfileComponent } from './Core/profile/profile.component';
import { UserService } from './Services/user.service';

import { ReactiveFormsModule } from '@angular/forms';

import { ProfileEditComponent } from './Core/profile-edit/profile-edit.component';
import { EmployerEditComponent } from './Core/employer-edit/employer-edit.component';
import { EmployerGridComponent } from './Core/employer-grid/employer-grid.component';
import { HrDialogViewComponent } from './Core/hr-dialog-view/hr-dialog-view.component'; 
import { EmployerAddComponent } from './Core/employer-add/employer-add.component';
// import { AgGridModule } from 'ag-grid-angular';
// import 'ag-grid-enterprise';


import { EmployerService } from './Services/employer.service';



@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavbarComponent,
    ProfileComponent,
    ProfileEditComponent,
    EmployerEditComponent,
    EmployerGridComponent,
    HrDialogViewComponent,
    EmployerAddComponent
  ],
  imports: [
    //  AgGridModule,               //for building Ag Grid
    BrowserModule,
    AppRoutingModule,
    OAuthModule.forRoot(),
    HttpClientModule,
    JsonPipe,
    MaterialModule, //keeping this module separate and then importing here
    ReactiveFormsModule,
],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true // ✅ VERY IMPORTANT
    },
    AuthService,
    UserService,
    EmployerService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
