
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/19/2020 01:19:47
-- Generated from EDMX file: C:\Users\Ciprian Craciun\source\repos\Proiect2-ObjectWCF\MyPhotosWCF\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Proiect2];
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

-- Creating table 'Files'
CREATE TABLE [dbo].[Files] (
    [FileID] int IDENTITY(1,1) NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [FileDesc] nvarchar(max)  NOT NULL,
    [FileType] nvarchar(max)  NOT NULL,
    [FileSize] nvarchar(max)  NOT NULL,
    [FilePath] nvarchar(max)  NOT NULL,
    [FileTags] nvarchar(max)  NOT NULL,
    [FileDate] nvarchar(max)  NOT NULL,
    [FolderId] int  NOT NULL
);
GO

-- Creating table 'Folders'
CREATE TABLE [dbo].[Folders] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Location] nvarchar(max)  NOT NULL,
    [Date] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [FileID] in table 'Files'
ALTER TABLE [dbo].[Files]
ADD CONSTRAINT [PK_Files]
    PRIMARY KEY CLUSTERED ([FileID] ASC);
GO

-- Creating primary key on [Id] in table 'Folders'
ALTER TABLE [dbo].[Folders]
ADD CONSTRAINT [PK_Folders]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [FolderId] in table 'Files'
ALTER TABLE [dbo].[Files]
ADD CONSTRAINT [FK_FolderFile]
    FOREIGN KEY ([FolderId])
    REFERENCES [dbo].[Folders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FolderFile'
CREATE INDEX [IX_FK_FolderFile]
ON [dbo].[Files]
    ([FolderId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------