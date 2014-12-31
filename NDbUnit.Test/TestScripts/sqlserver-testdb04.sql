/****** Object:  Table [dbo].[User]    Script Date: 02/28/2010 11:32:02 ******/
USE [testdb]
;
SET ANSI_NULLS ON
;
SET QUOTED_IDENTIFIER ON
;
SET ANSI_PADDING ON
;
CREATE TABLE [dbo].[User](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Age] [int] NULL,
	[SupervisorID] [int] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
;
SET ANSI_PADDING OFF
;
/****** Object:  Table [dbo].[Role]    Script Date: 02/28/2010 11:32:02 ******/
SET ANSI_NULLS ON
;
SET QUOTED_IDENTIFIER ON
;
SET ANSI_PADDING ON
;
CREATE TABLE [dbo].[Role](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
;
SET ANSI_PADDING OFF
;
/****** Object:  Table [OtherSchema].[Item]    Script Date: 02/28/2010 11:32:02 ******/
SET ANSI_NULLS ON
;
SET QUOTED_IDENTIFIER ON
;
SET ANSI_PADDING ON
;
CREATE TABLE [OtherSchema].[Item](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
;
SET ANSI_PADDING OFF
;
/****** Object:  Table [dbo].[UserRole]    Script Date: 02/28/2010 11:32:02 ******/
SET ANSI_NULLS ON
;
SET QUOTED_IDENTIFIER ON
;
CREATE TABLE [dbo].[UserRole](
	[UserID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[RoleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
;
/****** Object:  Table [OtherSchema].[Order]    Script Date: 02/28/2010 11:32:02 ******/
SET ANSI_NULLS ON
;
SET QUOTED_IDENTIFIER ON
;
SET ANSI_PADDING ON
;
CREATE TABLE [OtherSchema].[Order](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[OrderNumber] [int] NOT NULL,
	[Description] [varchar](50) NOT NULL,
	[ItemId] [int] NOT NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
;
SET ANSI_PADDING OFF
;
/****** Object:  ForeignKey [FK_User_Supervisor]    Script Date: 02/28/2010 11:32:02 ******/
ALTER TABLE [dbo].[User]  WITH NOCHECK ADD  CONSTRAINT [FK_User_Supervisor] FOREIGN KEY([SupervisorID])
REFERENCES [dbo].[User] ([ID])
;
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Supervisor]
;
/****** Object:  ForeignKey [FK_UserRole_Role]    Script Date: 02/28/2010 11:32:02 ******/
ALTER TABLE [dbo].[UserRole]  WITH NOCHECK ADD  CONSTRAINT [FK_UserRole_Role] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Role] ([ID])
;
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_UserRole_Role]
;
/****** Object:  ForeignKey [FK_UserRole_User]    Script Date: 02/28/2010 11:32:02 ******/
ALTER TABLE [dbo].[UserRole]  WITH NOCHECK ADD  CONSTRAINT [FK_UserRole_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
;
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_UserRole_User]
;
/****** Object:  ForeignKey [FK_Order_Item]    Script Date: 02/28/2010 11:32:02 ******/
ALTER TABLE [OtherSchema].[Order]  WITH NOCHECK ADD  CONSTRAINT [FK_Order_Item] FOREIGN KEY([ItemId])
REFERENCES [OtherSchema].[Item] ([id])
;
ALTER TABLE [OtherSchema].[Order] CHECK CONSTRAINT [FK_Order_Item]
;
