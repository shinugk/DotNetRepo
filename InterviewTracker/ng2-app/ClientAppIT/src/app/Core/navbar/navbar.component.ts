import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/Auth/auth.service';
import { UserService } from '../../Services/user.service';
import { UserProfile } from 'src/app/Interfaces/profile.model';
import { environment } from 'src/environment';

@Component({
  selector: 'app-navbar',
  // imports: [],
  standalone: false,
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent implements OnInit {
  profileImageUrl: string = '';
  name: string = '';

  constructor(private authService: AuthService, private userService: UserService) { }

  ngOnInit(): void {
    this.userService.getMyProfile().subscribe(res => {
      this.profileImageUrl = res?.profilePictureUrl;
      this.name = res?.name;
    })
  }

  logout() {
    this.authService.logout();
    //this.router.navigate(['/login']);
  }

  openSwagger(): void {
    window.open(`${environment.apiBaseUrl}/swagger/index.html`, '_blank');
  }

}
