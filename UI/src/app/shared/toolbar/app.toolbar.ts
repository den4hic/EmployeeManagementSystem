import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import {SignalRService} from "../../services/signal-r.service";

@Component({
  selector: 'app-toolbar',
  templateUrl: 'app.toolbar.html',
  styleUrl: 'app.toolbar.css'
})
export class ToolbarComponent {
  constructor(
    private authService: AuthService,
    private signalRService: SignalRService,
    private router: Router
  ) {}

  isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
  }

  goToProfile(): void {
    this.router.navigate(['/profile']);
  }

  logout(): void {
    this.authService.logout().subscribe(() => {
      this.router.navigate(['/login']);
      this.signalRService.disconnect();
    });
  }
}
