import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserProfile } from 'src/app/Interfaces/profile.model';
import { UserService } from 'src/app/Services/user.service';

@Component({
  selector: 'app-profile-edit',
  // imports: [],
  standalone: false,
  templateUrl: './profile-edit.component.html',
  styleUrl: './profile-edit.component.scss',
})
  
export class ProfileEditComponent {
  form!: FormGroup;
  user!: UserProfile;
  isLoading = true;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.userService.getMyProfile().subscribe({
      next: (profile) => {
        this.user = profile;

        this.form = this.fb.group({
          name: [{ value: profile.name, disabled: true }],
          email: [{ value: profile.email, disabled: true }],
          googleId: [{ value: profile.googleId, disabled: true }],

          age: [profile.age, [Validators.min(18), Validators.max(120)]],
          phoneNumber: [profile.phoneNumber, Validators.pattern(/^[0-9]{10}$/)],
          skills: [profile.skills],
          currentCompany: [profile.currentCompany],
        });

        this.isLoading = false;
      },
      error: () => (this.isLoading = false),
    });
  }

  save(): void {
    if (this.form.invalid) return;

    const payload = {
      age: this.form.value.age,
      phoneNumber: this.form.value.phoneNumber,
      skills: this.form.value.skills,
      currentCompany: this.form.value.currentCompany,
    };

    this.userService.updateUser(this.user.id, payload).subscribe({
      next: () => this.router.navigate(['/profile']),
      error: (err) => console.error('Update failed', err),
    });
  }

  cancel(): void {
    this.router.navigate(['/profile']);
  }
}
