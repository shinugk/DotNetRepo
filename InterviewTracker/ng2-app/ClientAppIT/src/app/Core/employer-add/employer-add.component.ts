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
    websiteLink: new FormControl(''),
    type: new FormControl(''),
    interviewStatus: new FormControl(''),
    ctcOffered: new FormControl(null),
    location: new FormControl(''),

    hrDetail: new FormGroup({
      name: new FormControl('',Validators.required),
      phoneNumber: new FormControl(''),
      emailId: new FormControl('')
    })
  });

  constructor(
    private employerService: EmployerService,
    private router: Router
  ) {}

  save() {
    if (this.employerForm.invalid) return;

    this.employerService.addEmployer(this.employerForm.value)
      .subscribe(() => this.router.navigate(['/home']));
  }

  cancel() {
    this.router.navigate(['/home']);
  }
}
