CREATE TABLE [dbo].[Task] (
    [Id]                   INT            IDENTITY (1, 1) NOT NULL,
    [ProjectId]            INT            NOT NULL,
    [AssignedToEmployeeId] INT            NULL,
    [Title]                NVARCHAR (100) NOT NULL,
    [Description]          NVARCHAR (MAX) NULL,
    [StatusId]             INT            NOT NULL,
    [DueDate]              DATE           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Task_Employee] FOREIGN KEY ([AssignedToEmployeeId]) REFERENCES [dbo].[Employee] ([Id]),
    CONSTRAINT [FK_Task_Project] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([Id]),
    CONSTRAINT [FK_Task_Status] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[Status] ([Id])
);

