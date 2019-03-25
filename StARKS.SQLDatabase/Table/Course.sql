CREATE TABLE [dbo].[Course]
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Code] INT NOT NULL,
    [Name] NVARCHAR(50) NOT NULL,
    [Description] NVARCHAR(max) NULL,

    CONSTRAINT [PK_Course] PRIMARY KEY ([Id]),
    CONSTRAINT [UNQ_Course_Code] UNIQUE ([Code])
)
