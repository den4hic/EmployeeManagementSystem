import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {TaskDto} from "../../services/dtos/task.dto";
import {EmployeeDto} from "../../services/dtos/employee.dto";
import {StatusDto} from "../../services/dtos/status.dto";

@Component({
  selector: 'app-show-task-dialog',
  templateUrl: './show-task-dialog.component.html',
  styleUrl: './show-task-dialog.component.css'
})
export class ShowTaskDialogComponent {

  constructor(private dialogRef: MatDialogRef<ShowTaskDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: { selectedTask: TaskDto | null, currentStatus: StatusDto })
  {}

  onCancel(): void {
    this.dialogRef.close();
  }
}
