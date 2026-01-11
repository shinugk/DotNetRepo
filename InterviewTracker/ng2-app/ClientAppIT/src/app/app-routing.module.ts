import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './Core/login/login.component';
import { AuthGuard } from './Guards/auth.guard';
import { HomeComponent } from './Core/home/home.component';
import { AppComponent } from './app.component';
import { ProfileComponent } from './Core/profile/profile.component';
import { ProfileEditComponent } from './Core/profile-edit/profile-edit.component';
import { EmployerAddComponent } from './Core/employer-add/employer-add.component';
import { EmployerEditComponent } from './Core/employer-edit/employer-edit.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'app',
    component: AppComponent,
    canActivate: [AuthGuard]   //this is IMPORTANT which redirects to login if user is not logged in
  },
  {
    path: 'home',
    component: HomeComponent,
    canActivate: [AuthGuard] 
  },
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'profile/edit',
    component: ProfileEditComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'employer/add',
    component: EmployerAddComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'employer/edit',
    component: EmployerEditComponent,
    canActivate: [AuthGuard]
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
  
export class AppRoutingModule { }
