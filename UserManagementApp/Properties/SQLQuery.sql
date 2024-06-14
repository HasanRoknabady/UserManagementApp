CREATE TABLE [dbo].[Users] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Username] NVARCHAR (50) NOT NULL,
    [Password] NVARCHAR (64) NOT NULL,
    [Status]   NVARCHAR (20) DEFAULT ('not available') NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Username] ASC)
);


ALTER TABLE [dbo].[Users]
ALTER COLUMN [Password] NVARCHAR(64) NOT NULL;
