CREATE TABLE [dbo].[BinaryData]
(
    [Id] INT NOT NULL PRIMARY KEY, 
    [Data] BINARY(32) NOT NULL, 
    [NullableData] BINARY(8) NULL
)
