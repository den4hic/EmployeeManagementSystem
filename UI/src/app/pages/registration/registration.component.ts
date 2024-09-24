import {Component, inject} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {AuthService} from "../../services/auth.service";
import {RegisterDto} from "../../services/dtos/register.dto";
import {Router} from "@angular/router";

@Component({
  selector: 'app-registration',
  templateUrl: "./registration.component.html",
  styleUrl: "./registration.component.css"
})
export class RegistrationComponent {
  registrationForm: FormGroup;
  router = inject(Router);

  constructor(private authService: AuthService, private fb: FormBuilder) {
    this.registrationForm = this.fb.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      phoneNumber: [''],
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
          this.router.navigate(['/login']);
        },
        error => {
          console.error('Register failed', error);
        }
      );
    }
  }
}
