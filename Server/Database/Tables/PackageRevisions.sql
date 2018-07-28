CREATE TABLE [dbo].[PackageRevisions]
(
    [PackageId] INT NOT NULL, 
    [VersionNumber] NVARCHAR(20) NOT NULL, 
    CONSTRAINT [FK_PackageRevisions_ToPackage] FOREIGN KEY (PackageId) REFERENCES Packages(Id)
)
