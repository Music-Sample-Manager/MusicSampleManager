/*
	Loads some sample data into the database for testing.
*/
USE msmdb
GO

DECLARE @MixGeniusAuthor VARCHAR(MAX) = 'MixGenius',
		@AbletonAuthor VARCHAR(MAX) = 'Ableton',
		@PaulBattersbyAuthor VARCHAR(MAX) = 'PaulBattersby'

INSERT Users (UserName)
VALUES (@MixGeniusAuthor),
	   (@AbletonAuthor),
	   (@PaulBattersbyAuthor)


DECLARE @Landr VARCHAR(MAX) = 'LANDR.SamplePacks',
		@LiveSchool VARCHAR(MAX) = 'LiveSchool.FreeSamples',
		@Ableton VARCHAR(MAX) = 'Ableton.FreePacks',
		@VPO VARCHAR(MAX) = 'VirtualPlayingOrchestra'

INSERT Packages (Identifier, [Description], AuthorId)
VALUES ('LegoWelt.FreeSamples', 'A collection of free samples', (SELECT Id FROM Users WHERE UserName = @MixGeniusAuthor)),
	   (@Landr, 'LANDR''s free sample pack', (SELECT Id FROM Users WHERE UserName = @MixGeniusAuthor)),
	   ('GroundYourSound', 'Some samples', (SELECT Id FROM Users WHERE UserName = @MixGeniusAuthor)),
	   (@LiveSchool, 'LiveSchool''s free sample packs', (SELECT Id FROM Users WHERE UserName = @MixGeniusAuthor)),
	   (@Ableton, 'The free samples that come with Ableton Live', (SELECT Id FROM Users WHERE UserName = @AbletonAuthor)),
	   ('BoxedEar.FreeKitPacks', 'BoxedEars'' free samples', (SELECT Id FROM Users WHERE UserName = @MixGeniusAuthor)),
	   (@VPO, 'A free, virtual orchestra.', (SELECT Id FROM Users WHERE UserName = @PaulBattersbyAuthor))


INSERT PackageRevisions (PackageId, VersionNumber)
VALUES ((SELECT Id FROM Packages WHERE Identifier = @Landr), '0.0.0.1'),
	   ((SELECT Id FROM Packages WHERE Identifier = @Landr), '0.0.0.2'),
	   ((SELECT Id FROM Packages WHERE Identifier = @Landr), '0.0.1.0'),
	   ((SELECT Id FROM Packages WHERE Identifier = @Landr), '1.0.0.0'),

	   ((SELECT Id FROM Packages WHERE Identifier = @LiveSchool), '1.0.1.0'),
	   ((SELECT Id FROM Packages WHERE Identifier = @Ableton), '1.0.1.1'),
	   ((SELECT Id FROM Packages WHERE Identifier = @VPO), '3.1.0.0')
GO