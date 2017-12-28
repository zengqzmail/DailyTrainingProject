
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 01/31/2013 14:16:29
-- Generated from EDMX file: c:\users\bjstratt\documents\visual studio 2010\Projects\ReactionTest\ReactionTest\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [COA_Functional];
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

-- Creating table 'SimpleReactions'
CREATE TABLE [dbo].[SimpleReactions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Color] nvarchar(max)  NOT NULL,
    [Delay] int  NOT NULL,
    [Location] nvarchar(max)  NOT NULL,
    [Height] int  NOT NULL,
    [Width] int  NOT NULL,
    [Touch] nvarchar(max)  NOT NULL,
    [Key] nvarchar(max)  NOT NULL,
    [Life] int  NOT NULL
);
GO

-- Creating table 'ComplexReactions'
CREATE TABLE [dbo].[ComplexReactions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Color] nvarchar(max)  NOT NULL,
    [Delay] int  NOT NULL,
    [Location] nvarchar(max)  NOT NULL,
    [Height] int  NOT NULL,
    [Width] int  NOT NULL,
    [Touch] nvarchar(max)  NOT NULL,
    [Key] nvarchar(max)  NOT NULL,
    [Life] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'SimpleReactions'
ALTER TABLE [dbo].[SimpleReactions]
ADD CONSTRAINT [PK_SimpleReactions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ComplexReactions'
ALTER TABLE [dbo].[ComplexReactions]
ADD CONSTRAINT [PK_ComplexReactions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------