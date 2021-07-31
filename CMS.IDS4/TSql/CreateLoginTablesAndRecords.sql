USE [MicroSvcLobDB]
GO

print '============================================'
print '        DROPPING STORED PROCEDURES'
print '============================================'

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id (N'[dbo].[usp_getModuleAndLinksOfCmsUser]') and OBJECTPROPERTY (id, N'IsProcedure') = 1) BEGIN
	print 'Dropping stored procedure [dbo].[usp_getModuleAndLinksOfCmsUser] ...'
    DROP PROCEDURE [dbo].usp_getModuleAndLinksOfCmsUser
	print 'Done.' + CHAR (13)
END

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id (N'[dbo].[usp_getCmsUser]') and OBJECTPROPERTY (id, N'IsProcedure') = 1) BEGIN
	print 'Dropping stored procedure [dbo].[usp_getCmsUser] ...'
    DROP PROCEDURE [dbo].usp_getCmsUser
	print 'Done.' + CHAR (13)
END

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id (N'[dbo].[usp_getRolesOfCmsUser]') and OBJECTPROPERTY (id, N'IsProcedure') = 1) BEGIN
	print 'Dropping stored procedure [dbo].[usp_getRolesOfCmsUser] ...'
    DROP PROCEDURE [dbo].[usp_getRolesOfCmsUser]
	print 'Done.' + CHAR (13)
END

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id (N'[dbo].[usp_authenticateCmsUser]') and OBJECTPROPERTY (id, N'IsProcedure') = 1) BEGIN
	print 'Dropping stored procedure [dbo].[usp_authenticateCmsUser] ...'
    DROP PROCEDURE [dbo].[usp_authenticateCmsUser]
	print 'Done.' + CHAR (13)
END

print '============================================'
print '             DROPPING TABLES'
print '============================================'

IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'CmsModuleAndLinkInfo')) BEGIN
	print 'Dropping table dbo.CmsModuleAndLinkInfo ...'
    DROP TABLE [dbo].[CmsModuleAndLinkInfo]
	print 'Done.' + CHAR (13)
END

IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'CmsSubModuleTask')) BEGIN
	print 'Dropping table dbo.CmsSubModuleTask ...'
    DROP TABLE [dbo].[CmsSubModuleTask]
	print 'Done.' + CHAR (13)
END

IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'CmsSubModule')) BEGIN
	print 'Dropping table dbo.CmsSubModule ...'
    DROP TABLE [dbo].[CmsSubModule]
	print 'Done.' + CHAR (13)
END

IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'CmsModule')) BEGIN
	print 'Dropping table dbo.CmsModule ...'
    DROP TABLE [dbo].[CmsModule]
	print 'Done.' + CHAR (13)
END

IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'CmsLoginInfo')) BEGIN
	print 'Dropping table dbo.CmsLoginInfo ...'
    DROP TABLE [dbo].[CmsLoginInfo]
	print 'Done.' + CHAR (13)
END

IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'CmsUserAndRoleInfo')) BEGIN
	print 'Dropping table dbo.CmsUserAndRoleInfo ...'
    DROP TABLE [dbo].[CmsUserAndRoleInfo]
	print 'Done.' + CHAR (13)
END

IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'CmsRole')) BEGIN
	print 'Dropping table dbo.CmsRole ...'
    DROP TABLE [dbo].[CmsRole]
	print 'Done.' + CHAR (13)
END

IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'CmsUser')) BEGIN
	print 'Dropping table dbo.CmsUser ...'
    DROP TABLE [dbo].[CmsUser]
	print 'Done.' + CHAR (13)
END

print '==============================================='
print '          DROPPING USER-DEFINED TYPES'
print '==============================================='

IF TYPE_ID (N'CmsLoginId') IS not NULL begin
	print 'Dropping user-defined type [dbo].[CmsLoginId] ...'
	drop type dbo.CmsLoginId
	print 'Done.' + CHAR (13)
END

IF TYPE_ID (N'EmailAddress') IS not NULL begin
	print 'Dropping user-defined type [dbo].[EmailAddress] ...'
	drop type dbo.EmailAddress
	print 'Done.' + CHAR (13)
END

IF TYPE_ID (N'PhoneNumber') IS not NULL begin
	print 'Dropping user-defined type [dbo].[PhoneNumber] ...'
	drop type dbo.PhoneNumber
	print 'Done.' + CHAR (13)
