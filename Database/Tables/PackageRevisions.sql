CREATE TABLE [dbo].[PackageRevisions]
(
    [PackageId] INT NOT NULL, 
    [VersionNumber] NVARCHAR(20) NOT NULL,
	[BlobReference] NVARCHAR(MAX) NOT NULL,
	CONSTRAINT [PK_PackageRevisions] PRIMARY KEY (PackageId, VersionNumber),
    CONSTRAINT [FK_PackageRevisions_ToPackage] FOREIGN KEY (PackageId) REFERENCES Packages(Id)
)