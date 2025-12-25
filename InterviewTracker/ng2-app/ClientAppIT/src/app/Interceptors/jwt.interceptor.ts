import { Injectable } from '@angular/core';
import { HttpInterceptor } from '@angular/common/http';
import { environment } from 'src/environment';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  //This is interceptor method which adds bearer token or jwt token which sets by .net app after login
  //this needs to added for every request
  intercept(req: any, next: any) {

     //Run ONLY for backend API calls
    if (!req.url.startsWith(environment.apiBaseUrl)) {
      return next.handle(req);
    }

    const token = localStorage.getItem('jwt');

    if (token) {
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    return next.handle(req);
  }
}
