CREATE TABLE [dbo].[Notifications] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [GroupId]   INT            NOT NULL,
    [Message]   NVARCHAR (MAX) NULL,
    [CreatedAt] DATETIME2 (7)  NOT NULL,
    [Type]      INT            NOT NULL,
    CONSTRAINT [PK__Notifica__3214EC07BA24D4D6] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Notifications_NotificationGroups] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[NotificationGroups] ([Id])
);

