import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from "../../services/auth.service";
import { RegisterDto } from "../../services/dtos/register.dto";
import { Router } from "@angular/router";
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-registration',
  templateUrl: "./registration.component.html",
  styleUrl: "./registration.component.css"
})
export class RegistrationComponent {
  registrationForm: FormGroup;
  router = inject(Router);
  private snackBar = inject(MatSnackBar);

  constructor(private authService: AuthService, private fb: FormBuilder) {
    this.registrationForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(20)]],
      email: ['', [Validators.required, Validators.email]],
      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      phoneNumber: [null, [Validators.pattern(/^0\d{9}$/)]],
      password: [null, [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onSubmit() {
    if (this.registrationForm.valid) {
      const registrationData: RegisterDto = this.registrationForm.value;
      this.authService.register(registrationData).subscribe(
        response => {
          console.log('Register successful', response);
          this.router.navigate(['/login']);
        },
        error => {
          console.error('Register failed', error);
          let errorMessage = 'An unknown error occurred';
          if (error.error && error.error.errors) {
            const firstErrorKey = Object.keys(error.error.errors)[0];
            if (firstErrorKey) {
              errorMessage = error.error.errors[firstErrorKey][0];
            }
          }
          this.showErrorMessage(errorMessage);
        }
      );
    }
  }

  private showErrorMessage(message: string) {
    this.snackBar.open(message, 'Close', {
      duration: 5000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
    });
  }
}
