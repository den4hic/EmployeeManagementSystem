import {EmployeeDto} from "./employee.dto";

export interface TaskDto {
  id: number;
  projectId: number;
  assignedToEmployeeId: number;
  statusId: number;
  title: string;
  description: string;
  dueDate: Date;
  assignedToEmployee: EmployeeDto;
}
