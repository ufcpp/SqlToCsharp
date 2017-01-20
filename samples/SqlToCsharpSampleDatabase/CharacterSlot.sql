CREATE TABLE [dbo].[CharacterSlot]
(
    [Id]       INT IDENTITY (1, 1) NOT NULL,
    [SlotIndex] INT NOT NULL,
    [CharacterId] INT NOT NULL,
    [Weapon1Id] INT NOT NULL,
    [Weapon2Id] INT NOT NULL,
    [Exp]      INT NOT NULL,
    CONSTRAINT [PK_CharacerSlot] PRIMARY KEY CLUSTERED ([Id] ASC)
);


