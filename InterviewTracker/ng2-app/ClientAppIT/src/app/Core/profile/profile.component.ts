import { Component } from '@angular/core';
import { UserProfile } from 'src/app/Interfaces/profile.model';
import { UserService } from 'src/app/Services/user.service';
import { MatDialog } from '@angular/material/dialog';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { ResumePreviewDialogComponent } from '../resume-preview-dialog/resume-preview-dialog.component';
import { NotificationService } from 'src/app/Services/notification.service';

@Component({
  selector: 'app-profile',
  // imports: [],
  standalone: false,
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
})


export class ProfileComponent {

  userProfile!: UserProfile;
  isLoading = true;
  isDownloading = false;
  skillList: string[] = [];
  isUploading = false;

  constructor(private userService: UserService,
    private dialog: MatDialog,
    private sanitizer: DomSanitizer,
    private notification: NotificationService
  ) { }

  ngOnInit(): void {
    this.loadProfile();

  }

  loadProfile(): void {
    this.userService.getMyProfile().subscribe({
      next: (profile: UserProfile) => {
        this.userProfile = profile;
        this.isLoading = false;
        this.userProfile.resumeFileName = 'resume.pdf'; //-> Need to change

        if (this.userProfile?.skills) {
          this.skillList = this.userProfile.skills
            .split(',')
            .map(s => s.trim())
            .filter(s => s);
        }
      },
      error: (err) => {
        console.error('Failed to load profile', err);
        this.isLoading = false;
      }
    });
  }

  displayValue(value: any): any {
    return value ?? '—';
  }


  downloadResume(): void {
    this.isDownloading = true;

    this.userService.downloadResume().subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = this.userProfile.resumeFileName ?? 'resume.pdf';
        a.click();
        window.URL.revokeObjectURL(url);
        this.isDownloading = false;
        this.notification.success("File downloaded successfully!!");
      },
      error: (err) => {
        console.error('Resume download failed', err);
        this.notification.error("File download Failed!");
        this.isDownloading = false;
      }
    });
  }

  hasResume(): boolean {
    return !!this.userProfile?.resumeFileName;  //-> Add resumeFileName to user model
  }


  previewResume(): void {
    this.userService.downloadResume().subscribe({
      next: (blob) => {
        const url = URL.createObjectURL(blob);
        const safeUrl: SafeResourceUrl =
          this.sanitizer.bypassSecurityTrustResourceUrl(url);

        this.dialog.open(ResumePreviewDialogComponent, {
          width: '80vw',
          height: '90vh',
          data: {
            pdfUrl: safeUrl
          }
        });
      },
      error: (err) => {
        console.error('Failed to preview resume', err);
        this.notification.error('Failed to preview resume. Check resume is uploaded or not');
      }
    });
  }

  onResumeSelected(event: Event): void {
    const input = event.target as HTMLInputElement;

    if (!input.files || input.files.length === 0) return;

    const file = input.files[0];

    // Validation
    if (file.type !== 'application/pdf') {
      alert('Only PDF files are allowed');
      return;
    }

    if (file.size > 5 * 1024 * 1024) {
      alert('File size should be under 5MB');
      return;
    }

    this.uploadResume(file);

    // Reset input so same file can be reselected
    input.value = '';
  }

  uploadResume(file: File): void {
    this.isUploading = true;

    this.userService.uploadResume(file).subscribe({
      next: (response) => {
        //not called when there is no response
      },
      error: (err) => {
        console.error('Resume upload failed', err);
        this.notification.error("File cannot be uploaded");
        this.isUploading = false;
      },
      complete: () => {
        // ✅ This is called for 204 No Content
        this.isUploading = false;
        this.notification.success('File uploaded successfully!');
        this.loadProfile(); // refresh resumeFileName from backend
      }
    });
  }


}
