﻿CREATE TABLE [dbo].[ProjectEmployee] (
    [ProjectId]  INT NOT NULL,
    [EmployeeId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([ProjectId] ASC, [EmployeeId] ASC),
    CONSTRAINT [FK_ProjectEmployee_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_ProjectEmployee_Project] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

