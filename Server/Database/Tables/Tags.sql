﻿CREATE TABLE [dbo].[Tags]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(200) NOT NULL, 
    [Description] NVARCHAR(MAX) NOT NULL
)
