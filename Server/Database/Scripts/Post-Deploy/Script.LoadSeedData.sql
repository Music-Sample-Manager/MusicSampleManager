/*
	Loads some sample data into the database for testing.
*/
USE msmdb
GO

INSERT Packages (Identifier)
VALUES ('LegoWelt.FreeSamples'),
	   ('LANDR.SamplePacks'),
	   ('GroundYourSound'),
	   ('LiveSchool.FreeSamples'),
	   ('Ableton.FreePacks'),
	   ('BoxedEar.FreeKitPacks')
GO