CREATE TABLE [dbo].[ProjectManager] (
    [ProjectId] INT NOT NULL,
    [ManagerId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([ProjectId] ASC, [ManagerId] ASC),
    CONSTRAINT [FK_ProjectManager_Manager] FOREIGN KEY ([ManagerId]) REFERENCES [dbo].[Manager] ([Id]),
    CONSTRAINT [FK_ProjectManager_Project] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([Id])
);

