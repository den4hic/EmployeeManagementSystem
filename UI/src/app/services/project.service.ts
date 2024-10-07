import { Injectable } from '@angular/core';
import {ApiPaths} from "./enums/api-paths";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {ProjectDto} from "./dtos/project.dto";
import {CreateProjectDto} from "./dtos/create-project.dto";

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private apiPath: string = ApiPaths.Project;
  constructor(private http: HttpClient) { }

  getProjects(): Observable<ProjectDto[]> {
    const url = `${this.apiPath}`;
    return this.http.get<ProjectDto[]>(url);
  }

  createProject(project: CreateProjectDto): Observable<ProjectDto> {
    const url = `${this.apiPath}`;
    return this.http.post<ProjectDto>(url, project);
  }

  updateProject(project: CreateProjectDto) {
    const url = `${this.apiPath}`;
    return this.http.put(url, project);
  }

  deleteProject(projectId: number) {
    const url = `${this.apiPath}/${projectId}`;
    return this.http.delete(url);
  }
}
