/*
	Loads some sample data into the database for testing.
*/
USE msmdb
GO

DECLARE @Landr VARCHAR(MAX) = 'LANDR.SamplePacks',
		@LiveSchool VARCHAR(MAX) = 'LiveSchool.FreeSamples',
		@Ableton VARCHAR(MAX) = 'Ableton.FreePacks',
		@VPO VARCHAR(MAX) = 'VirtualPlayingOrchestra'

INSERT Packages (Identifier)
VALUES ('LegoWelt.FreeSamples'),
	   (@Landr),
	   ('GroundYourSound'),
	   (@LiveSchool),
	   (@Ableton),
	   ('BoxedEar.FreeKitPacks'),
	   (@VPO)


INSERT PackageRevisions (PackageId, VersionNumber)
VALUES ((SELECT Id FROM Packages WHERE Identifier = @Landr), '0.0.0.1'),
	   ((SELECT Id FROM Packages WHERE Identifier = @Landr), '0.0.0.2'),
	   ((SELECT Id FROM Packages WHERE Identifier = @Landr), '0.0.1.0'),
	   ((SELECT Id FROM Packages WHERE Identifier = @Landr), '1.0.0.0'),

	   ((SELECT Id FROM Packages WHERE Identifier = @LiveSchool), '1.0.1.0'),
	   ((SELECT Id FROM Packages WHERE Identifier = @Ableton), '1.0.1.1'),
	   ((SELECT Id FROM Packages WHERE Identifier = @Ableton), '1.0.1.1'),
	   ((SELECT Id FROM Packages WHERE Identifier = @VPO), '3.1.0.0')
GO