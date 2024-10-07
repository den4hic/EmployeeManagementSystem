import {Component, Input, OnInit} from '@angular/core';
import { ProjectService } from '../../services/project.service';
import { TaskService } from '../../services/task.service';
import { StatusService } from '../../services/status.service';
import { ProjectDto } from '../../services/dtos/project.dto';
import { TaskDto } from '../../services/dtos/task.dto';
import { StatusDto } from '../../services/dtos/status.dto';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import {EmployeeDto} from "../../services/dtos/employee.dto";
import {CreateTaskDialogComponent} from "../../shared/create-task-dialog/create-task-dialog.component";
import {MatDialog} from "@angular/material/dialog";
import {CreateProjectDialogComponent} from "../../shared/create-project-dialog/create-project-dialog.component";
import {MatSnackBar} from "@angular/material/snack-bar";
import {EmployeeService} from "../../services/employee.service";
import {CreateProjectDto} from "../../services/dtos/create-project.dto";
import {ManagerService} from "../../services/manager.service";
import {ManagerDto} from "../../services/dtos/manager.dto";
import {ConfirmDialogComponent} from "../../shared/confirm-dialog/confirm-dialog.component";

@Component({
  selector: 'app-project-dashboard',
  templateUrl: './project-dashboard.component.html',
  styleUrls: ['./project-dashboard.component.css'],
})
export class ProjectDashboardComponent implements OnInit {
  employees: EmployeeDto[] = [];
  managers: ManagerDto[] = [];
  projects: ProjectDto[] = [];
  selectedProject: ProjectDto | null = null;
  statuses: StatusDto[] = [];
  tasks: { [key: number]: TaskDto[] } = {};
  connectedLists: string[] = [];
  totalTasks: number = 0;
  completedTasks: number = 0;

  constructor(
    private projectService: ProjectService,
    private employeeService: EmployeeService,
    private taskService: TaskService,
    private statusService: StatusService,
    private managerService: ManagerService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.loadProjects();
    this.loadStatuses();
    this.loadEmployees();
    this.loadManagers();
  }

  private loadManagers() {
    this.managerService.getManagers().subscribe(
      (managers) => {
        this.managers = managers;
        console.log('Managers:', managers);
      }
    )
  }

  loadProjects() {
    this.projectService.getProjects().subscribe(
      (projects) => {
        this.projects = projects;
        console.log(projects);
        if (projects.length > 0) {
          this.selectProject(projects[0]);
        }
      }
    );
  }

  loadStatuses() {
    this.statusService.getStatuses().subscribe(
      (statuses) => {
        this.statuses = statuses;
        this.connectedLists = this.statuses.map(s => s.id.toString());
      }
    );
  }

  private loadEmployees() {
    this.employeeService.getEmployees().subscribe(
      (employees) => {
        this.employees = employees;
        console.log('Employees:', employees);
      }
    );
  }

  selectProject(project: ProjectDto) {
    this.selectedProject = project;
    this.loadTasks(project.id);
  }

  loadTasks(projectId: number) {
    this.taskService.getTasksByProjectId(projectId).subscribe(
      (tasks) => {
        console.log('Tasks:', tasks);
        this.tasks = {};
        this.totalTasks = tasks.length;
        this.completedTasks = tasks.filter(task => task.statusId === this.getCompletedStatusId()).length;
        this.statuses.forEach(status => {
          this.tasks[status.id] = tasks.filter(task => task.statusId === status.id);
        });
      }
    );
  }

  onTaskDrop(event: CdkDragDrop<TaskDto[]>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );

      const task = event.container.data[event.currentIndex];
      const newStatusId = parseInt(event.container.id);
      const previousStatusId = parseInt(event.previousContainer.id);

