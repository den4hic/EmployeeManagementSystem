import {ManagerDto} from "./manager.dto";
import {EmployeeDto} from "./employee.dto";

export interface UserDto {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  username: string;
  manager: ManagerDto | null;
  employee: EmployeeDto | null;
  role: string;
}
