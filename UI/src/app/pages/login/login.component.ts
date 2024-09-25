import {Component, inject} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {AuthService} from "../../services/auth.service";
import {LoginDto} from "../../services/dtos/login.dto";
import {JwtService} from "../../services/jwt.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-login',
  templateUrl: "./login.component.html",
  styleUrl: "./login.component.css"
})
export class LoginComponent {
  loginForm: FormGroup;
  router = inject(Router);

  constructor(private authService: AuthService, private jwtService: JwtService, private fb: FormBuilder) {
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
          this.router.navigate(['/']);
        },
        error => {
          console.error('Login failed', error);
        }
      );
    }
  }
}
