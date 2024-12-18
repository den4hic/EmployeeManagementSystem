import {Component, inject} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {AuthService} from "../../services/auth.service";
import {LoginDto} from "../../services/dtos/login.dto";
import {JwtService} from "../../services/jwt.service";
import {Router} from "@angular/router";
import {MatSnackBar} from "@angular/material/snack-bar";
import {SignalRService} from "../../services/signal-r.service";
import {UserService} from "../../services/user.service";

@Component({
  selector: 'app-login',
  templateUrl: "./login.component.html",
  styleUrl: "./login.component.css"
})
export class LoginComponent {
  loginForm: FormGroup;
  router = inject(Router);
  hidePassword = true;
  private snackBar = inject(MatSnackBar);

  constructor(private userService: UserService, private authService: AuthService, private jwtService: JwtService, private fb: FormBuilder, private signalRService: SignalRService) {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(20)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      const loginData: LoginDto = this.loginForm.value;
      this.authService.login(loginData).subscribe(
        response => {
          console.log('Login successful', response);
          console.log('User role:', this.jwtService.getUserRole());
          this.signalRService.connect();
          this.userService.getUserInfo(true);
          this.router.navigate(['/']);
        },
        error => {
          console.error('Login failed', error);
          let errorMessage = 'An unknown error occurred';
          if (error.status === 401) {
            errorMessage = 'Invalid credentials';
          } else if (error.error && error.error.errors) {
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
