export interface CreateProjectDto {
  id: number;
  name: string;
  description: string;
  startDate: Date;
  endDate: Date;
  statusId: number;
  managerIds: number[];
  employeeIds: number[];
  taskIds: number[];
}
