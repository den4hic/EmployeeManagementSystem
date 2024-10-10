import {Component, OnInit} from '@angular/core';
import {MatDialog} from '@angular/material/dialog';
import {MatSnackBar} from '@angular/material/snack-bar';
import {MatTabChangeEvent} from '@angular/material/tabs';
import {CdkDragDrop, moveItemInArray, transferArrayItem} from '@angular/cdk/drag-drop';

import {ProjectService} from '../../services/project.service';
import {TaskService} from '../../services/task.service';
import {StatusService} from '../../services/status.service';
import {EmployeeService} from '../../services/employee.service';
import {ManagerService} from '../../services/manager.service';
import {UserService} from '../../services/user.service';
import {JwtService} from '../../services/jwt.service';

import {ProjectDto} from '../../services/dtos/project.dto';
import {TaskDto} from '../../services/dtos/task.dto';
import {StatusDto} from '../../services/dtos/status.dto';
import {EmployeeDto} from '../../services/dtos/employee.dto';
import {ManagerDto} from '../../services/dtos/manager.dto';
import {UserDto} from '../../services/dtos/user.dto';

import {CreateTaskDialogComponent} from '../../shared/create-task-dialog/create-task-dialog.component';
import {CreateProjectDialogComponent} from '../../shared/create-project-dialog/create-project-dialog.component';
import {ConfirmDialogComponent} from '../../shared/confirm-dialog/confirm-dialog.component';
import {ShowTaskDialogComponent} from '../../shared/show-task-dialog/show-task-dialog.component';

