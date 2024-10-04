import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { EmployeeManagerRoleDto } from './dtos/employee-manager-role.dto';
import {RoleDto} from "./dtos/role.dto";
import {ApiPaths} from "./enums/api-paths";

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  private apiPath: string = ApiPaths.Role;

  constructor(private http: HttpClient) {}

  assignRole(userId: number, role: string, roleData: EmployeeManagerRoleDto): Observable<any> {
    roleData.hireDate = new Date();
    const url = `${this.apiPath}/assign-role?userId=${userId}&role=${role}`;
    return this.http.post(url, roleData);
  }

  getRoles(): Observable<RoleDto[]> {
    const url = `${this.apiPath}`;
    return this.http.get<RoleDto[]>(url);
  }
}