END

print '==============================================='
print '          DROPPING TABLE-VALUED TYPES'
print '==============================================='

IF TYPE_ID (N'tvpRoleName') IS not NULL begin
	print 'Dropping user-defined type [dbo].[tvpRoleName] ...'
	drop type dbo.tvpRoleName
	print 'Done.' + CHAR (13)
END

print '==============================================='
print '          DEFINING USER-DEFINED TYPES'
print '==============================================='

print 'Creating user-defined type [dbo].[CmsLoginId] ...'
/****** Object:  UserDefinedDataType [dbo].[CmsLoginId]    Script Date: 02-Jul-19 10:03:25 PM ******/
CREATE TYPE [dbo].[CmsLoginId] FROM [varchar](50) NOT NULL
GO
print 'Done.' + CHAR (13)

print 'Creating user-defined type [dbo].[EmailAddress] ...'
/****** Object:  UserDefinedDataType [dbo].[EmailAddress]    Script Date: 02-Jul-19 10:03:35 PM ******/
CREATE TYPE [dbo].[EmailAddress] FROM [varchar](100) NOT NULL
GO
print 'Done.' + CHAR (13)

print 'Creating user-defined type [dbo].[PhoneNumber] ...'
/****** Object:  UserDefinedDataType [dbo].[PhoneNumber]    Script Date: 02-Jul-19 10:03:48 PM ******/
CREATE TYPE [dbo].[PhoneNumber] FROM [varchar](16) NOT NULL
GO
print 'Done.' + CHAR (13)

print '==============================================='
print '   DEFINING USER-DEFINED TABLE-VALUED-TYPES'
print '==============================================='

