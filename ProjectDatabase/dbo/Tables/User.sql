CREATE TABLE [dbo].[User] (
    [Id]                     INT            IDENTITY (1, 1) NOT NULL,
    [AspNetUserId]           NVARCHAR (450) NOT NULL,
    [FirstName]              NVARCHAR (50)  NOT NULL,
    [LastName]               NVARCHAR (50)  NOT NULL,
    [Email]                  NVARCHAR (256) NOT NULL,
    [PhoneNumber]            NVARCHAR (20)  NULL,
    [RefreshToken]           NVARCHAR (50)  NULL,
    [RefreshTokenExpiryTime] DATETIME       NULL,
    [IsBlocked]              BIT            CONSTRAINT [DF_User_IsBlocked] DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_User_AspNetUsers] FOREIGN KEY ([AspNetUserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);

