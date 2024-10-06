import { Injectable } from '@angular/core';
import {ApiPaths} from "./enums/api-paths";
import {HttpClient} from "@angular/common/http";
import {StatusDto} from "./dtos/status.dto";
import {Observable} from "rxjs";
import {EmployeeDto} from "./dtos/employee.dto";

@Injectable({
  providedIn: 'root'
})
export class StatusService {
  private apiPath: string = ApiPaths.Status;

  constructor(private http: HttpClient) { }

  getStatusById(id: number): Observable<StatusDto> {
    const url = `${this.apiPath}/${id}`;
    return this.http.get<StatusDto>(url);
  }

  getStatuses(): Observable<StatusDto[]> {
    const url = `${this.apiPath}`;
    return this.http.get<StatusDto[]>(url);
  }
}
