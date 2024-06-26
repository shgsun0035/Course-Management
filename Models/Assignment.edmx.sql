
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/06/2020 21:50:49
-- Generated from EDMX file: D:\Visual Studio Community 2019\Project\Assignment\Models\Assignment.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Assignment];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ClassroomSet'
CREATE TABLE [dbo].[ClassroomSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ClassroomName] nvarchar(max)  NOT NULL,
    [ClassroomDescription] nvarchar(max)  NOT NULL,
    [ClassroomLatitude] nvarchar(max)  NOT NULL,
    [ClassroomLongitude] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CourseSet'
CREATE TABLE [dbo].[CourseSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CourseName] nvarchar(max)  NOT NULL,
    [CourseDescription] nvarchar(max)  NOT NULL,
    [CourseTime] nvarchar(max)  NOT NULL,
    [CourseRating] nvarchar(max)  NOT NULL,
    [TrainerName] nvarchar(max)  NOT NULL,
    [ClassroomId] int  NOT NULL
);
GO

-- Creating table 'BookingSet'
CREATE TABLE [dbo].[BookingSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] nvarchar(max)  NOT NULL,
    [CourseId] int  NOT NULL
);
GO

-- Creating table 'RatingSet'
CREATE TABLE [dbo].[RatingSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RatingValue] nvarchar(max)  NOT NULL,
    [UserId] nvarchar(max)  NOT NULL,
    [CourseId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'ClassroomSet'
ALTER TABLE [dbo].[ClassroomSet]
ADD CONSTRAINT [PK_ClassroomSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CourseSet'
ALTER TABLE [dbo].[CourseSet]
ADD CONSTRAINT [PK_CourseSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BookingSet'
ALTER TABLE [dbo].[BookingSet]
ADD CONSTRAINT [PK_BookingSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RatingSet'
ALTER TABLE [dbo].[RatingSet]
ADD CONSTRAINT [PK_RatingSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ClassroomId] in table 'CourseSet'
ALTER TABLE [dbo].[CourseSet]
ADD CONSTRAINT [FK_ClassroomCourse]
    FOREIGN KEY ([ClassroomId])
    REFERENCES [dbo].[ClassroomSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClassroomCourse'
CREATE INDEX [IX_FK_ClassroomCourse]
ON [dbo].[CourseSet]
    ([ClassroomId]);
GO

-- Creating foreign key on [CourseId] in table 'BookingSet'
ALTER TABLE [dbo].[BookingSet]
ADD CONSTRAINT [FK_CourseBooking]
    FOREIGN KEY ([CourseId])
    REFERENCES [dbo].[CourseSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CourseBooking'
CREATE INDEX [IX_FK_CourseBooking]
ON [dbo].[BookingSet]
    ([CourseId]);
GO

-- Creating foreign key on [CourseId] in table 'RatingSet'
ALTER TABLE [dbo].[RatingSet]
ADD CONSTRAINT [FK_CourseRating]
    FOREIGN KEY ([CourseId])
    REFERENCES [dbo].[CourseSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CourseRating'
CREATE INDEX [IX_FK_CourseRating]
ON [dbo].[RatingSet]
    ([CourseId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------