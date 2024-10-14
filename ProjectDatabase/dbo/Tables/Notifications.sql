CREATE TABLE [dbo].[Notifications] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [GroupId]   INT            NOT NULL,
    [Message]   NVARCHAR (MAX) NOT NULL,
    [CreatedAt] DATETIME2 (7)  NOT NULL,
    [Type]      INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Notifications_NotificationGroups] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[NotificationGroups] ([Id])
);

