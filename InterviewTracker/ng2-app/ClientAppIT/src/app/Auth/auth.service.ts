import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { authConfig } from './auth.config';
import { from, of, switchMap } from 'rxjs';

@Injectable({ providedIn: 'root' })
  
export class AuthService {

  constructor(
    private oauth: OAuthService,
    private http: HttpClient
  ) {
    this.oauth.configure(authConfig);
    this.oauth.loadDiscoveryDocument();
  }

  loginWithGoogle() {
    this.oauth.initLoginFlow();
  }

  handleLoginCallback() {
  console.log("URIIIIII"+window.location.origin);
  return from(this.oauth.tryLoginImplicitFlow()).pipe(
    switchMap(() => {
      const idToken = this.oauth.getIdToken();

      if (!idToken) {
        return of(null);
      }

      return this.http.post<any>(
        'https://localhost:7257/api/auth/google',
        { idToken }
      );
    })
  );
}

  storeJwt(jwt: string) {
    localStorage.setItem('jwt', jwt);
  }

  logout() {
    //localStorage.clear();
    // Clear your app JWT
    localStorage.removeItem('jwt');

    // Optional: clear OAuth state
    sessionStorage.clear();
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('jwt');
  }

}

