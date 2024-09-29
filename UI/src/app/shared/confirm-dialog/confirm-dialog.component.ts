import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-confirm-dialog',
  template: `
    <h2 mat-dialog-title>Submit action</h2>
    <mat-dialog-content>
      <p>Are you sure?</p>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>No</button>
      <button mat-raised-button color="warn" [mat-dialog-close]="true">Yes</button>
    </mat-dialog-actions>
  `,
  styles: [`
    mat-dialog-content { padding: 20px; }
    mat-dialog-actions { margin-bottom: 10px; }
  `]
})
export class ConfirmDialogComponent {
  constructor(public dialogRef: MatDialogRef<ConfirmDialogComponent>) {}
}
