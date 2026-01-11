import { Component, OnInit , ViewChild} from '@angular/core';
import { ColDef } from 'ag-grid-community';
import { EmployerService } from 'src/app/Services/employer.service';
import { EmployerEditComponent } from '../employer-edit/employer-edit.component'; 
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { HrDialogViewComponent } from '../hr-dialog-view/hr-dialog-view.component';
import { Employer } from 'src/app/Interfaces/employer.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-employer-grid',
  // imports: [],
  standalone: false,
  templateUrl: './employer-grid.component.html',
  styleUrl: './employer-grid.component.scss',
})
export class EmployerGridComponent implements OnInit{
   displayedColumns: string[] = [
     'companyName',
     'websiteLink',
     'type',
    'hrDetails',
    'interviewStatus',
    'ctcOffered',
    'location',
    'actions'
  ];

  dataSource = new MatTableDataSource<any>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private employerService: EmployerService,
    private dialog: MatDialog,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadEmployers();
  }

  loadEmployers() {
    this.employerService.getEmployersForUser().subscribe({
      next: data => {
        this.dataSource.data = data;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      },
      error: err => console.error(err)
    });
  }

  addEmployer() {
  this.router.navigate(['/employer/add']);
}

editEmployer(id: number) {
  this.router.navigate(['/employer/edit', id]);
}

  viewHrDetails(hrDetail: any) {
  this.dialog.open(HrDialogViewComponent, {
    width: '400px',
    data: hrDetail
  });
}
}