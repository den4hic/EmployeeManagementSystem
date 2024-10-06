import { Component, OnInit } from '@angular/core';
import { ProjectService } from '../../services/project.service';
import { TaskService } from '../../services/task.service';
import { StatusService } from '../../services/status.service';
import { ProjectDto } from '../../services/dtos/project.dto';
import { TaskDto } from '../../services/dtos/task.dto';
import { StatusDto } from '../../services/dtos/status.dto';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-project-dashboard',
  templateUrl: './project-dashboard.component.html',
  styleUrls: ['./project-dashboard.component.css']
})
export class ProjectDashboardComponent implements OnInit {
  projects: ProjectDto[] = [];
  selectedProject: ProjectDto | null = null;
  statuses: StatusDto[] = [];
  tasks: { [key: number]: TaskDto[] } = {};
  connectedLists: string[] = [];
  totalTasks: number = 0;
  completedTasks: number = 0;

  constructor(
    private projectService: ProjectService,
    private taskService: TaskService,
    private statusService: StatusService
  ) {}

  ngOnInit() {
    this.loadProjects();
    this.loadStatuses();
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

  selectProject(project: ProjectDto) {
    this.selectedProject = project;
    this.loadTasks(project.id);
  }

  loadTasks(projectId: number) {
    this.taskService.getTasksByProjectId(projectId).subscribe(
      (tasks) => {
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
}
