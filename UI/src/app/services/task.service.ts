import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TaskDto } from './dtos/task.dto';
import { ApiPaths } from './enums/api-paths';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private apiPath: string = ApiPaths.Task;

  constructor(private http: HttpClient) { }

  getTasksByProjectId(projectId: number): Observable<TaskDto[]> {
    const url = `${this.apiPath}/project/${projectId}`;
    return this.http.get<TaskDto[]>(url);
  }

  updateTaskStatus(taskId: number, newStatusId: number): Observable<TaskDto> {
    const url = `${this.apiPath}/${taskId}/status`;
    return this.http.put<TaskDto>(url, newStatusId);
  }

  createTask(task: TaskDto): Observable<TaskDto> {
    console.log(task);
    return this.http.post<TaskDto>(this.apiPath, task);
  }

  deleteTask(taskId: number) {
    const url = `${this.apiPath}/${taskId}`;
    return this.http.delete(url);
  }

  updateTask(task: TaskDto) {
    const url = `${this.apiPath}`;
    return this.http.put(url, task);
  }
}
