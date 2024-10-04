import { Injectable } from '@angular/core';
import {ApiPaths} from "./enums/api-paths";
import {HttpClient} from "@angular/common/http";
import {EmployeeDto} from "./dtos/employee.dto";
import {Observable} from "rxjs";
import {RoleDto} from "./dtos/role.dto";

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private apiPath: string = ApiPaths.Employee;
  constructor(private http: HttpClient) { }

  getEmployees(): Observable<EmployeeDto[]> {
    const url = `${this.apiPath}`;
    return this.http.get<EmployeeDto[]>(url);
  }
}
