// photo-dialog.component.ts
import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-photo-dialog',
  templateUrl: './photo-dialog.component.html',
  styleUrls: ['./photo-dialog.component.scss']
})
export class PhotoDialogComponent {
  dragActive = false;
  photo: File | null = null;
  previewUrl: string | null = null;
  readonly MAX_SIZE_MB = 5;
  readonly MAX_SIZE_BYTES = this.MAX_SIZE_MB * 1024 * 1024; // 5MB in bytes

  constructor(
    public dialogRef: MatDialogRef<PhotoDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private snackBar: MatSnackBar
  ) {}

  onNoClick(): void {
    this.dialogRef.close();
  }

  onSaveClick(): void {
    this.dialogRef.close(this.photo);
  }

  onFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      this.validateAndHandleFile(file);
    }
  }

  handleDrag(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.dragActive = event.type === 'dragenter' || event.type === 'dragover';
  }

  handleDrop(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.dragActive = false;

    const files = event.dataTransfer?.files;
    if (files?.length) {
      this.validateAndHandleFile(files[0]);
    }
  }

  private validateAndHandleFile(file: File): void {
    if (!file.type.startsWith('image/')) {
      this.showError('Please upload an image file');
      return;
    }

    if (file.size > this.MAX_SIZE_BYTES) {
      this.showError(`File size must be less than ${this.MAX_SIZE_MB}MB`);
      return;
    }

    this.handleFile(file);
  }

  private handleFile(file: File): void {
    this.photo = file;
    const reader = new FileReader();
    reader.onload = () => {
      this.previewUrl = reader.result as string;
    };
    reader.readAsDataURL(file);
  }

  private showError(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 5000,
      panelClass: ['error-snackbar'],
      horizontalPosition: 'center',
      verticalPosition: 'bottom'
    });
  }

  removePhoto(): void {
    this.photo = null;
    this.previewUrl = null;
  }

  formatFileSize(size: number): string {
    return `${(size / (1024 * 1024)).toFixed(2)}MB`;
  }
}
