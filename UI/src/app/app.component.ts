import { Component } from '@angular/core';
import {SignalRService} from "./services/signal-r.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  constructor(private signalRService: SignalRService) {
  }

  ngOnInit() {
    window.addEventListener('beforeunload', this.handleUnload.bind(this));
  }

  ngOnDestroy() {
    window.removeEventListener('beforeunload', this.handleUnload.bind(this));
  }

  private handleUnload() {
    this.signalRService.disconnect();
  }
}