@Component({
  selector: 'app-project-dashboard',
  templateUrl: './project-dashboard.component.html',
  styleUrls: ['./project-dashboard.component.css']
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
  userRole: string | null = null;
  user: UserDto | null = null;
  showOnlyMyTasks: boolean = false;
  selectedProjectManagers: ManagerDto[] = [];
  selectedProjectEmployees: EmployeeDto[] = [];

  constructor(
    private projectService: ProjectService,
    private employeeService: EmployeeService,
    private taskService: TaskService,
    private statusService: StatusService,
    private managerService: ManagerService,
    private userService: UserService,
    private jwtService: JwtService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.userRole = this.jwtService.getUserRole();
    this.userService.getUserInfo().subscribe(
      (user) => {
        this.user = user;
      }
    );
    this.loadProjects();
    this.loadStatuses();
    this.loadEmployees();
    this.loadManagers();
  }

  loadProjects() {
    this.projectService.getProjects().subscribe(
      (projects) => {
        this.projects = projects;
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

  loadEmployees() {
    this.employeeService.getEmployees().subscribe(
      (employees) => {
        this.employees = employees;
      }
    );
  }

  loadManagers() {
    this.managerService.getManagers().subscribe(
      (managers) => {
        console.log('managers:', managers);
        this.managers = managers;
      }
    );
  }

  selectProject(project: ProjectDto) {
    this.selectedProject = project;
    this.loadTasks(project.id);
    this.selectedProjectEmployees = this.employees.filter(employee =>
      project.employees.some(projectEmployee => projectEmployee.id === employee.id)
    );

    this.selectedProjectManagers = this.managers.filter(manager =>
      project.managers.some(projectManager => projectManager.id === manager.id)
    );
  }

  loadTasks(projectId: number, employeeId?: number) {
    this.taskService.getTasksByProjectId(projectId).subscribe(
      (tasks) => {
        this.tasks = {};
        this.totalTasks = tasks.length;
        this.completedTasks = tasks.filter(task => task.statusId === this.getCompletedStatusId()).length;
        this.statuses.forEach(status => {
          this.tasks[status.id] = tasks.filter(task =>
            task.statusId === status.id &&
            (!employeeId || task.assignedToEmployeeId === employeeId)
          );
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
    if (!this.selectedProject) {
      return;
    }

    const projectEmployees = this.employees.filter(employee =>
      this.selectedProject?.employees.some(projectEmployee => projectEmployee.id === employee.id)
    );
    const dialogRef = this.dialog.open(CreateTaskDialogComponent, {
      width: '400px',
      data: { selectedTask: null, projectId: this.selectedProject.id, employees: projectEmployees }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.taskService.createTask(result).subscribe(
          (newTask) => {
            this.loadTasks(this.selectedProject?.id || 0);
          },
          (error) => {
            console.error('Error creating task:', error);
          }
        );
      }
    });
  }

  openCreateProjectDialog() {
    const dialogRef = this.dialog.open(CreateProjectDialogComponent, {
      width: '90vw',
      data: { selectedProject: null, managerId: 1, employees: this.employees, managers: this.managers }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.projectService.createProject(result).subscribe(
          () => {
            this.snackBar.open('Project created successfully', 'Close', { duration: 3000 });
            this.loadProjects();
          },
          (error) => {
            this.showError(error);
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
      data: { selectedProject: createProjectDto, managerId: 1, employees: this.employees, managers: this.managers }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        result.id = this.selectedProject?.id;
        result.taskIds = this.selectedProject?.tasks.map(task => task.id) || [];
        this.projectService.updateProject(result).subscribe(
          () => {
            this.snackBar.open('Project updated successfully', 'Close', { duration: 3000 });
            this.loadProjects();
          },
          error => {
            this.showError(error);
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

  openEditTaskDialog(task: TaskDto) {
    if (!this.selectedProject) {
      return;
    }
    if (this.userRole === 'Employee') {
      const infoDialogRef = this.dialog.open(ShowTaskDialogComponent, {
        width: '600px',
        data: { selectedTask: task, currentStatus: this.statuses.find(status => status.id === task.statusId) }
      });
      return;
    }
    const projectEmployees = this.employees.filter(employee =>
      this.selectedProject?.employees.some(projectEmployee => projectEmployee.id === employee.id)
    );
    const dialogRef = this.dialog.open(CreateTaskDialogComponent, {
      width: '400px',
      data: { selectedTask: task, projectId: this.selectedProject.id, employees: projectEmployees }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        result.id = task.id;
        this.taskService.updateTask(result).subscribe(
          () => {
            this.loadTasks(this.selectedProject?.id || 0);
          },
          (error) => {
            console.error('Error updating task:', error);
          }
        );
      }
    });
  }

  canDragTask(task: TaskDto): boolean {
    return this.userRole === 'Manager' || this.userRole === 'Admin' || this.user?.employee?.id === task.assignedToEmployeeId;
  }

  onTaskToogleChanged() {
    if (!this.selectedProject) {
      return;
    }
    if (this.selectedProject.id === 0) {
      this.loadTasks(this.selectedProject.id);
    } else if (this.showOnlyMyTasks) {
      this.loadTasks(this.selectedProject.id, this.user?.employee?.id);
    } else {
      this.loadTasks(this.selectedProject.id);
    }
  }

  onTabChange(event: MatTabChangeEvent) {
  }

  private projectDtoToCreateProjectDto(project: ProjectDto): any {
    return {
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
  }

  private showError(error: any) {
    let errorMessage = 'An unknown error occurred';
    if (error.error && error.error.errors) {
      const firstErrorKey = Object.keys(error.error.errors)[0];
      if (firstErrorKey) {
        errorMessage = error.error.errors[firstErrorKey][0];
      }
    }
    this.snackBar.open(errorMessage, 'Close', { duration: 3000 });
  }

  confirmRemoveEmployee(employee: EmployeeDto) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '300px'
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        if (!this.selectedProject) {
          return;
        }
        const createProjectDto = this.projectDtoToCreateProjectDto(this.selectedProject);
        const newProjectEmployeeIds = createProjectDto.employeeIds.filter((id: number) => id !== employee.id);
        const newProject = {
          ...createProjectDto,
          employeeIds: newProjectEmployeeIds
        };

        this.projectService.updateProject(newProject).subscribe(
          () => {
            this.snackBar.open('Employee removed successfully', 'Close', { duration: 3000 });
            this.loadProjects();
          },
          (error) => {
            this.showError(error);
          }
        );
      }
    });
  }

  confirmRemoveManager(manager: ManagerDto) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '300px'
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        if (!this.selectedProject) {
          return;
        }
        const createProjectDto = this.projectDtoToCreateProjectDto(this.selectedProject);
        const newProjectManagerIds = createProjectDto.managerIds.filter((id: number) => id !== manager.id);

        const newProject = {
          ...createProjectDto,
          managerIds: newProjectManagerIds
        };

        this.projectService.updateProject(newProject).subscribe(
          () => {
            this.snackBar.open('Manager removed successfully', 'Close', { duration: 3000 });
            this.loadProjects();
          },
          (error) => {
            this.showError(error);
          }
        );
      }
    });
  }
}
