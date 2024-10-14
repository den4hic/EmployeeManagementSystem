CREATE TABLE [dbo].[UserNotificationGroups] (
    [UserId]  INT NOT NULL,
    [GroupId] INT NOT NULL,
    CONSTRAINT [PK_UserNotificationGroups] PRIMARY KEY CLUSTERED ([UserId] ASC, [GroupId] ASC),
    CONSTRAINT [FK_UserNotificationGroups_NotificationGroups] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[NotificationGroups] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_UserNotificationGroups_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

