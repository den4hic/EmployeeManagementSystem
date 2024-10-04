import { Component, inject, OnInit } from '@angular/core';
import { UserService } from "../../services/user.service";
import { UserDto } from "../../services/dtos/user.dto";
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  private userService = inject(UserService);
  private fb = inject(FormBuilder);

  public userModel: UserDto | null = null;
  public editMode = false;
  public userForm: FormGroup;

  constructor() {
    this.userForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: [null, [Validators.pattern(/^0\d{9}$/)]],
    });
  }

  ngOnInit() {
    this.loadUserInfo();
  }

  private loadUserInfo(isNew: boolean = false): void {
    this.userService.getUserInfo(isNew).subscribe({
      next: (user) => {
        this.userModel = user;
        this.userForm.patchValue(user);
        console.log('User info', user);
      },
      error: (error) => {
        console.error('Failed to get user info', error);
      }
    });
  }

  public toggleEditMode(): void {
    this.editMode = !this.editMode;
    if (!this.editMode) {
      this.userForm.patchValue(this.userModel!);
    }
  }

  public saveChanges(): void {
    if (this.userForm.valid) {
      const updatedUser: UserDto = { ...this.userModel!, ...this.userForm.value };
      this.userService.updateUserInfo(updatedUser).subscribe({
        next: () => {
          this.loadUserInfo(true);
          this.editMode = false;
          console.log('User info updated', updatedUser);
        },
        error: (error) => {
          console.error('Failed to update user info', error);
        }
      });
    }
  }
}
