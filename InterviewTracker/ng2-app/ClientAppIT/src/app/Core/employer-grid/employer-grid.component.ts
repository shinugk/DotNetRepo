import { Component, OnInit, ViewChild } from '@angular/core';
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
export class EmployerGridComponent implements OnInit {
  displayedColumns: string[] = [
    'companyName',
    // 'websiteLink',
    'type',
    'hrDetails',
    'interviewStatus',
    'ctcOffered',
    'location',
    'interviewLevel',
    'offeredRole',
    'dateOfJoining',
    'notes',
    'actions'
  ];

  dataSource = new MatTableDataSource<any>([]);
  statusList = ['All', 'Selected', 'Not Selected', 'In-Progress'];
  selectedStatus = 'All';

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private employerService: EmployerService,
    private dialog: MatDialog,
    private router: Router
  ) { }

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

  applyFilter(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.dataSource.filter = value.trim().toLowerCase();
  }

  filterByStatus(status: string) {
    this.selectedStatus = status;

    if (status === 'All') {
      this.dataSource.filter = '';
    } else {
      this.dataSource.filterPredicate = (data: any) =>
        data.interviewStatus === status;
      this.dataSource.filter = status;
    }
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'Selected':
        return 'status-selected';
      case 'Not Selected':
        return 'status-not-selected';
      case 'In-Progress':
        return 'status-in-progress';
      default:
        return '';
    }
  }

}