CREATE TABLE [dbo].[UserNotifications] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [UserId]         INT           NOT NULL,
    [NotificationId] INT           NOT NULL,
    [IsRead]         BIT           DEFAULT ((0)) NOT NULL,
    [ReadAt]         DATETIME2 (7) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserNotifications_Notifications] FOREIGN KEY ([NotificationId]) REFERENCES [dbo].[Notifications] ([Id]),
    CONSTRAINT [FK_UserNotifications_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

