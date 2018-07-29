CREATE TABLE [dbo].[Packages]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Identifier] NVARCHAR(128) NOT NULL, 
    [Description] NVARCHAR(MAX) NOT NULL, 
    [AuthorId] INT NOT NULL, 
    CONSTRAINT [FK_Packages_ToUser] FOREIGN KEY (AuthorId) REFERENCES Users(Id)
)