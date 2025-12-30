import { Component } from '@angular/core';
import { UserProfile } from 'src/app/Interfaces/profile.model';
import { UserService } from 'src/app/Services/user.service';

@Component({
  selector: 'app-profile',
  // imports: [],
  standalone: false,
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
})
  
  
export class ProfileComponent {

  userProfile!: UserProfile;
  isLoading = true;

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.userService.getMyProfile().subscribe({
      next: (profile: UserProfile) => {
        this.userProfile = profile;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to load profile', err);
        this.isLoading = false;
      }
    });
  }

  displayValue(value: any): any {
    return value ?? '—';
  }

}
