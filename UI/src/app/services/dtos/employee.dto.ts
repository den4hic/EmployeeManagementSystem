import {ProjectDto} from "./project.dto";
import {UserDto} from "./user.dto";

export interface EmployeeDto {
  id: number;
  userId: number;
  position: string;
  hireDate: Date;
  salary: number;
  projects: ProjectDto[];
  user: UserDto;
}
