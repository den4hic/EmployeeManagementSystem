import { Component } from '@angular/core';
import {ProjectService} from "../../services/project.service";
import {ProjectDto} from "../../services/dtos/project.dto";
import {MatDialog} from "@angular/material/dialog";
import {MatSnackBar} from "@angular/material/snack-bar";
import {CreateProjectDialogComponent} from "../../shared/create-project-dialog/create-project-dialog.component";
import {EmployeeDto} from "../../services/dtos/employee.dto";
import {EmployeeService} from "../../services/employee.service";

@Component({
  selector: 'app-project-table',
  templateUrl: './project-table.component.html',
  styleUrl: './project-table.component.css'
})
export class ProjectTableComponent {
  constructor(
    private projectService: ProjectService,
    private employeeService: EmployeeService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  projects: ProjectDto[] = [];
  employees: EmployeeDto[] = [];

  ngOnInit() {
    this.loadProjects();
    this.loadEmployees();
  }

  loadProjects() {
    this.projectService.getProjects().subscribe(
      (projects) => {
        this.projects = projects;
        console.log(projects);
      }
    );
  }


  openCreateProjectDialog() {
    const dialogRef = this.dialog.open(CreateProjectDialogComponent, {
      width: '90vw',
      data: { managerId: 1, employees: this.employees }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.projectService.createProject(result).subscribe(
          () => {
            this.snackBar.open('Project created successfully', 'Close', { duration: 3000 });
            this.loadProjects();
          }
        );
      }
    });
  }

  private loadEmployees() {
    this.employeeService.getEmployees().subscribe(
      (employees) => {
        this.employees = employees;
      }
    );
  }
}