/****** Object:  UserDefinedTableType [dbo].[tvpRoleName]    Script Date: 08-Jul-19 12:09:55 AM ******/
CREATE TYPE [dbo].[tvpRoleName] AS TABLE(
	[RoleName] [varchar](50) NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[RoleName] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO

print '==============================================='
print '          CREATING TABLE - CMS USER'
print '==============================================='

/****** Object:  Table [dbo].[CmsUser]    Script Date: 02-Jul-19 10:04:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CmsUser](
	[Id] [bigint] NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[BirthDate] [datetime] NOT NULL,
	[Gender] [char](1) NOT NULL,
	[MobilePhone] [dbo].[PhoneNumber] NULL,
	[WorkPhone] [dbo].[PhoneNumber] NULL,
	[HomePhone] [dbo].[PhoneNumber] NULL,
	[Email] [dbo].[EmailAddress] NOT NULL
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

print '==============================================='
print '        CREATING TABLE - CMS LOGIN INFO'
print '==============================================='

/****** Object:  Table [dbo].[CmsLoginInfo]    Script Date: 03-Jul-19 12:24:59 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CmsLoginInfo](
	[LoginId] [dbo].[CmsLoginId] NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[UserId] [bigint] NOT NULL,
 CONSTRAINT [PK_CmsLoginInfo] PRIMARY KEY CLUSTERED 
(
	[LoginId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CmsLoginInfo]  WITH CHECK ADD  CONSTRAINT [FK_CmsLoginInfo_CmsUser] FOREIGN KEY([UserId])
REFERENCES [dbo].[CmsUser] ([Id])
GO

ALTER TABLE [dbo].[CmsLoginInfo] CHECK CONSTRAINT [FK_CmsLoginInfo_CmsUser]
GO

print '==============================================='
print '           CREATING TABLE - CMS ROLE'
print '==============================================='

/****** Object:  Table [dbo].[CmsRole]    Script Date: 03-Jul-19 12:19:30 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CmsRole](
	[Id] [bigint] NOT NULL,
	[Name] [varchar](100) NOT NULL,
 CONSTRAINT [PK_CmsRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

print '==============================================='
print '      CREATING TABLE - CMS MODULE'
print '==============================================='

/****** Object:  Table [dbo].[CmsModule]    Script Date: 07-Jul-19 10:47:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CmsModule](
	[Id] [bigint] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_CmsModule] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

print '==============================================='
print '      CREATING TABLE - CMS SUB MODULE'
print '==============================================='

/****** Object:  Table [dbo].[CmsSubModule]    Script Date: 07-Jul-19 9:37:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CmsSubModule](
	[Id] [bigint] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[ModuleId] [bigint] NOT NULL,
 CONSTRAINT [PK_CmsSubModule] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CmsSubModule]  WITH CHECK ADD  CONSTRAINT [FK_CmsSubModule_CmsModule] FOREIGN KEY([ModuleId])
REFERENCES [dbo].[CmsModule] ([Id])
GO

ALTER TABLE [dbo].[CmsSubModule] CHECK CONSTRAINT [FK_CmsSubModule_CmsModule]
GO

print '==============================================='
print '      CREATING TABLE - CMS SUB MODULE TASKS'
print '==============================================='

/****** Object:  Table [dbo].[CmsSubModuleTask]    Script Date: 07-Jul-19 11:03:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CmsSubModuleTask](
	[Id] [bigint] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[SubModuleId] [bigint] NOT NULL,
 CONSTRAINT [PK_SubModuleTask] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CmsSubModuleTask]  WITH CHECK ADD  CONSTRAINT [FK_CmsSubModuleTask_CmsSubModule] FOREIGN KEY([SubModuleId])
REFERENCES [dbo].[CmsSubModule] ([Id])
GO

ALTER TABLE [dbo].[CmsSubModuleTask] CHECK CONSTRAINT [FK_CmsSubModuleTask_CmsSubModule]
GO

print '==============================================='
print '      CREATING TABLE - CMS ROLE AND USER'
print '==============================================='

/****** Object:  Table [dbo].[CmsUserAndRoleInfo]    Script Date: 02-Jul-19 9:43:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CmsUserAndRoleInfo](
	[UserId] [bigint] NOT NULL,
	[RoleId] [bigint] NOT NULL,
	[ValidFrom] [datetime] NOT NULL,
	[ValidTill] [datetime] NOT NULL,
	[IsActive] [char](1) NOT NULL,
 CONSTRAINT [PK_CmsUserAndRoleInfo] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CmsUserAndRoleInfo]  WITH CHECK ADD  CONSTRAINT [FK_CmsUserAndRoleInfo_CmsRole] FOREIGN KEY([RoleId])
REFERENCES [dbo].[CmsRole] ([Id])
GO

ALTER TABLE [dbo].[CmsUserAndRoleInfo] CHECK CONSTRAINT [FK_CmsUserAndRoleInfo_CmsRole]
GO

ALTER TABLE [dbo].[CmsUserAndRoleInfo]  WITH CHECK ADD  CONSTRAINT [FK_CmsUserAndRoleInfo_CmsUser] FOREIGN KEY([UserId])
REFERENCES [dbo].[CmsUser] ([Id])
GO

ALTER TABLE [dbo].[CmsUserAndRoleInfo] CHECK CONSTRAINT [FK_CmsUserAndRoleInfo_CmsUser]
GO

print '==============================================='
print '      CREATING TABLE - CMS MODULE AND LINK INFO'
print '==============================================='

/****** Object:  Table [dbo].[CmsModuleAndLinkInfo]    Script Date: 08-Jul-19 12:29:56 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CmsModuleAndLinkInfo](
	[CmsSubModuleTaskId] [bigint] NOT NULL,
	[RelativeUri] [varchar](300) NOT NULL,
	[CmsRoleId] [bigint] NOT NULL,
 CONSTRAINT [PK_CmsModuleAndLinkInfo] PRIMARY KEY CLUSTERED 
(
	[CmsSubModuleTaskId] ASC,
	[RelativeUri] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CmsModuleAndLinkInfo]  WITH CHECK ADD  CONSTRAINT [FK_CmsModuleAndLinkInfo_CmsRole] FOREIGN KEY([CmsRoleId])
REFERENCES [dbo].[CmsRole] ([Id])
GO

ALTER TABLE [dbo].[CmsModuleAndLinkInfo] CHECK CONSTRAINT [FK_CmsModuleAndLinkInfo_CmsRole]
GO

ALTER TABLE [dbo].[CmsModuleAndLinkInfo]  WITH CHECK ADD  CONSTRAINT [FK_CmsModuleAndLinkInfo_CmsSubModuleTask] FOREIGN KEY([CmsSubModuleTaskId])
REFERENCES [dbo].[CmsSubModuleTask] ([Id])
GO

ALTER TABLE [dbo].[CmsModuleAndLinkInfo] CHECK CONSTRAINT [FK_CmsModuleAndLinkInfo_CmsSubModuleTask]
GO

print '==============================================='
print '          DATA INSERTION INTO TABLES'
print '==============================================='

print 'Inserting CMS Role records ...'

delete from dbo.CmsRole
go

insert into dbo.CmsRole values (0, 'Administrator')
go

insert into dbo.CmsRole values (10101, 'Quotes Management')
insert into dbo.CmsRole values (10102, 'Quotes Archive')
insert into dbo.CmsRole values (10201, 'Proposals Management')
insert into dbo.CmsRole values (10202, 'Broker Management')
insert into dbo.CmsRole values (10301, 'Policy Issuance')
insert into dbo.CmsRole values (10302, 'Endorsements')
insert into dbo.CmsRole values (10401, 'Claims Management')
go

print 'Done.' + CHAR (13)

print 'Inserting CMS User records ...'

delete from dbo.CmsUser
go

insert into dbo.CmsUser values (0, 'Christopher', 'Nolan', '01-JUN-1973', 'M', '6789012345', NULL, NULL, 'christopher.nolan@fourwallsinc.com')
go

insert into dbo.CmsUser values (101, 'Tom', 'Hanks', '01-JUN-1973', 'M', '9481805674', NULL, NULL, 'tom.hanks@fourwallsinc.com')
insert into dbo.CmsUser values (102, 'Steven', 'Spielberg', '01-JAN-1975', 'M', '2345678901', NULL, NULL, 'steven.spielberg@fourwallsinc.com')
insert into dbo.CmsUser values (103, 'Harrison', 'Ford', '02-FEB-1975', 'M', '3456789012', NULL, NULL, 'harrison.ford@fourwallsinc.com')
insert into dbo.CmsUser values (104, 'Will', 'Smith', '03-MAR-1975', 'M', '4567890123', NULL, NULL, 'will.smith@fourwallsinc.com')
insert into dbo.CmsUser values (105, 'James', 'Cameron', '04-APR-1975', 'M', '5678901234', NULL, NULL, 'james.cameron@fourwallsinc.com')
go

print 'Done.' + CHAR (13)

print 'Inserting CMS Login Info records ...'

delete from dbo.CmsLoginInfo
go

insert into dbo.CmsLoginInfo values ('christopher', '123', 0)
insert into dbo.CmsLoginInfo values ('tom', '123', 101)
insert into dbo.CmsLoginInfo values ('steven', '123', 102)
insert into dbo.CmsLoginInfo values ('harrison', '123', 103)
insert into dbo.CmsLoginInfo values ('will', '123', 104)
insert into dbo.CmsLoginInfo values ('james', '123', 105)
go

print 'Done.' + CHAR (13)

print 'Inserting CMS Uesr and Role records ...'

delete from dbo.CmsUserAndRoleInfo
go

insert into dbo.CmsUserAndRoleInfo values (104, 10101, '01-JAN-2000', '31-DEC-2099', 'Y')		-- Will Smith, Quotes Management
insert into dbo.CmsUserAndRoleInfo values (101, 10101, '01-JAN-2000', '31-DEC-2099', 'Y')		-- Tom Hanks, Quotes Management
insert into dbo.CmsUserAndRoleInfo values (101, 10102, '01-JAN-2000', '31-DEC-2099', 'Y')		-- Tom Hanks, Quotes Archive
insert into dbo.CmsUserAndRoleInfo values (101, 10301, '01-JAN-2000', '31-DEC-2099', 'Y')		-- Tom Hanks, Policy Issuance
insert into dbo.CmsUserAndRoleInfo values (102, 10201, '01-JAN-2000', '31-DEC-2099', 'Y')		-- Steven Spielberg, Proposals Management
insert into dbo.CmsUserAndRoleInfo values (102, 10202, '01-JAN-2000', '31-DEC-2099', 'Y')		-- Steven Spielberg, Broker Management
insert into dbo.CmsUserAndRoleInfo values (102, 10302, '01-JAN-2000', '31-DEC-2099', 'Y')		-- Steven Spielberg, Endorsements
insert into dbo.CmsUserAndRoleInfo values (105, 10401, '01-JAN-2000', '31-DEC-2099', 'Y')		-- James Cameron, Claims Management
go

print 'Inserting CMS Module records ...'

delete from dbo.CmsModule
go

insert into dbo.CmsModule values (101, 'Quotes')
insert into dbo.CmsModule values (102, 'Proposals')
insert into dbo.CmsModule values (103, 'Policy Administration')
insert into dbo.CmsModule values (104, 'Claims')

print 'Inserting CMS Sub Module records ...'

delete from dbo.CmsSubModule
go

insert into dbo.CmsSubModule values (10101, 'Quotes Management', 101)
insert into dbo.CmsSubModule values (10102, 'Quotes Archive', 101)
insert into dbo.CmsSubModule values (10201, 'Proposals Management', 102)
insert into dbo.CmsSubModule values (10202, 'Broker Management', 102)
insert into dbo.CmsSubModule values (10301, 'Policy Issuance', 103)
insert into dbo.CmsSubModule values (10302, 'Endorsements', 103)
insert into dbo.CmsSubModule values (10401, 'Claims Management', 104)

print 'Done.' + CHAR (13)

print 'Inserting CMS Sub Module Tasks records ...'

delete from dbo.CmsSubModuleTask
go

insert into dbo.CmsSubModuleTask values (1010101, 'Issue Quote', 10101)			-- Quotes Management
insert into dbo.CmsSubModuleTask values (1010102, 'Modify Quote', 10101)			-- Quotes Management
insert into dbo.CmsSubModuleTask values (1010201, 'Search Quote', 10102)	-- Quotes Archive
insert into dbo.CmsSubModuleTask values (1010202, 'Archive Quote', 10102)	-- Quotes Archive
insert into dbo.CmsSubModuleTask values (1020101, 'Convert Quote to Proposal', 10201)		-- Proposals Management
insert into dbo.CmsSubModuleTask values (1020102, 'Search Proposals', 10201)			-- Proposals Management
insert into dbo.CmsSubModuleTask values (1020201, 'Manage Brokers', 10202)			-- Proposals Management
insert into dbo.CmsSubModuleTask values (1020202, 'Search Brokers', 10202)				-- Proposals Management
insert into dbo.CmsSubModuleTask values (1030101, 'Issue Policy', 10301)				-- Policy Issuance
insert into dbo.CmsSubModuleTask values (1030102, 'Search Policies', 10301)			-- Policy Issuance
insert into dbo.CmsSubModuleTask values (1030201, 'Add Endorsements', 10302)			-- Endorsements
insert into dbo.CmsSubModuleTask values (1030202, 'Search Endorsements', 10302)		-- Endorsements
insert into dbo.CmsSubModuleTask values (1040101, 'Raise New Claim', 10401)			-- Claims

print 'Done.' + CHAR (13)

print 'Inserting CMS Module and Link Info records ...'

INSERT INTO dbo.CmsModuleAndLinkInfo VALUES (1010101, 'http://localhost:57364/Quotes/IssueQuote', 10101)
INSERT INTO dbo.CmsModuleAndLinkInfo VALUES (1010102, 'http://localhost:57364/Quotes/ModifyQuote', 10101)
INSERT INTO dbo.CmsModuleAndLinkInfo VALUES (1010201, 'http://localhost:51289/NotImplemented/Index', 10102)
INSERT INTO dbo.CmsModuleAndLinkInfo VALUES (1010202, 'http://localhost:51289/NotImplemented/Index', 10102)
INSERT INTO dbo.CmsModuleAndLinkInfo VALUES (1020101, 'http://localhost:53458/Proposals/ConvertQuoteToProposal', 10201)
INSERT INTO dbo.CmsModuleAndLinkInfo VALUES (1020102, 'http://localhost:53458/Proposals/Search', 10201)
INSERT INTO dbo.CmsModuleAndLinkInfo VALUES (1020201, 'http://localhost:51289/NotImplemented/Index', 10202)
INSERT INTO dbo.CmsModuleAndLinkInfo VALUES (1020202, 'http://localhost:51289/NotImplemented/Index', 10202)
INSERT INTO dbo.CmsModuleAndLinkInfo VALUES (1030101, 'http://localhost:51289/NotImplemented/Index', 10301)
INSERT INTO dbo.CmsModuleAndLinkInfo VALUES (1030102, 'http://localhost:51289/NotImplemented/Index', 10301)
INSERT INTO dbo.CmsModuleAndLinkInfo VALUES (1030201, 'http://localhost:51289/NotImplemented/Index', 10302)
INSERT INTO dbo.CmsModuleAndLinkInfo VALUES (1030202, 'http://localhost:51289/NotImplemented/Index', 10302)
INSERT INTO dbo.CmsModuleAndLinkInfo VALUES (1040101, 'http://localhost:51289/NotImplemented/Index', 10401)

print 'Done.' + CHAR (13)

print '==============================================='
print '          STORED PROCEDURES CREATION'
print '==============================================='

print 'Creating stored procedure [dbo].[usp_getRolesOfCmsUser] ...'

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dinesh Jayadevan
-- Create date: 02-JUL-2019
-- Description:	Helps get roles of a CMS user.
-- =============================================
CREATE PROCEDURE [dbo].[usp_getRolesOfCmsUser] 
	@cmsLoginId varchar(50)
AS BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		cr.[Id],
		cr.[Name]
	FROM
		dbo.CmsUserAndRoleInfo AS cur
			INNER JOIN dbo.CmsUser AS cu
				on cu.Id = cur.UserId
			INNER JOIN dbo.CmsLoginInfo cli
				on cu.Id = cli.UserId
			INNER JOIN dbo.CmsRole AS cr
				on cr.Id = cur.RoleId
	WHERE
		cli.LoginId = @cmsLoginId
	ORDER BY
		cu.FirstName,
		cu.LastName
END

GO

print 'Done.' + CHAR (13)

print 'Creating stored procedure [dbo].[usp_authenticateCmsUser] ...'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dinesh Jayadevan
-- Create date: 02-JUL-2019
-- Description:	Checks if the specified CMS credentials are valid
-- =============================================
CREATE PROCEDURE [dbo].[usp_authenticateCmsUser] 
	-- Add the parameters for the stored procedure here
	@cmsLoginId [dbo].[CmsLoginId],
	@cmsPassword varchar(50),
	@areCredentialsValid bit output
AS BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF
		(
			EXISTS
				(
					SELECT 1
					FROM [dbo].[CmsLoginInfo] AS cli
					WHERE cli.LoginId = @cmsLoginId
						AND cli.[Password] = @cmsPassword
				)
		) BEGIN
		SET @areCredentialsValid = 1
	END ELSE BEGIN
		SET @areCredentialsValid = 0
	END
END

GO

print 'Done.' + CHAR (13)

print 'Creating stored procedure [dbo].[usp_getCmsUser] ...'

/****** Object:  StoredProcedure [dbo].[usp_getCmsUser]    Script Date: 06-Jul-19 10:33:48 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_getCmsUser]
	@cmsLoginId varchar(50)
AS BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT top 1
		cu.[Id],
		cu.FirstName,
		cu.LastName,
		cu.Gender,
		cu.BirthDate,
		cu.Email,
		cu.HomePhone,
		cu.MobilePhone,
		cu.WorkPhone
	FROM
		dbo.CmsUser AS cu
			INNER JOIN dbo.CmsLoginInfo AS cli
				ON cli.UserId = cu.Id
	WHERE
		cli.LoginId = @cmsLoginId
	ORDER BY
		cu.FirstName,
		cu.LastName
END

GO

print 'Done.' + CHAR (13)

print 'Creating stored procedure [dbo].[usp_getModuleAndLinksOfCmsUser] ...'

/****** Object:  StoredProcedure [dbo].[usp_getModuleAndLinksOfCmsUser]    Script Date: 06-Jul-19 12:37:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_getModuleAndLinksOfCmsUser] 
	@roleNames [dbo].[tvpRoleName] readonly
AS BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		cm.Name AS ModuleName,
		csm.Name AS SubModuleName,
		cmt.Name AS ClickableLinkDisplayLabel,
		cml.RelativeUri,
		cml.CmsRoleId
	FROM
		[dbo].[CmsModuleAndLinkInfo] AS cml
			INNER JOIN [dbo].[CmsSubModuleTask] AS cmt
				on cmt.Id = cml.CmsSubModuleTaskId
			INNER JOIN [dbo].[CmsSubModule] AS csm
				on csm.Id = cmt.SubModuleId
			INNER JOIN [dbo].[CmsModule] AS cm
				on cm.Id = csm.ModuleId
			INNER JOIN [dbo].[CmsRole] cr
				ON cr.Id = cml.CmsRoleId
	WHERE
		cr.Name in (select RoleName from @roleNames)
	ORDER BY
		cm.Id,
		csm.Id,
		cmt.Id
END

GO

print 'Done.' + CHAR (13)

print '================================================================='
print '          DB OBJECTS AND TEST DATA CREATION IS COMPLETE'
print '================================================================='
