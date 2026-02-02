import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { EmployerService } from 'src/app/Services/employer.service';

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
    private router: Router
  ) { }

  save() {
    if (this.employerForm.invalid) return;

    this.employerService.addEmployer(this.employerForm.value)
      .subscribe(() => this.router.navigate(['/home']));
  }

  cancel() {
    this.router.navigate(['/home']);
  }
}
