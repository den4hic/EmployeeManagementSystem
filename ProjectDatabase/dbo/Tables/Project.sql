CREATE TABLE [dbo].[Project] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [StartDate]   DATETIME       NOT NULL,
    [EndDate]     DATETIME       NULL,
    [StatusId]    INT            NOT NULL,
    CONSTRAINT [PK__Project__3214EC07D1BD2A26] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Project_Status] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[Status] ([Id])
);

