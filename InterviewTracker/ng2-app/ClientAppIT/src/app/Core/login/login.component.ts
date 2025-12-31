import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/Auth/auth.service';


@Component({
  selector: 'app-login',
  //imports: [],
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
  
export class LoginComponent implements OnInit {

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.authService.initGoogleLogin();
  }

  login() {
    this.authService.login();
  }

}
