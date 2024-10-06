import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EmployeeDto } from '../../services/dtos/employee.dto';
import {provideNativeDateAdapter} from "@angular/material/core";

@Component({
  selector: 'app-create-task-dialog',
  templateUrl: './create-task-dialog.component.html',
  styleUrl: './create-task-dialog.component.css',
  providers: [
    provideNativeDateAdapter()
  ]
})
export class CreateTaskDialogComponent {
  taskForm: FormGroup;
  employees: EmployeeDto[] = [];

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<CreateTaskDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { projectId: number, employees: EmployeeDto[] }
  ) {
    this.taskForm = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      projectId: [this.data.projectId, Validators.required],
      assignedToEmployeeId: [null],
      statusId: [1],
      dueDate: [null]
    });

    this.employees = this.data.employees;
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    if (this.taskForm.valid) {
      this.dialogRef.close({
        ...this.taskForm.value,
        projectId: this.data.projectId
      });
    }
  }
}
