﻿CREATE TABLE [dbo].[Email]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [UserId] INT NOT NULL, 
    [EmailAddress] NVARCHAR(100) NOT NULL,
)
