CREATE TABLE [dbo].[Student]
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [FirstName] NVARCHAR(50) NOT NULL,
    [LastName] NVARCHAR(50) NOT NULL,
    [Address] NVARCHAR(200) NOT NULL,
    [City] NVARCHAR(50) NOT NULL,
    [State] NVARCHAR(50) NOT NULL,
    [DateOfBirth] Date NOT NULL,
    [Gender] INT NOT NULL,

    CONSTRAINT [PK_Student] PRIMARY KEY ([Id])
)
