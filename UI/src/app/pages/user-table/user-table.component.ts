import {Component, inject, OnInit, ViewChild} from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import {UserDto} from "../../services/dtos/user.dto";
import {UserService} from "../../services/user.service";

@Component({
  selector: 'app-user-table',
  templateUrl: './user-table.component.html',
  styleUrls: ['./user-table.component.scss']
})
export class UserTableComponent implements OnInit {
  displayedColumns: string[] = ['id', 'firstName', 'lastName', 'email', 'phoneNumber', 'username', 'role', 'actions'];
  dataSource = new MatTableDataSource<UserDto>();
  private userService = inject(UserService);


  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  users: UserDto[] = [
    { id: 1, firstName: 'John', lastName: 'Doe', email: 'john.doe@example.com', phoneNumber: '1234567890', username: 'johndoe', role: 'Admin' },
    { id: 2, firstName: 'Jane', lastName: 'Smith', email: 'jane.smith@example.com', phoneNumber: '0987654321', username: 'janesmith', role: 'User' },
  ];

  ngOnInit() {
    const res = this.userService.getUsers();

    console.log(res);
    res.subscribe((users) => {
      this.users = users;
      this.dataSource.data = this.users;
    });

  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
}
