CREATE TABLE [dbo].[Manager] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [UserId]     INT            NOT NULL,
    [Department] NVARCHAR (100) NOT NULL,
    [HireDate]   DATETIME       NOT NULL,
    CONSTRAINT [PK__Manager__3214EC0774CAD08C] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Manager_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Manager]
    ON [dbo].[Manager]([UserId] ASC);

