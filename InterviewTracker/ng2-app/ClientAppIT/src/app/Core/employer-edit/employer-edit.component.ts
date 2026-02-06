import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployerService } from 'src/app/Services/employer.service';
import { Employer } from '../../Interfaces/employer.model';
import { NotificationService } from 'src/app/Services/notification.service';

@Component({
  selector: 'app-employer-edit',
  // imports: [],
  standalone: false,
  templateUrl: './employer-edit.component.html',
  styleUrl: './employer-edit.component.scss',
})
export class EmployerEditComponent {
  types = ['Service Based', 'Product Based', 'Consulting'];
  statuses = ['Selected', 'Not Selected', 'In-Progress'];

  employerForm!: FormGroup;
  employerId!: number;

  constructor(
    private route: ActivatedRoute,
    private employerService: EmployerService,
    private router: Router,
    private notification: NotificationService
  ) { }

  ngOnInit() {
    this.employerId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadEmployer();
  }

  loadEmployer() {
    this.employerService.getEmployerById(this.employerId)
      .subscribe((data: Employer) => {
        this.employerForm = new FormGroup({
          companyName: new FormControl(data.companyName, Validators.required),
          websiteLink: new FormControl(data.websiteLink),
          type: new FormControl(data.type),
          interviewStatus: new FormControl(data.interviewStatus),
          ctcOffered: new FormControl(data.ctcOffered),
          location: new FormControl(data.location),
          interviewLevel: new FormControl(data.interviewLevel),
          offeredRole: new FormControl(data.offeredRole),
          dateOfJoining: new FormControl(data.dateOfJoining),
          notes: new FormControl(data.notes),
          hrDetail: new FormGroup({
            name: new FormControl(data.hrDetail?.name, Validators.required),
            phoneNumber: new FormControl(data.hrDetail?.phoneNumber),
            emailId: new FormControl(data.hrDetail?.emailId)
          })
        });
      });
  }

  update() {
    if (this.employerForm.invalid) {
      this.notification.error('Please fill all required fields');
      return;
    }

    this.employerService
      .updateEmployer(this.employerId, this.employerForm.value)
      .subscribe({
        next: () => {
          this.notification.success('Employer updated successfully');
          this.router.navigate(['/home']);
        },
        error: (err) => {
          console.error(err);
          this.notification.error('Failed to update employer. Please try again.');
        }
      });
  }

  cancel() {
    this.router.navigate(['/home']);
  }
}
