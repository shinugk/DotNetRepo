import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/Auth/auth.service';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environment';

@Component({
  selector: 'app-home',
// imports: [],
  standalone: false,
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit{

  healthResponse: any;
  userResponse: any;
  loading = false;
  error: string | null = null;

   constructor(
    private authService: AuthService,
    private router: Router,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    this.getdbhealth();
  }

  getdbhealth()
  {
    this.loading = true;
    return this.http.get<any>(`${environment.apiBaseUrl}/api/DbHealthCheck`).subscribe({
      next: (res) => {
        this.healthResponse = res;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to fetch DB health';
        console.error(err);
        this.loading = false;
      }
    });;
  }
}
