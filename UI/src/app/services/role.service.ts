import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { EmployeeManagerRoleDto } from './dtos/employee-manager-role.dto';
import {RoleDto} from "./dtos/role.dto";

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  private apiUrl = `https://localhost:7110/api/role`;

  constructor(private http: HttpClient) {}

  assignRole(userId: number, role: string, roleData: EmployeeManagerRoleDto): Observable<any> {
    roleData.hireDate = new Date();
    const url = `${this.apiUrl}/assign-role?userId=${userId}&role=${role}`;
    return this.http.post(url, roleData);
  }

  getRoles(): Observable<RoleDto[]> {
    const url = `${this.apiUrl}`;
    return this.http.get<RoleDto[]>(url);
  }
}
