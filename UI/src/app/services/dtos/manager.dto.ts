import {ProjectDto} from "./project.dto";

export interface ManagerDto {
  id: number;
  userId: number;
  department: string;
  hireDate: Date;
  projects: ProjectDto[];
}
