import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { EmployerService } from 'src/app/Services/employer.service';
import { NotificationService } from 'src/app/Services/notification.service';

@Component({
  selector: 'app-employer-add',
  // imports: [],
  standalone: false,
  templateUrl: './employer-add.component.html',
  styleUrl: './employer-add.component.scss',
})
export class EmployerAddComponent {
  employerForm = new FormGroup({
    companyName: new FormControl('', Validators.required),
    websiteLink: new FormControl(null),
    type: new FormControl(null, Validators.required),
    interviewStatus: new FormControl(null),
    ctcOffered: new FormControl(null),
    location: new FormControl(''),
    interviewLevel: new FormControl(''),
    offeredRole: new FormControl(''),
    notes: new FormControl(''),
    dateOfJoining: new FormControl(null),
    hrDetail: new FormGroup({
      name: new FormControl('', Validators.required),
      phoneNumber: new FormControl(null),
      emailId: new FormControl(null)
    })
  });

  constructor(
    private employerService: EmployerService,
    private router: Router,
    private notification: NotificationService
  ) { }

  save() {
    if (this.employerForm.invalid) {
      this.notification.error('Please fill all required fields');
      return;
    }

    this.employerService.addEmployer(this.employerForm.value)
      .subscribe({
        next: () => {
          this.notification.success('Employer added successfully!!');
          this.router.navigate(['/home']);
        },
        error: (err) => {
          console.error(err);
          this.notification.error('Failed to add employer. Please try again.');
        }
      });
  }

  cancel() {
    this.router.navigate(['/home']);
  }
}
