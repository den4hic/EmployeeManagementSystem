import { Injectable } from '@angular/core';
import {ApiPaths} from "./enums/api-paths";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {ProjectDto} from "./dtos/project.dto";

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
}
