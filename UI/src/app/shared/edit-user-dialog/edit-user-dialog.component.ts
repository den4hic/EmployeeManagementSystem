import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UserDto } from '../../services/dtos/user.dto';
import { UserService } from '../../services/user.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-edit-user-dialog',
  templateUrl: './edit-user-dialog.component.html',
  styleUrl: './edit-user-dialog.component.css'
})
export class EditUserDialogComponent implements OnInit {
  userForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private snackBar: MatSnackBar,
    public dialogRef: MatDialogRef<EditUserDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { user: UserDto }
  ) {
    this.userForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.pattern(/^0\d{9}$/)]
    });
  }

  ngOnInit() {
    this.userForm.patchValue(this.data.user);
  }

  onSubmit() {
    if (this.userForm.valid) {
      const updatedUser: UserDto = {
        ...this.data.user,
        ...this.userForm.value
      };

      this.userService.updateUserInfo(updatedUser).subscribe({
        next: () => {
          this.snackBar.open('User updated successfully', 'Close', { duration: 3000 });
          this.dialogRef.close(updatedUser);
        },
        error: (error) => {
          console.error('Error updating user', error);
          this.snackBar.open('Error updating user', 'Close', { duration: 3000 });
        }
      });
    }
  }
}
