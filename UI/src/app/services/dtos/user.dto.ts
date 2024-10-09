import {ManagerDto} from "./manager.dto";
import {EmployeeDto} from "./employee.dto";
import {UserPhotoDto} from "./user-photo.dto";

export interface UserDto {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  username: string;
  isBlocked: boolean;
  manager: ManagerDto | null;
  employee: EmployeeDto | null;
  userPhoto: UserPhotoDto | null;
  role: string;
}
