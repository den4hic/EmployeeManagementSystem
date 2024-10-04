import {ProjectDto} from "./project.dto";

export interface EmployeeDto {
  id: number;
  userId: number;
  position: string;
  hireDate: Date;
  salary: number;
  projects: ProjectDto[];
}
