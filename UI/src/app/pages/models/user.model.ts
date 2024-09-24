export interface UserModel {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  username: string;
  hireDate: Date | null;
  salary: number | null;
  position: string | null;
  department: string | null;
}
