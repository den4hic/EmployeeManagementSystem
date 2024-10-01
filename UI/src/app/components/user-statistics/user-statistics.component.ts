import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import {UserStatistics} from "../../services/dtos/user-statistic.dto";

@Component({
  selector: 'app-user-statistics',
  templateUrl: './user-statistics.component.html',
  styleUrls: ['./user-statistics.component.scss']
})
export class UserStatisticsComponent implements OnInit {
  statistics: UserStatistics = {
    totalUsers: 0,
    activeAdmins: 0
  };

  constructor(private userService: UserService) {}

  ngOnInit() {
    this.loadStatistics();
  }

  loadStatistics() {
    this.userService.getUserStatistics().subscribe(
      (stats: UserStatistics) => {
        this.statistics = stats;
      },
      (error) => {
        console.error('Error loading user statistics', error);
      }
    );
  }
}
