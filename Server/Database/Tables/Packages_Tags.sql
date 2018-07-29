CREATE TABLE [dbo].[Packages_Tags]
(
	[PackageId] INT NOT NULL, 
    [TagId] INT NOT NULL, 
    CONSTRAINT [FK_Packages_Tags_ToPackages] FOREIGN KEY (PackageId) REFERENCES Packages(Id),
	CONSTRAINT [FK_Packages_Tags_ToTags] FOREIGN KEY (TagId) REFERENCES Tags(Id),
	CONSTRAINT [PK_Packages_Tags] PRIMARY KEY (PackageId, TagId)
)
