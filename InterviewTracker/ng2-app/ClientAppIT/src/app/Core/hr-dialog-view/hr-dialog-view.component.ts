import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogActions, MatDialogContent } from '@angular/material/dialog';

@Component({
  selector: 'app-hr-dialog-view',
  // imports: [MatDialogActions, MatDialogContent],
  standalone: false,
  templateUrl: './hr-dialog-view.component.html',
  styleUrl: './hr-dialog-view.component.scss',
})
export class HrDialogViewComponent {

   constructor(
    public dialogRef: MatDialogRef<HrDialogViewComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }
  
  close() {
    this.dialogRef.close();
  }
}
