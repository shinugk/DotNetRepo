import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Note, NoteService } from 'src/app/Services/note.service';
import { NoteDialogComponent } from '../note-dialog/note-dialog.component';
import { NotificationService } from 'src/app/Services/notification.service';
import { ConfirmationService } from 'src/app/Services/confirm-dialog.service';

@Component({
  standalone: false,
  selector: 'app-user-notes',
  // imports: [],
  templateUrl: './user-notes.component.html',
  styleUrl: './user-notes.component.scss',
})
export class UserNotesComponent {
  notes: Note[] = [];

  constructor(
    private noteService: NoteService,
    private dialog: MatDialog,
    private notify: NotificationService,
    private confirm: ConfirmationService
  ) { }

  ngOnInit() {
    this.loadNotes();
  }

  loadNotes() {
    this.noteService.getNotes().subscribe(notes => {
      this.notes = notes;
    });
  }

  addNote() {
    const dialogRef = this.dialog.open(NoteDialogComponent, {
      width: '500px',
      data: { title: '', content: '' }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.noteService.createNote(result).subscribe({
          next: () => {
            this.loadNotes();
            this.notify.success('Note added successfully!!');
          },
          error: () => {
            this.notify.error('Failed to add note');
          }
        });
      }
    });
  }

  editNote(note: Note) {
    const dialogRef = this.dialog.open(NoteDialogComponent, {
      width: '500px',
      data: { ...note }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.noteService.updateNote(note.id, result).subscribe({
          next: () => {
            this.loadNotes();
            this.notify.success('Note updated successfully');
          },
          error: () => {
            this.notify.error('Failed to update note');
          }
        });
      }
    });
  }

  deleteNote(note: Note) {
    this.confirm.confirm('Delete note', 'Are you sure you want to delete this note?')
      .subscribe(confirmed => {
        if (confirmed) {
          this.noteService.deleteNote(note.id).subscribe({
            next: () => {
              this.notes = this.notes.filter(n => n.id !== note.id);
              this.notify.success('Note deleted');
            },
            error: () => {
              this.notify.error('Failed to delete note');
            }
          });
        }
      });


  }

}
