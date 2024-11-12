USE [HelpdeskDb]
GO

/****** Object: Table [dbo].[Departments] Script Date: 2024-11-02 2:21:02 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Departments] (
    [Id]             INT          IDENTITY (100, 100) NOT NULL,
    [DepartmentName] VARCHAR (50) NULL,
    [Timer]          ROWVERSION   NOT NULL
);


