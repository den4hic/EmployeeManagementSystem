import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {RoleService} from "../../services/role.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {ProjectService} from "../../services/project.service";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-create-project-dialog',
  templateUrl: './create-project-dialog.component.html',
  styleUrl: './create-project-dialog.component.css'
})
export class CreateProjectDialogComponent {
  createProjectForm: FormGroup;

  constructor(
    private dialogRef: MatDialogRef<CreateProjectDialogComponent>,
    private projectService: ProjectService,
    private snackBar: MatSnackBar,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: { managerId: number }
  ) {
    this.createProjectForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      startDate: [new Date(), Validators.required],
      endDate: [new Date(), Validators.required],
      statusId: [1, Validators.required],
    });
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onSubmit() {

  }
}
