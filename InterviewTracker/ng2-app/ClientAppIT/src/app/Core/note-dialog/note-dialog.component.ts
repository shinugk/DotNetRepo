import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  standalone: false,
  selector: 'app-note-dialog',
  // imports: [],
  templateUrl: './note-dialog.component.html',
  styleUrl: './note-dialog.component.scss',
})
export class NoteDialogComponent {
  constructor(
    private dialogRef: MatDialogRef<NoteDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {
      title?: string;
      content: string;
    }
  ) { }

  save() {
    if (!this.data.content || !this.data.content.trim()) {
      return;
    }
    this.dialogRef.close(this.data);
  }

  cancel() {
    this.dialogRef.close();
  }
}
