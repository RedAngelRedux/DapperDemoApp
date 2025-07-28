CREATE TABLE [dbo].[Addresses] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [UserId]        INT            NOT NULL,
    [StreetAddress] NVARCHAR (100) NOT NULL,
    [City]          NVARCHAR (50)  NOT NULL,
    [State]         VARCHAR (2)    NOT NULL,
    [ZipCode]       VARCHAR (10)   NULL,
    CONSTRAINT [PK_Addresses] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Addresses_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

