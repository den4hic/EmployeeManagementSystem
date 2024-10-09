import { Component, inject, OnInit } from '@angular/core';
import { UserService } from "../../services/user.service";
import { UserDto } from "../../services/dtos/user.dto";
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {ConfirmDialogComponent} from "../../shared/confirm-dialog/confirm-dialog.component";
import {MatDialog} from "@angular/material/dialog";
import {UserPhotoService} from "../../services/user-photo.service";
import {PhotoDialogComponent} from "../../shared/photo-dialog/photo-dialog.component";
import {UserPhotoDto} from "../../services/dtos/user-photo.dto";
import {FileUploadRequestDto} from "../../services/dtos/file-upload-request.dto";

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

  constructor(private dialog: MatDialog,
              private userPhotoService: UserPhotoService) {
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


  confirmDeletePhoto() {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '300px'
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.deleteUser(this.userModel?.userPhoto?.id!);
      }
    });
  }

  private deleteUser(photoId: number) {
    this.userPhotoService.deleteTask(photoId).subscribe({
      next: () => {
        this.loadUserInfo(true);
        console.log('User photo deleted');
      },
      error: (error) => {
        console.error('Failed to delete user photo', error);
      }
    });
  }

  openPhotoDialog(): void {
    const dialogRef = this.dialog.open(PhotoDialogComponent, {
      width: '400px',
      data: { userPhoto: this.userModel?.userPhoto }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.updatePhoto(result);
      }
    });
  }

  private updatePhoto(result: File) {
    console.log('Updating photo', result);
    if (this.userModel && this.userModel.userPhoto) {
      const fileUploadRequest: FileUploadRequestDto = {
        file: result,
        userId: this.userModel.id
      };
      this.userPhotoService.updateUserPhoto(fileUploadRequest).subscribe({
        next: () => {
          this.loadUserInfo(true);
          console.log('User photo updated');
        },
        error: (error) => {
          console.error('Failed to update user photo', error);
        }
      });
    } else if (this.userModel && !this.userModel.userPhoto) {
      const fileUploadRequest: FileUploadRequestDto = {
        file: result,
        userId: this.userModel.id
      };
      console.log('Creating photo', fileUploadRequest);
      this.userPhotoService.createUserPhoto(fileUploadRequest).subscribe({
        next: () => {
          this.loadUserInfo(true);
          console.log('User photo created');
        },
        error: (error) => {
          console.error('Failed to create user photo', error);
        }
      });
    }
    console.log(result);
  }
}
