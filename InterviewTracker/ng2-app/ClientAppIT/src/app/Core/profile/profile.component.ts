import { Component } from '@angular/core';
import { UserProfile } from 'src/app/Interfaces/profile.model';
import { UserService } from 'src/app/Services/user.service';
import { MatDialog } from '@angular/material/dialog';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { ResumePreviewDialogComponent } from '../resume-preview-dialog/resume-preview-dialog.component';

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


  constructor(private userService: UserService,
    private dialog: MatDialog,
    private sanitizer: DomSanitizer) { }

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
      },
      error: (err) => {
        console.error('Resume download failed', err);
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
      }
    });
  }

}
