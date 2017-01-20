CREATE TABLE [dbo].[Weapon] (
    [Id]       INT IDENTITY (1, 1) NOT NULL,
    [MasterId] INT NOT NULL,
    [Exp]      INT NOT NULL,
    CONSTRAINT [PK_Weapon] PRIMARY KEY CLUSTERED ([Id] ASC)
);


