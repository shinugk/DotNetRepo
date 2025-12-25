import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/Core/AuthComponent/auth.service';


@Component({
  selector: 'app-login',
  //imports: [],
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {

   constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.authService.handleLoginCallback().subscribe(res => {
    if (res?.token) {
      this.authService.storeJwt(res.token);
      this.router.navigate(['/home']);       //<-- Redirect to home oage after log in using OAuth
    }
    });
    
  }

  login() {
    this.authService.loginWithGoogle();
  }

}
