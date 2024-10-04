import {TaskDto} from "./task.dto";
import {EmployeeDto} from "./employee.dto";
import {ManagerDto} from "./manager.dto";

export interface ProjectDto {
  id: number;
  statusId: number;
  name: string;
  description: string;
  startDate: Date;
  endDate: Date;
  tasks: TaskDto[];
  employees: EmployeeDto[];
  managers: ManagerDto[];
}
