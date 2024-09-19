CREATE TABLE [dbo].[Employee] (
    [Id]       INT             IDENTITY (1, 1) NOT NULL,
    [UserId]   INT             NOT NULL,
    [Position] NVARCHAR (100)  NOT NULL,
    [HireDate] DATE            NOT NULL,
    [Salary]   DECIMAL (10, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Employee_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

