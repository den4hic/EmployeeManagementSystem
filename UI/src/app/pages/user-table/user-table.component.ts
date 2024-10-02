import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { UserDto } from "../../services/dtos/user.dto";
import { UserService } from "../../services/user.service";
import { MatSnackBar } from "@angular/material/snack-bar";
import { MatDialog } from "@angular/material/dialog";
import { ConfirmDialogComponent } from "../../shared/confirm-dialog/confirm-dialog.component";
import { AssignRoleDialogComponent } from "../../shared/role-dialog/role-dialog.component";
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';
import {RoleService} from "../../services/role.service";
import {MatSlideToggleChange} from "@angular/material/slide-toggle";

@Component({
  selector: 'app-user-table',
  templateUrl: './user-table.component.html',
  styleUrls: ['./user-table.component.scss']
})
export class UserTableComponent implements OnInit, AfterViewInit {
  displayedColumns: string[] = ['select', 'id', 'firstName', 'lastName', 'email', 'phoneNumber', 'hireDate', 'role', 'isBlocked', 'actions'];
  dataSource = new MatTableDataSource<UserDto>();
  selectedUserIds: number[] = [];
  totalItems = 0;
  pageSize = 10;
  pageSizeOptions: number[] = [5, 10, 20];
  currentPage = 0;
  filterValue = '';
  sortActive = 'id';
  sortDirection = 'asc';
  roles: string[] = [];
  selectedRole = '';

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private userService: UserService,
    private roleService: RoleService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.loadRoles();
    this.loadUsersPage();
  }

  ngAfterViewInit() {
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
    merge(this.sort.sortChange, this.paginator.page)
      .pipe(tap(() => this.loadUsersPage()))
      .subscribe();
  }

  loadRoles() {
    this.roleService.getRoles().subscribe(
      (roles) => {
        this.roles = roles.map((role) => role.name);
      },
      (error) => {
        console.error('Error loading roles', error);
        this.showSnackBar('Error loading roles');
      }
    );
  }

  loadUsersPage() {
    this.userService.getUsersWithDetails(
      this.currentPage,
      this.pageSize,
      this.sortActive,
      this.sortDirection,
      this.filterValue,
      this.selectedRole
    ).subscribe(
      (response) => {
        this.dataSource.data = response.items;
        this.totalItems = response.totalItems;
      },
      (error) => {
        console.error('Error loading users', error);
        this.showSnackBar('Error loading users');
      }
    );
  }

  applyFilter(event: Event) {
    this.filterValue = (event.target as HTMLInputElement).value;
    this.paginator.pageIndex = 0;
    this.loadUsersPage();
  }

  onPageChange(event: PageEvent) {
    this.currentPage = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadUsersPage();
  }

  onRoleFilterChange() {
    this.paginator.pageIndex = 0;
    this.loadUsersPage();
  }

  onSortChange(sort: Sort) {
    this.sortActive = sort.active;
    this.sortDirection = sort.direction;
    this.loadUsersPage();
  }

  confirmDelete(userId: number) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '300px'
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.deleteUser(userId);
      }
    });
  }

  deleteUser(userId: number) {
    this.userService.deleteUser(userId).subscribe({
      next: () => {
        this.loadUsersPage();
        this.showSnackBar('User deleted successfully');
      },
      error: (error) => {
        console.error('Error deleting user', error);
        this.showSnackBar('Error deleting user');
      }
    });
  }

  onRowSelect(userId: number, isChecked: boolean) {
    if (isChecked) {
      this.selectedUserIds.push(userId);
    } else {
      this.selectedUserIds = this.selectedUserIds.filter(id => id !== userId);
    }
  }

  selectAllRows() {
    this.selectedUserIds = this.dataSource.data.map(user => user.id);
  }

  deselectAllRows() {
    this.selectedUserIds = [];
  }

  openAssignRoleDialog() {
    const dialogRef = this.dialog.open(AssignRoleDialogComponent, {
      width: '300px',
      data: { userIds: this.selectedUserIds }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.snackBar.open('Role assigned successfully', 'Close', { duration: 3000 });
        this.loadUsersPage();
      }
    });
  }

  private showSnackBar(message: string) {
    this.snackBar.open(message, 'Close', { duration: 3000 });
  }

  onToggleChange(userId: number, $event: MatSlideToggleChange) {
    const isBlocked : boolean = $event.checked;

    this.userService.blockUser(userId, isBlocked).subscribe({
      next: () => {
        if (isBlocked) {
          this.showSnackBar('User blocked successfully');
        } else {
          this.showSnackBar('User unblocked successfully');
        }
      },
      error: (error) => {
        console.error('Error blocking user', error);
        this.showSnackBar('Error blocking user');
      }
    });
  }
}

