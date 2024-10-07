import { Injectable } from '@angular/core';
import {ApiPaths} from "./enums/api-paths";
import {HttpClient} from "@angular/common/http";
import {ManagerDto} from "./dtos/manager.dto";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ManagerService {
  private apiPath: string = ApiPaths.Manager;

  constructor(private http: HttpClient) { }

  getManagers(): Observable<ManagerDto[]> {
    const url = `${this.apiPath}`;
    return this.http.get<ManagerDto[]>(url);
  }
}
