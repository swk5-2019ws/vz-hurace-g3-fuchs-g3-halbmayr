DROP TABLE IF EXISTS [dbo].[SeasonPlan];
DROP TABLE IF EXISTS [dbo].[Venue];
DROP TABLE IF EXISTS [dbo].[Season];

CREATE TABLE [dbo].[Season]
(
	[SeasonId] INT NOT NULL,
	[Name] VARCHAR(50) NOT NULL,
	[StartDate] DATE NOT NULL,
	[EndDate] DATE NOT NULL,
	CONSTRAINT Season_pk PRIMARY KEY ([SeasonId])
);

CREATE TABLE [dbo].[Venue]
(
	[Name] VARCHAR(30) NOT NULL,
	CONSTRAINT Venue_pk PRIMARY KEY ([Name])
);

CREATE TABLE [dbo].[SeasonPlan]
(
	[SeasonId] INT NOT NULL,
	[Venue] VARCHAR(30) NOT NULL,
	CONSTRAINT SeasonPlan_pk PRIMARY KEY ([SeasonId], [Venue]),
	CONSTRAINT SeasonPlan_Season_fk FOREIGN KEY ([SeasonId])
		REFERENCES [dbo].[Season] ([SeasonId])
);

