import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
    providedIn: 'root'
})
export class NotificationService {

    private readonly defaultDuration = 3000;

    constructor(private snackBar: MatSnackBar) { }

    success(message: string) {
        this.open(message, 'success-snackbar');
    }

    error(message: string) {
        this.open(message, 'error-snackbar');
    }

    info(message: string) {
        this.open(message, 'info-snackbar');
    }

    private open(message: string, panelClass: string) {
        this.snackBar.open(message, 'Close', {
            duration: this.defaultDuration,
            panelClass
        });
    }
}
