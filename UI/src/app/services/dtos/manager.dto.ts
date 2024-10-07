import {ProjectDto} from "./project.dto";
import {UserDto} from "./user.dto";

export interface ManagerDto {
  id: number;
  userId: number;
  department: string;
  hireDate: Date;
  projects: ProjectDto[];
  user: UserDto;
}
