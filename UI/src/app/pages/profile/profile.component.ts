import { Component, inject, OnInit } from '@angular/core';
import { UserService } from "../../services/user.service";
import { UserDto } from "../../services/dtos/user.dto";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  private userService = inject(UserService);
  public userModel: UserDto | null = null;

  ngOnInit() {
    this.loadUserInfo();
  }

  private loadUserInfo(): void {
    this.userService.getUserInfo().subscribe({
      next: (user) => {
        this.userModel = user;
        console.log('User info', user);
      },
      error: (error) => {
        console.error('Failed to get user info', error);
      }
    });
  }
}
