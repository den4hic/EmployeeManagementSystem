import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { UserDto } from "../../services/dtos/user.dto";
import { UserService } from "../../services/user.service";
import { MatSnackBar } from "@angular/material/snack-bar";
import { MatDialog } from "@angular/material/dialog";
import { ConfirmDialogComponent } from "../../shared/confirm-dialog/confirm-dialog.component";
import { merge, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-user-table',
  templateUrl: './user-table.component.html',
  styleUrls: ['./user-table.component.scss']
})
export class UserTableComponent implements OnInit, AfterViewInit {
  displayedColumns: string[] = ['id', 'firstName', 'lastName', 'email', 'phoneNumber', 'hireDate', 'role', 'actions'];
  dataSource = new MatTableDataSource<UserDto>();

  totalItems = 0;
  pageSize = 10;
  pageSizeOptions: number[] = [5, 10, 20];
  currentPage = 0;

  filterValue = '';
  sortActive = 'id';
  sortDirection = 'asc';

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  users: UserDto[] = [];
  constructor(
    private userService: UserService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.loadUsersPage();
  }

  ngAfterViewInit() {
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);

    merge(this.sort.sortChange, this.paginator.page)
      .pipe(
        tap(() => this.loadUsersPage())
      )
      .subscribe();
  }

  loadUsersPage() {
    this.userService.getUsersWithDetails(
      this.currentPage,
      this.pageSize,
      this.sortActive,
      this.sortDirection,
      this.filterValue
    ).subscribe(
      (response) => {
        this.users = response;
        this.dataSource.data = this.users;
      },
      (error) => {
        console.error('Error loading users', error);
        this.showSnackBar('Помилка завантаження користувачів');
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
        this.loadUsersPage(); // Перезавантажуємо поточну сторінку
        this.showSnackBar('Користувача успішно видалено');
      },
      error: (error) => {
        console.error('Error deleting user', error);
        this.showSnackBar('Помилка видалення користувача');
      }
    });
  }

  private showSnackBar(message: string) {
    this.snackBar.open(message, 'Закрити', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
    });
  }
}