      this.taskService.updateTaskStatus(task.id, newStatusId).subscribe(
        () => {
          this.updateTaskCompletion(previousStatusId, newStatusId);
        },
        (error) => {
          console.error('Error updating task status:', error);
          transferArrayItem(
            event.container.data,
            event.previousContainer.data,
            event.currentIndex,
            event.previousIndex
          );
        }
      );
    }
  }

  updateTaskCompletion(previousStatusId: number, newStatusId: number) {
    const completedStatusId = this.getCompletedStatusId();
    if (newStatusId === completedStatusId && previousStatusId !== completedStatusId) {
      this.completedTasks++;
    } else if (previousStatusId === completedStatusId && newStatusId !== completedStatusId) {
      this.completedTasks--;
    }
  }

  getCompletedStatusId(): number {
    const completedStatus = this.statuses.find(status => status.name.toLowerCase() === 'done');
    return completedStatus ? completedStatus.id : -1;
  }

  getProjectProgress(): number {
    return this.totalTasks > 0 ? (this.completedTasks / this.totalTasks) * 100 : 0;
  }

  openCreateTaskDialog(): void {
    const dialogRef = this.dialog.open(CreateTaskDialogComponent, {
      width: '400px',
      data: { projectId: this.selectedProject?.id, employees: this.employees }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.taskService.createTask(result).subscribe(
          (newTask) => {
            const initialStatusId = this.statuses[0].id;
            if (!this.tasks[initialStatusId]) {
              this.tasks[initialStatusId] = [];
            }
            this.tasks[initialStatusId].push(newTask);
            this.totalTasks++;
          },
          (error) => {
            console.error('Error creating task:', error);
          }
        );
      }
    });
  }

  projectDtoToCreateProjectDto(project: ProjectDto): CreateProjectDto {
    let result: CreateProjectDto;

    result = {
      id: project.id,
      name: project.name,
      description: project.description,
      startDate: project.startDate,
      endDate: project.endDate,
      statusId: project.statusId,
      managerIds: project.managers.map(manager => manager.id),
      employeeIds: project.employees.map(employee => employee.id),
      taskIds: project.tasks.map(task => task.id)
    };

    return result;
  }

  openCreateProjectDialog() {
    const dialogRef = this.dialog.open(CreateProjectDialogComponent, {
      width: '90vw',
      data: { selectedProject: null, managerId: 1, employees: this.employees }
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

  openEditProjectDialog() {
    if (!this.selectedProject) {
      return;
    }
    const createProjectDto = this.projectDtoToCreateProjectDto(this.selectedProject);
    const dialogRef = this.dialog.open(CreateProjectDialogComponent, {
      width: '90vw',
      data: { selectedProject: createProjectDto, managerId: 1, employees: this.employees }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        result.id = this.selectedProject?.id;
        this.projectService.updateProject(result).subscribe(
          () => {
            this.snackBar.open('Project update successfully', 'Close', { duration: 3000 });
            this.loadProjects();
          }
        );
      }
    });
  }

  confirmDelete(projectId: number) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '300px'
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.deleteProject(projectId);
      }
    });
  }

  private deleteProject(projectId: number) {
    this.projectService.deleteProject(projectId).subscribe({
      next: () => {
        this.loadProjects();
        this.snackBar.open('Project deleted successfully', 'Close', { duration: 3000 });
      },
      error: (error) => {
        console.error('Error deleting project', error);
        this.snackBar.open('Error deleting project', 'Close', { duration: 3000 });
      }
    });
  }

  confirmDeleteTask(taskId: number) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '300px'
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.deleteTask(taskId);
      }
    });
  }

  private deleteTask(taskId: number) {
    this.taskService.deleteTask(taskId).subscribe({
      next: () => {
        this.loadTasks(this.selectedProject?.id || 0);
        this.snackBar.open('Task deleted successfully', 'Close', {duration: 3000});
      },
      error: (error) => {
        console.error('Error deleting task', error);
        this.snackBar.open('Error deleting task', 'Close', {duration: 3000});
      }
    });
  }
}
