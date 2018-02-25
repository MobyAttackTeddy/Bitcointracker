
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/25/2018 13:57:15
-- Generated from EDMX file: C:\Users\musk0\source\repos\BitCoin\BitCoin\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Bitcoin];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[MarketSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MarketSet];
GO
IF OBJECT_ID(N'[dbo].[WalletSetSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WalletSetSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'MarketSet'
CREATE TABLE [dbo].[MarketSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [BitValue] float  NOT NULL,
    [DateStamp] datetime  NULL
);
GO

-- Creating table 'WalletSetSet'
CREATE TABLE [dbo].[WalletSetSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [BitInDollar] float  NOT NULL,
    [DateStamp] datetime  NOT NULL,
    [Dollar] float  NOT NULL,
    [Bitcoin] float  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'MarketSet'
ALTER TABLE [dbo].[MarketSet]
ADD CONSTRAINT [PK_MarketSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'WalletSetSet'
ALTER TABLE [dbo].[WalletSetSet]
ADD CONSTRAINT [PK_WalletSetSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------