import { Component } from '@angular/core';
import {ProjectService} from "../../services/project.service";
import {ProjectDto} from "../../services/dtos/project.dto";
import {MatDialog} from "@angular/material/dialog";
import {MatSnackBar} from "@angular/material/snack-bar";
import {CreateProjectDialogComponent} from "../../shared/create-project-dialog/create-project-dialog.component";

@Component({
  selector: 'app-project-table',
  templateUrl: './project-table.component.html',
  styleUrl: './project-table.component.css'
})
export class ProjectTableComponent {
  constructor(
    private projectService: ProjectService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  projects: ProjectDto[] = [];

  ngOnInit() {
    this.loadProjects();
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
      width: '300px',
      data: { managerId: 1 }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.snackBar.open('Project created successfully', 'Close', { duration: 3000 });
        this.loadProjects();
      }
    });
  }
}
