
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 01/07/2013 15:28:50
-- Generated from EDMX file: C:\Users\bjstratt\Documents\Visual Studio 2010\Projects\MainMenu-bran\MainMenu-bran\MainMenu.edmx
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

IF OBJECT_ID(N'[dbo].[FK_ParticipantParticipantExternalStudyAssociative]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ParticipantExternalStudyAssociatives] DROP CONSTRAINT [FK_ParticipantParticipantExternalStudyAssociative];
GO
IF OBJECT_ID(N'[dbo].[FK_AssesorParticipant]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Participants] DROP CONSTRAINT [FK_AssesorParticipant];
GO
IF OBJECT_ID(N'[dbo].[FK_AssesorStudy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Studies] DROP CONSTRAINT [FK_AssesorStudy];
GO
IF OBJECT_ID(N'[dbo].[FK_ParticipantTaskInstance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskInstances] DROP CONSTRAINT [FK_ParticipantTaskInstance];
GO
IF OBJECT_ID(N'[dbo].[FK_ExternalStudyParticipantExternalStudyAssociative]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ParticipantExternalStudyAssociatives] DROP CONSTRAINT [FK_ExternalStudyParticipantExternalStudyAssociative];
GO
IF OBJECT_ID(N'[dbo].[FK_StudyTaskInstance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskInstances] DROP CONSTRAINT [FK_StudyTaskInstance];
GO
IF OBJECT_ID(N'[dbo].[FK_SiteStudy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Studies] DROP CONSTRAINT [FK_SiteStudy];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskTypeTaskInstance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskInstances] DROP CONSTRAINT [FK_TaskTypeTaskInstance];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskInstanceTaskData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskDatas] DROP CONSTRAINT [FK_TaskInstanceTaskData];
GO
IF OBJECT_ID(N'[dbo].[FK_StudyParticipant]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Participants] DROP CONSTRAINT [FK_StudyParticipant];
GO
IF OBJECT_ID(N'[dbo].[FK_SiteParticipant]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Participants] DROP CONSTRAINT [FK_SiteParticipant];
GO
IF OBJECT_ID(N'[dbo].[FK_SiteTaskData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskDatas] DROP CONSTRAINT [FK_SiteTaskData];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Participants]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Participants];
GO
IF OBJECT_ID(N'[dbo].[ParticipantExternalStudyAssociatives]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ParticipantExternalStudyAssociatives];
GO
IF OBJECT_ID(N'[dbo].[ExternalStudies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ExternalStudies];
GO
IF OBJECT_ID(N'[dbo].[TaskInstances]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskInstances];
GO
IF OBJECT_ID(N'[dbo].[Assesors]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Assesors];
GO
IF OBJECT_ID(N'[dbo].[Studies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Studies];
GO
IF OBJECT_ID(N'[dbo].[Sites]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sites];
GO
IF OBJECT_ID(N'[dbo].[TaskTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskTypes];
GO
IF OBJECT_ID(N'[dbo].[TaskDatas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskDatas];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Participants'
CREATE TABLE [dbo].[Participants] (
    [ParticipantId] int IDENTITY(1,1) NOT NULL,
    [Age] smallint  NOT NULL,
    [Gender] nvarchar(max)  NOT NULL,
    [EffectiveDate] datetime  NOT NULL,
    [AssesorId] int  NOT NULL,
    [StudyId] int  NOT NULL,
    [SiteId] int  NOT NULL
);
GO

-- Creating table 'ParticipantExternalStudyAssociatives'
CREATE TABLE [dbo].[ParticipantExternalStudyAssociatives] (
    [PartExtStudyAssId] int IDENTITY(1,1) NOT NULL,
    [ExtStudyParticipantId] nvarchar(max)  NOT NULL,
    [ParticipantId] int  NOT NULL,
    [ExternalStudyId] int  NOT NULL
);
GO

-- Creating table 'ExternalStudies'
CREATE TABLE [dbo].[ExternalStudies] (
    [ExternalStudyId] int IDENTITY(1,1) NOT NULL,
    [ExternalStudyDesc] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'TaskInstances'
CREATE TABLE [dbo].[TaskInstances] (
    [TaskInstanceId] int IDENTITY(1,1) NOT NULL,
    [TaskTitle] nvarchar(max)  NOT NULL,
    [Status] nvarchar(max)  NOT NULL,
    [StartDate] datetime  NOT NULL,
    [ParticipantId] int  NOT NULL,
    [StudyId] int  NOT NULL,
    [TaskTypeId] int  NOT NULL
);
GO

-- Creating table 'Assesors'
CREATE TABLE [dbo].[Assesors] (
    [AssesorId] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [AssesorName] nvarchar(max)  NOT NULL,
    [AssesorTitle] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Studies'
CREATE TABLE [dbo].[Studies] (
    [StudyId] int IDENTITY(1,1) NOT NULL,
    [StudyTitle] nvarchar(max)  NOT NULL,
    [Examiner] nvarchar(max)  NOT NULL,
    [AssesorId] int  NOT NULL,
    [SiteId] int  NOT NULL
);
GO

-- Creating table 'Sites'
CREATE TABLE [dbo].[Sites] (
    [SiteId] int IDENTITY(1,1) NOT NULL,
    [SiteTitle] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'TaskTypes'
CREATE TABLE [dbo].[TaskTypes] (
    [TaskTypeId] int IDENTITY(1,1) NOT NULL,
    [TaskTypeDesc] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'TaskDatas'
CREATE TABLE [dbo].[TaskDatas] (
    [TaskDataId] int IDENTITY(1,1) NOT NULL,
    [Time] time  NOT NULL,
    [EventType] nvarchar(max)  NOT NULL,
    [EventData] nvarchar(max)  NOT NULL,
    [TaskInstanceId] int  NOT NULL,
    [EventSummary] nvarchar(max)  NULL,
    [SiteId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ParticipantId] in table 'Participants'
ALTER TABLE [dbo].[Participants]
ADD CONSTRAINT [PK_Participants]
    PRIMARY KEY CLUSTERED ([ParticipantId] ASC);
GO

-- Creating primary key on [PartExtStudyAssId] in table 'ParticipantExternalStudyAssociatives'
ALTER TABLE [dbo].[ParticipantExternalStudyAssociatives]
ADD CONSTRAINT [PK_ParticipantExternalStudyAssociatives]
    PRIMARY KEY CLUSTERED ([PartExtStudyAssId] ASC);
GO

-- Creating primary key on [ExternalStudyId] in table 'ExternalStudies'
ALTER TABLE [dbo].[ExternalStudies]
ADD CONSTRAINT [PK_ExternalStudies]
    PRIMARY KEY CLUSTERED ([ExternalStudyId] ASC);
GO

-- Creating primary key on [TaskInstanceId] in table 'TaskInstances'
ALTER TABLE [dbo].[TaskInstances]
ADD CONSTRAINT [PK_TaskInstances]
    PRIMARY KEY CLUSTERED ([TaskInstanceId] ASC);
GO

-- Creating primary key on [AssesorId] in table 'Assesors'
ALTER TABLE [dbo].[Assesors]
ADD CONSTRAINT [PK_Assesors]
    PRIMARY KEY CLUSTERED ([AssesorId] ASC);
GO

-- Creating primary key on [StudyId] in table 'Studies'
ALTER TABLE [dbo].[Studies]
ADD CONSTRAINT [PK_Studies]
    PRIMARY KEY CLUSTERED ([StudyId] ASC);
GO

-- Creating primary key on [SiteId] in table 'Sites'
ALTER TABLE [dbo].[Sites]
ADD CONSTRAINT [PK_Sites]
    PRIMARY KEY CLUSTERED ([SiteId] ASC);
GO

-- Creating primary key on [TaskTypeId] in table 'TaskTypes'
ALTER TABLE [dbo].[TaskTypes]
ADD CONSTRAINT [PK_TaskTypes]
    PRIMARY KEY CLUSTERED ([TaskTypeId] ASC);
GO

-- Creating primary key on [TaskDataId] in table 'TaskDatas'
ALTER TABLE [dbo].[TaskDatas]
ADD CONSTRAINT [PK_TaskDatas]
    PRIMARY KEY CLUSTERED ([TaskDataId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ParticipantId] in table 'ParticipantExternalStudyAssociatives'
ALTER TABLE [dbo].[ParticipantExternalStudyAssociatives]
ADD CONSTRAINT [FK_ParticipantParticipantExternalStudyAssociative]
    FOREIGN KEY ([ParticipantId])
    REFERENCES [dbo].[Participants]
        ([ParticipantId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ParticipantParticipantExternalStudyAssociative'
CREATE INDEX [IX_FK_ParticipantParticipantExternalStudyAssociative]
ON [dbo].[ParticipantExternalStudyAssociatives]
    ([ParticipantId]);
GO

-- Creating foreign key on [AssesorId] in table 'Participants'
ALTER TABLE [dbo].[Participants]
ADD CONSTRAINT [FK_AssesorParticipant]
    FOREIGN KEY ([AssesorId])
    REFERENCES [dbo].[Assesors]
        ([AssesorId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AssesorParticipant'
CREATE INDEX [IX_FK_AssesorParticipant]
ON [dbo].[Participants]
    ([AssesorId]);
GO

-- Creating foreign key on [AssesorId] in table 'Studies'
ALTER TABLE [dbo].[Studies]
ADD CONSTRAINT [FK_AssesorStudy]
    FOREIGN KEY ([AssesorId])
    REFERENCES [dbo].[Assesors]
        ([AssesorId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AssesorStudy'
CREATE INDEX [IX_FK_AssesorStudy]
ON [dbo].[Studies]
    ([AssesorId]);
GO

-- Creating foreign key on [ParticipantId] in table 'TaskInstances'
ALTER TABLE [dbo].[TaskInstances]
ADD CONSTRAINT [FK_ParticipantTaskInstance]
    FOREIGN KEY ([ParticipantId])
    REFERENCES [dbo].[Participants]
        ([ParticipantId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ParticipantTaskInstance'
CREATE INDEX [IX_FK_ParticipantTaskInstance]
ON [dbo].[TaskInstances]
    ([ParticipantId]);
GO

-- Creating foreign key on [ExternalStudyId] in table 'ParticipantExternalStudyAssociatives'
ALTER TABLE [dbo].[ParticipantExternalStudyAssociatives]
ADD CONSTRAINT [FK_ExternalStudyParticipantExternalStudyAssociative]
    FOREIGN KEY ([ExternalStudyId])
    REFERENCES [dbo].[ExternalStudies]
        ([ExternalStudyId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ExternalStudyParticipantExternalStudyAssociative'
CREATE INDEX [IX_FK_ExternalStudyParticipantExternalStudyAssociative]
ON [dbo].[ParticipantExternalStudyAssociatives]
    ([ExternalStudyId]);
GO

-- Creating foreign key on [StudyId] in table 'TaskInstances'
ALTER TABLE [dbo].[TaskInstances]
ADD CONSTRAINT [FK_StudyTaskInstance]
    FOREIGN KEY ([StudyId])
    REFERENCES [dbo].[Studies]
        ([StudyId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_StudyTaskInstance'
CREATE INDEX [IX_FK_StudyTaskInstance]
ON [dbo].[TaskInstances]
    ([StudyId]);
GO

-- Creating foreign key on [SiteId] in table 'Studies'
ALTER TABLE [dbo].[Studies]
ADD CONSTRAINT [FK_SiteStudy]
    FOREIGN KEY ([SiteId])
    REFERENCES [dbo].[Sites]
        ([SiteId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SiteStudy'
CREATE INDEX [IX_FK_SiteStudy]
ON [dbo].[Studies]
    ([SiteId]);
GO

-- Creating foreign key on [TaskTypeId] in table 'TaskInstances'
ALTER TABLE [dbo].[TaskInstances]
ADD CONSTRAINT [FK_TaskTypeTaskInstance]
    FOREIGN KEY ([TaskTypeId])
    REFERENCES [dbo].[TaskTypes]
        ([TaskTypeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskTypeTaskInstance'
CREATE INDEX [IX_FK_TaskTypeTaskInstance]
ON [dbo].[TaskInstances]
    ([TaskTypeId]);
GO

-- Creating foreign key on [TaskInstanceId] in table 'TaskDatas'
ALTER TABLE [dbo].[TaskDatas]
ADD CONSTRAINT [FK_TaskInstanceTaskData]
    FOREIGN KEY ([TaskInstanceId])
    REFERENCES [dbo].[TaskInstances]
        ([TaskInstanceId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskInstanceTaskData'
CREATE INDEX [IX_FK_TaskInstanceTaskData]
ON [dbo].[TaskDatas]
    ([TaskInstanceId]);
GO

-- Creating foreign key on [StudyId] in table 'Participants'
ALTER TABLE [dbo].[Participants]
ADD CONSTRAINT [FK_StudyParticipant]
    FOREIGN KEY ([StudyId])
    REFERENCES [dbo].[Studies]
        ([StudyId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_StudyParticipant'
CREATE INDEX [IX_FK_StudyParticipant]
ON [dbo].[Participants]
    ([StudyId]);
GO

-- Creating foreign key on [SiteId] in table 'Participants'
ALTER TABLE [dbo].[Participants]
ADD CONSTRAINT [FK_SiteParticipant]
    FOREIGN KEY ([SiteId])
    REFERENCES [dbo].[Sites]
        ([SiteId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SiteParticipant'
CREATE INDEX [IX_FK_SiteParticipant]
ON [dbo].[Participants]
    ([SiteId]);
GO

-- Creating foreign key on [SiteId] in table 'TaskDatas'
ALTER TABLE [dbo].[TaskDatas]
ADD CONSTRAINT [FK_SiteTaskData]
    FOREIGN KEY ([SiteId])
    REFERENCES [dbo].[Sites]
        ([SiteId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SiteTaskData'
CREATE INDEX [IX_FK_SiteTaskData]
ON [dbo].[TaskDatas]
    ([SiteId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------