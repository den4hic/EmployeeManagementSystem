import {Component, inject} from '@angular/core';
import {UserService} from "../../services/user.service";
import {UserModel} from "../models/user.model";
import {UserDto} from "../../services/dtos/user.dto";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent {
  private userService = inject(UserService);
  // private userModel : UserModel;

  public userModel : UserDto | null = null;
  ngOnInit() {
    const user = this.userService.getUserInfo();

    if (!user) {
      console.error('User is not authenticated');
      return;
    }

    user.subscribe(
      user => {
        this.userModel = user;
        console.log('User info', user);
      },
      error => {
        console.error('Failed to get user info', error);
      }
    );
  }
}
