CREATE TABLE [dbo].[Character] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [MasterId] INT           NOT NULL,
    [Name]     NVARCHAR (20) NOT NULL,
    [Exp]      INT           NOT NULL,
    CONSTRAINT [PK_Character] PRIMARY KEY CLUSTERED ([Id] ASC)
);


