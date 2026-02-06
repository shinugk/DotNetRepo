import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { ConfirmDialogComponent } from '../Core/confirm-dialog/confirm-dialog.component';

@Injectable({
    providedIn: 'root'
})
export class ConfirmationService {

    constructor(private dialog: MatDialog) { }

    confirm(
        title: string,
        message: string,
        confirmText = 'Confirm',
        cancelText = 'Cancel'
    ): Observable<boolean> {

        const dialogRef = this.dialog.open(ConfirmDialogComponent, {
            width: '400px',
            data: {
                title,
                message,
                confirmText,
                cancelText
            }
        });

        return dialogRef.afterClosed();
    }
}
