CREATE TABLE [dbo].[UserPhoto] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [UserId]      INT             NOT NULL,
    [PhotoData]   VARBINARY (MAX) NOT NULL,
    [ContentType] NVARCHAR (100)  NOT NULL,
    [UploadDate]  DATETIME2 (7)   DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserPhoto_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [UQ_UserPhoto_UserId] UNIQUE NONCLUSTERED ([UserId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_UserPhoto_UserId]
    ON [dbo].[UserPhoto]([UserId] ASC);

