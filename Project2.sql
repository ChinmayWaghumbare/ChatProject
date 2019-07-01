
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/01/2019 09:40:19
-- Generated from EDMX file: E:\Project\WebServerEnitityFramework\WebServerEntityFramework\Models\ChatMaster.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Project2];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__MESSAGEMA__FROM___7F2BE32F]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MESSAGEMAST] DROP CONSTRAINT [FK__MESSAGEMA__FROM___7F2BE32F];
GO
IF OBJECT_ID(N'[dbo].[FK__MESSAGEMA__TO_US__7E37BEF6]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MESSAGEMAST] DROP CONSTRAINT [FK__MESSAGEMA__TO_US__7E37BEF6];
GO
IF OBJECT_ID(N'[dbo].[FK__ONLINEUSE__USERI__7B5B524B]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ONLINEUSERS] DROP CONSTRAINT [FK__ONLINEUSE__USERI__7B5B524B];
GO
IF OBJECT_ID(N'[dbo].[FK__USERINFO__LOGIN___5FB337D6]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[USERINFO] DROP CONSTRAINT [FK__USERINFO__LOGIN___5FB337D6];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[CONNECTIONS]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CONNECTIONS];
GO
IF OBJECT_ID(N'[dbo].[employee]', 'U') IS NOT NULL
    DROP TABLE [dbo].[employee];
GO
IF OBJECT_ID(N'[dbo].[LOGIN]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LOGIN];
GO
IF OBJECT_ID(N'[dbo].[MESSAGEMAST]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MESSAGEMAST];
GO
IF OBJECT_ID(N'[dbo].[ONLINEUSERS]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ONLINEUSERS];
GO
IF OBJECT_ID(N'[dbo].[USERINFO]', 'U') IS NOT NULL
    DROP TABLE [dbo].[USERINFO];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'LOGINs'
CREATE TABLE [dbo].[LOGINs] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [UID] varchar(20)  NULL,
    [UPWD] varchar(20)  NULL
);
GO

-- Creating table 'MESSAGEMASTs'
CREATE TABLE [dbo].[MESSAGEMASTs] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TO_USER] int  NOT NULL,
    [FROM_USER] int  NOT NULL,
    [MSG] varchar(200)  NULL,
    [SENDTIME] datetime  NULL,
    [DELIVERED] bit  NULL,
    [MSG_PARTIAL] bit  NULL
);
GO

-- Creating table 'USERINFOes'
CREATE TABLE [dbo].[USERINFOes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [USER_NAME] varchar(20)  NULL,
    [LOGIN_ID] int  NOT NULL
);
GO

-- Creating table 'CONNECTIONS'
CREATE TABLE [dbo].[CONNECTIONS] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [FROM_USER] int  NULL,
    [TO_USER] int  NULL
);
GO

-- Creating table 'ONLINEUSERS'
CREATE TABLE [dbo].[ONLINEUSERS] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [USERID] int  NOT NULL
);
GO

-- Creating table 'employees'
CREATE TABLE [dbo].[employees] (
    [code] nvarchar(10)  NOT NULL,
    [age] int  NULL,
    [gender] nvarchar(6)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'LOGINs'
ALTER TABLE [dbo].[LOGINs]
ADD CONSTRAINT [PK_LOGINs]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'MESSAGEMASTs'
ALTER TABLE [dbo].[MESSAGEMASTs]
ADD CONSTRAINT [PK_MESSAGEMASTs]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'USERINFOes'
ALTER TABLE [dbo].[USERINFOes]
ADD CONSTRAINT [PK_USERINFOes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CONNECTIONS'
ALTER TABLE [dbo].[CONNECTIONS]
ADD CONSTRAINT [PK_CONNECTIONS]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ONLINEUSERS'
ALTER TABLE [dbo].[ONLINEUSERS]
ADD CONSTRAINT [PK_ONLINEUSERS]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [code] in table 'employees'
ALTER TABLE [dbo].[employees]
ADD CONSTRAINT [PK_employees]
    PRIMARY KEY CLUSTERED ([code] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [LOGIN_ID] in table 'USERINFOes'
ALTER TABLE [dbo].[USERINFOes]
ADD CONSTRAINT [FK__USERINFO__LOGIN___5FB337D6]
    FOREIGN KEY ([LOGIN_ID])
    REFERENCES [dbo].[LOGINs]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__USERINFO__LOGIN___5FB337D6'
CREATE INDEX [IX_FK__USERINFO__LOGIN___5FB337D6]
ON [dbo].[USERINFOes]
    ([LOGIN_ID]);
GO

-- Creating foreign key on [FROM_USER] in table 'MESSAGEMASTs'
ALTER TABLE [dbo].[MESSAGEMASTs]
ADD CONSTRAINT [FK__MESSAGEMA__FROM___7F2BE32F]
    FOREIGN KEY ([FROM_USER])
    REFERENCES [dbo].[USERINFOes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__MESSAGEMA__FROM___7F2BE32F'
CREATE INDEX [IX_FK__MESSAGEMA__FROM___7F2BE32F]
ON [dbo].[MESSAGEMASTs]
    ([FROM_USER]);
GO

-- Creating foreign key on [TO_USER] in table 'MESSAGEMASTs'
ALTER TABLE [dbo].[MESSAGEMASTs]
ADD CONSTRAINT [FK__MESSAGEMA__TO_US__7E37BEF6]
    FOREIGN KEY ([TO_USER])
    REFERENCES [dbo].[USERINFOes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__MESSAGEMA__TO_US__7E37BEF6'
CREATE INDEX [IX_FK__MESSAGEMA__TO_US__7E37BEF6]
ON [dbo].[MESSAGEMASTs]
    ([TO_USER]);
GO

-- Creating foreign key on [USERID] in table 'ONLINEUSERS'
ALTER TABLE [dbo].[ONLINEUSERS]
ADD CONSTRAINT [FK__ONLINEUSE__USERI__7B5B524B]
    FOREIGN KEY ([USERID])
    REFERENCES [dbo].[USERINFOes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__ONLINEUSE__USERI__7B5B524B'
CREATE INDEX [IX_FK__ONLINEUSE__USERI__7B5B524B]
ON [dbo].[ONLINEUSERS]
    ([USERID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------