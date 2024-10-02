import { Component, OnInit } from '@angular/core';
import { JwtService } from '../../services/jwt.service';

@Component({
  selector: 'app-blocked',
  templateUrl: './blocked.component.html',
  styleUrls: ['./blocked.component.css']
})
export class BlockedComponent implements OnInit {
  username: string | null = null;

  constructor(
    private jwtService: JwtService,
  ) { }

  ngOnInit(): void {
    this.username = this.jwtService.getUsername();
  }
}
