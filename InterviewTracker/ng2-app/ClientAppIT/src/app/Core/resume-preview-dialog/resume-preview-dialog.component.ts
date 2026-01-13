import { Component, Inject, OnDestroy } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-resume-preview-dialog',
  // imports: [],
  standalone: false,
  templateUrl: './resume-preview-dialog.component.html',
  styleUrl: './resume-preview-dialog.component.scss',
})
export class ResumePreviewDialogComponent  implements OnDestroy {
   constructor(
    @Inject(MAT_DIALOG_DATA) public data: { pdfUrl: string }
  ) {}

  ngOnDestroy(): void {
    // Clean up object URL
    URL.revokeObjectURL(this.data.pdfUrl);
  }
}
