import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { RoleService } from '../../services/role.service';
import { EmployeeManagerRoleDto } from '../../services/dtos/employee-manager-role.dto';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-role-dialog',
  templateUrl: './role-dialog.component.html',
  styleUrls: ['./role-dialog.component.scss']
})
export class AssignRoleDialogComponent {
  selectedRole: string = '';
  roles: string[] = [];
  roleData: EmployeeManagerRoleDto = {};

  constructor(
    private dialogRef: MatDialogRef<AssignRoleDialogComponent>,
    private roleService: RoleService,
    private snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: { userIds: number[] }
  ) {}

  ngOnInit(): void {
    console.log(this.data.userIds);
    this.loadRoles();
  }

  loadRoles(): void {
    this.roleService.getRoles().subscribe({
      next: (roles) => (this.roles = roles.map((role) => role.name)),
      error: (error) => this.snackBar.open('Error loading roles', 'Close', { duration: 3000 })
    });
  }

  onAssignRole(): void {
    for(let i = 0; i < this.data.userIds.length; i++) {
      this.roleService.assignRole(this.data.userIds[i], this.selectedRole, this.roleData).subscribe({
        next: () => {
          this.snackBar.open('Role assigned successfully', 'Close', { duration: 3000 });
          this.dialogRef.close({ success: true });
        },
        error: (error) => {
          this.snackBar.open('Error assigning role', 'Close', { duration: 3000 });
        }
      });
    }

  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
