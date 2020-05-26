
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/06/2020 18:12:44
-- Generated from EDMX file: D:\Cours informatique\Doranco\C#\workspace\Semaine5_\WpfProjetFilm\Model\EFProjetFilm.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ProjetFilm];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_FilmGenre_Film]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FilmGenre] DROP CONSTRAINT [FK_FilmGenre_Film];
GO
IF OBJECT_ID(N'[dbo].[FK_FilmGenre_Genre]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FilmGenre] DROP CONSTRAINT [FK_FilmGenre_Genre];
GO
IF OBJECT_ID(N'[dbo].[FK_FilmPays_Film]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FilmPays] DROP CONSTRAINT [FK_FilmPays_Film];
GO
IF OBJECT_ID(N'[dbo].[FK_FilmPays_Pays]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FilmPays] DROP CONSTRAINT [FK_FilmPays_Pays];
GO
IF OBJECT_ID(N'[dbo].[FK_PaysDrapeau]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[drapeau] DROP CONSTRAINT [FK_PaysDrapeau];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[film]', 'U') IS NOT NULL
    DROP TABLE [dbo].[film];
GO
IF OBJECT_ID(N'[dbo].[pays]', 'U') IS NOT NULL
    DROP TABLE [dbo].[pays];
GO
IF OBJECT_ID(N'[dbo].[genre]', 'U') IS NOT NULL
    DROP TABLE [dbo].[genre];
GO
IF OBJECT_ID(N'[dbo].[drapeau]', 'U') IS NOT NULL
    DROP TABLE [dbo].[drapeau];
GO
IF OBJECT_ID(N'[dbo].[FilmGenre]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FilmGenre];
GO
IF OBJECT_ID(N'[dbo].[FilmPays]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FilmPays];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'film'
CREATE TABLE [dbo].[film] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Titre] nvarchar(max)  NOT NULL,
    [Synopsis] nvarchar(max)  NOT NULL,
    [Annee] int  NOT NULL,
    [Affiche] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'pays'
CREATE TABLE [dbo].[pays] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Libelle] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'genre'
CREATE TABLE [dbo].[genre] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Libelle] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'drapeau'
CREATE TABLE [dbo].[drapeau] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CheminImage] nvarchar(max)  NULL,
    [Pays_Id] int  NULL
);
GO

-- Creating table 'FilmGenre'
CREATE TABLE [dbo].[FilmGenre] (
    [Film_Id] int  NOT NULL,
    [Genre_Id] int  NOT NULL
);
GO

-- Creating table 'FilmPays'
CREATE TABLE [dbo].[FilmPays] (
    [Film_Id] int  NOT NULL,
    [Pays_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'film'
ALTER TABLE [dbo].[film]
ADD CONSTRAINT [PK_film]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'pays'
ALTER TABLE [dbo].[pays]
ADD CONSTRAINT [PK_pays]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'genre'
ALTER TABLE [dbo].[genre]
ADD CONSTRAINT [PK_genre]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'drapeau'
ALTER TABLE [dbo].[drapeau]
ADD CONSTRAINT [PK_drapeau]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Film_Id], [Genre_Id] in table 'FilmGenre'
ALTER TABLE [dbo].[FilmGenre]
ADD CONSTRAINT [PK_FilmGenre]
    PRIMARY KEY CLUSTERED ([Film_Id], [Genre_Id] ASC);
GO

-- Creating primary key on [Film_Id], [Pays_Id] in table 'FilmPays'
ALTER TABLE [dbo].[FilmPays]
ADD CONSTRAINT [PK_FilmPays]
    PRIMARY KEY CLUSTERED ([Film_Id], [Pays_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Film_Id] in table 'FilmGenre'
ALTER TABLE [dbo].[FilmGenre]
ADD CONSTRAINT [FK_FilmGenre_Film]
    FOREIGN KEY ([Film_Id])
    REFERENCES [dbo].[film]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Genre_Id] in table 'FilmGenre'
ALTER TABLE [dbo].[FilmGenre]
ADD CONSTRAINT [FK_FilmGenre_Genre]
    FOREIGN KEY ([Genre_Id])
    REFERENCES [dbo].[genre]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FilmGenre_Genre'
CREATE INDEX [IX_FK_FilmGenre_Genre]
ON [dbo].[FilmGenre]
    ([Genre_Id]);
GO

-- Creating foreign key on [Film_Id] in table 'FilmPays'
ALTER TABLE [dbo].[FilmPays]
ADD CONSTRAINT [FK_FilmPays_Film]
    FOREIGN KEY ([Film_Id])
    REFERENCES [dbo].[film]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Pays_Id] in table 'FilmPays'
ALTER TABLE [dbo].[FilmPays]
ADD CONSTRAINT [FK_FilmPays_Pays]
    FOREIGN KEY ([Pays_Id])
    REFERENCES [dbo].[pays]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FilmPays_Pays'
CREATE INDEX [IX_FK_FilmPays_Pays]
ON [dbo].[FilmPays]
    ([Pays_Id]);
GO

-- Creating foreign key on [Pays_Id] in table 'drapeau'
ALTER TABLE [dbo].[drapeau]
ADD CONSTRAINT [FK_PaysDrapeau]
    FOREIGN KEY ([Pays_Id])
    REFERENCES [dbo].[pays]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PaysDrapeau'
CREATE INDEX [IX_FK_PaysDrapeau]
ON [dbo].[drapeau]
    ([Pays_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------