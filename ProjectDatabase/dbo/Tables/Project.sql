CREATE TABLE [dbo].[Project] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [StartDate]   DATE           NOT NULL,
    [EndDate]     DATE           NULL,
    [StatusId]    INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Project_Status] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[Status] ([Id])
);

