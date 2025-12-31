// import { HttpClient } from '@angular/common/http';
// import { Injectable } from '@angular/core';
// import { OAuthService } from 'angular-oauth2-oidc';
// import { authConfig } from './auth.config';
// import { from, of, switchMap } from 'rxjs';
// import { environment } from 'src/environment';

// @Injectable({ providedIn: 'root' })
  
// export class AuthService {

//   constructor(
//     private oauth: OAuthService,
//     private http: HttpClient
//   ) {
//     this.oauth.configure(authConfig);
//     this.oauth.loadDiscoveryDocument();
//   }

//   loginWithGoogle() {
//     this.oauth.initLoginFlow();
//   }

//   handleLoginCallback() {
//   console.log("URIIIIII"+window.location.origin);
//   return from(this.oauth.tryLoginImplicitFlow()).pipe(
//     switchMap(() => {
//       const idToken = this.oauth.getIdToken();

//       if (!idToken) {
//         return of(null);
//       }

//       return this.http.post<any>(
//         `${environment.apiBaseUrl}/api/auth/google`,
//         { idToken }
//       );
//     })
//   );
// }

//   storeJwt(jwt: string) {
//     localStorage.setItem('jwt', jwt);
//   }

//   logout() {
//     //localStorage.clear();
//     // Clear your app JWT
//     localStorage.removeItem('jwt');

//     // Optional: clear OAuth state
//     sessionStorage.clear();
//   }

//   isLoggedIn(): boolean {
//     return !!localStorage.getItem('jwt');
//   }

// }

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environment';

declare global {
  interface Window {
    google: any;
  }
}

@Injectable({ providedIn: 'root' })
  
export class AuthService {

  private googleLoaded = false;

  constructor(private http: HttpClient) {}

  private loadGoogleScript(): Promise<void> {
    return new Promise((resolve, reject) => {
      if (window.google && window.google.accounts?.id) {
        this.googleLoaded = true;
        resolve();
        return;
      }

      const script = document.createElement('script');
      script.src = 'https://accounts.google.com/gsi/client';
      script.async = true;
      script.defer = true;

      script.onload = () => {
        this.googleLoaded = true;
        resolve();
      };

      script.onerror = () => reject('Google script failed to load');

      document.head.appendChild(script);
    });
  }

  async initGoogleLogin() {
    await this.loadGoogleScript();

    window.google.accounts.id.initialize({
      client_id: '250228091409-kugdd045lkeh9i8uvrkuhaa28vc7jr4r.apps.googleusercontent.com',
      callback: (response: any) => {
        this.sendIdTokenToBackend(response.credential);
      },
      use_fedcm_for_prompt: false
    });
  }

  async login() {
    if (!this.googleLoaded) {
      await this.initGoogleLogin();
    }

    window.google.accounts.id.prompt();
  }

  private sendIdTokenToBackend(idToken: string) {
    this.http.post<any>(
      `${environment.apiBaseUrl}/api/auth/google`,
      { idToken }
    ).subscribe(res => {
      localStorage.setItem('jwt', res.token);
      window.location.href = '/home';
    });
  }

    logout() {
    //localStorage.clear();
    // Clear your app JWT
    localStorage.removeItem('jwt');

    // Optional: clear OAuth state
    sessionStorage.clear();
  }
}
