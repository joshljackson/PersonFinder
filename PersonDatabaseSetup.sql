
 /*
	This sql script can be used in MS SQL Express to create a new "person" database, a Person table in that database, and populate the table with some initial records.
	This script is to be run in SQL Server Management Studio (or Azure Data Studio) and assumes MS SQL Express is installed and connected to.
*/

USE [master]
GO

DROP DATABASE if exists [Person]
GO


CREATE DATABASE [Person]
 CONTAINMENT = NONE
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Person].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO


USE [Person]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Person](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NULL,
	[Age] [int] NULL,
	[Address] [nvarchar](200) NULL,
	[Interests] [nvarchar](200) NULL,
 CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED ([Id] ASC) ON [PRIMARY]
) ON [PRIMARY]
GO

-- Seed the database with initial person records
set identity_insert Person on;

insert into Person (Id, Name, Age, Address, Interests)
	select 1 as Id, 'Jolee Bindo' as Name, 62 as Age, 'Kashyyyk' as Address, 'Retired Jedi' as Interests union all
	select 2 as Id, 'Bastila Shan' as Name, 32 as Age, 'Dantooine' as Address, 'Battle meditation' as Interests union all
	select 3 as Id, 'Zaalbar' as Name, 35 as Age, 'Kashyyyk' as Address, 'Crossbow' as Interests union all
	select 4 as Id, 'Mission Vao' as Name, 20 as Age, 'Taris' as Address, null as Interests
;

set identity_insert Person off;

select * from Person;

