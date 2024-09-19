CREATE TABLE [dbo].[Manager] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [UserId]     INT            NOT NULL,
    [Department] NVARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Manager_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

