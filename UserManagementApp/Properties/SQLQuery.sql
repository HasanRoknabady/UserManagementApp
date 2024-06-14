-- Drop the Users table if it exists (be careful with this as it will delete all existing data)
-- DROP TABLE IF EXISTS [dbo].[Users];

-- Create the Users table
CREATE TABLE [dbo].[Users] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Username] NVARCHAR (50) NOT NULL,
    [Password] NVARCHAR (64) NOT NULL,
    [Status]   NVARCHAR (20) DEFAULT ('not available') NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Username] ASC)
);

-- If the table already exists, just update the Password column
ALTER TABLE [dbo].[Users]
ALTER COLUMN [Password] NVARCHAR(64) NOT NULL;
