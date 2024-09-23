import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {AuthService} from "../../services/auth.service";
import {LoginDto} from "../../services/dtos/login.dto";
import {RegisterDto} from "../../services/dtos/register.dto";

@Component({
  selector: 'app-registration',
  templateUrl: "./registration.component.html",
  styleUrl: "./registration.component.css"
})
export class RegistrationComponent {
  registrationForm: FormGroup;

  constructor(private authService: AuthService, private fb: FormBuilder) {
    this.registrationForm = this.fb.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      phoneNumber: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onSubmit() {
    if (this.registrationForm.valid) {
      const registrationData: RegisterDto = this.registrationForm.value;
      this.authService.register(registrationData).subscribe(
        response => {
          console.log('Register successful', response);
        },
        error => {
          console.error('Register failed', error);
        }
      );
    }
  }
}
