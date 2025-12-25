import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../Core/AuthComponent/auth.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(): boolean {
   if (!localStorage.getItem('jwt')) {
    this.router.navigate(['/login']);
    return false;
  }
  return true;
  }
}