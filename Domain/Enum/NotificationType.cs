﻿namespace Domain.Enum;

public enum NotificationType
{
    AssignedToTask,
    TaskStatusChanged,
    TaskDeleted,
    TaskCreated,
    UnassignedFromTask,
    TaskDueDateChanged,
    NewProject,
    UnassignedFromProject,
    ProjectDeleted
}
