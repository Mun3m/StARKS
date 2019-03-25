CREATE TABLE [dbo].[Enrollment]
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [StudentId] UNIQUEIDENTIFIER NOT NULL,
    [CourseId] UNIQUEIDENTIFIER NOT NULL,
    [Grade] INT NULL,

    CONSTRAINT [PK_Enrollment] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Enrollment_Student] FOREIGN KEY ([StudentId]) REFERENCES [dbo].[Student]([Id]),
    CONSTRAINT [FK_Enrollment_Course] FOREIGN KEY ([CourseId]) REFERENCES [dbo].[Course]([Id])
)
