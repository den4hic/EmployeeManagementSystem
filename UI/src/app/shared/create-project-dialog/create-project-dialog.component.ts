import { Component, Inject } from '@angular/core';
import {AbstractControl, FormBuilder, FormGroup, Validators} from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CreateProjectDto } from '../../services/dtos/create-project.dto';
import {provideNativeDateAdapter} from "@angular/material/core";
import {EmployeeService} from "../../services/employee.service";
import {EmployeeDto} from "../../services/dtos/employee.dto";
import {StatusService} from "../../services/status.service";
import {StatusDto} from "../../services/dtos/status.dto";
import {STEPPER_GLOBAL_OPTIONS} from "@angular/cdk/stepper";
import {ProjectDto} from "../../services/dtos/project.dto";
import {ManagerDto} from "../../services/dtos/manager.dto";

@Component({
  selector: 'app-create-project-dialog',
  templateUrl: './create-project-dialog.component.html',
  styleUrl: './create-project-dialog.component.css',
  providers: [
    {
      provide: STEPPER_GLOBAL_OPTIONS,
      useValue: {showError: true},
    },
    provideNativeDateAdapter()
  ]
})
export class CreateProjectDialogComponent {
  assignForm: FormGroup;
  valueForm: FormGroup;
  managerIds: number[] = [];
  employeeIds: number[] = [];

  employees: EmployeeDto[] = [];
  managers: ManagerDto[] = [];
  statuses: StatusDto[] = [];
  selectedProject: CreateProjectDto | null = null;

  constructor(
    private employeeService: EmployeeService,
    private statusService: StatusService,
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<CreateProjectDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { selectedProject: CreateProjectDto | null, managerId: number, employees: EmployeeDto[], managers: ManagerDto[] }
  ) {
    this.employees = this.data.employees;
    this.managers = this.data.managers;

    statusService.getStatuses().subscribe(
      (statuses) => {
        this.statuses = statuses;
      }
    );

    this.selectedProject = this.data.selectedProject;

    this.valueForm = this.fb.group({
      name: [this.selectedProject?.name || '', [Validators.required, Validators.maxLength(100)]],
      description: [this.selectedProject?.description || ''],
      startDate: [this.selectedProject?.startDate || null, [Validators.required]],
      endDate: [this.selectedProject?.endDate || null],
    });

    this.assignForm = this.fb.group({
      employeeIds: [this.selectedProject?.employeeIds || [], Validators.required],
      managerIds: [this.selectedProject?.managerIds || [], Validators.required],
      statusId: [this.selectedProject?.statusId || 1, Validators.required],
    });
  }

  startDateValidator(control: AbstractControl): { [key: string]: boolean } | null {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    const startDate = new Date(control.value);
    if (startDate < today) {
      return { 'startDateInvalid': true };
    }
    return null;
  }

  endDateValidator(control: AbstractControl): { [key: string]: boolean } | null {
    const startDateForm = this.valueForm?.get('startDate')
    if (!startDateForm) {
      return null;
    }
    const startDate = startDateForm.value;
    const endDate = new Date(control.value);
    if (startDate && endDate <= new Date(startDate)) {
      return { 'endDateInvalid': true };
    }
    return null;
  }


  onSubmit() {
    if (this.valueForm.valid && this.assignForm.valid) {
      const createProjectDto: CreateProjectDto = {
        ...this.valueForm.value,
        ...this.assignForm.value,
      };

      console.log(createProjectDto);
      this.dialogRef.close(createProjectDto);
    }
  }
}
