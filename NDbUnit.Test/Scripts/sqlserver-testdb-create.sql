-- run this using osql -E -i sqlserver-testdb-create.sql

DROP DATABASE [testdb]
GO

CREATE DATABASE [testdb]
GO

USE [testdb]
GO

CREATE TABLE [dbo].[Role] (
	[ID] [int] IDENTITY NOT NULL PRIMARY KEY,
	[Name] [varchar] (50) NOT NULL,
	[Description] [varchar] (50)
)
GO

CREATE TABLE [dbo].[User] (
	[ID] [int] IDENTITY NOT NULL PRIMARY KEY,
	[FirstName] [varchar] (50) NOT NULL,
	[LastName] [varchar] (50) NOT NULL,
	[Age] [smallint]
)
GO

CREATE TABLE [dbo].[UserRole] (
	[UserID] [int] NOT NULL REFERENCES [dbo].[User]([ID]),
	[RoleID] [int] NOT NULL REFERENCES [dbo].[Role]([ID]),
	PRIMARY KEY([UserID], [RoleID])
)
GO
