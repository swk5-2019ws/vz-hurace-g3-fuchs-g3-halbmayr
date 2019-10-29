CREATE SCHEMA [Hurace];
GO

CREATE TABLE [Hurace].[Season]
(
	[SeasonId] INT NOT NULL IDENTITY,
	[Name] VARCHAR(30) NOT NULL,
	[StartDate] DATE NOT NULL,
	[EndDate] DATE NOT NULL,
	CONSTRAINT Season_pk PRIMARY KEY ([SeasonId])
);

CREATE TABLE [Hurace].[Venue]
(
	[Name] VARCHAR(30) NOT NULL,
	CONSTRAINT Venue_pk PRIMARY KEY ([Name])
);

CREATE TABLE [Hurace].[SeasonPlan]
(
	[SeasonId] INT NOT NULL,
	[Venue] VARCHAR(30) NOT NULL,
	CONSTRAINT SeasonPlan_pk PRIMARY KEY ([SeasonId], [Venue]),
	CONSTRAINT SeasonPlan_Season_fk FOREIGN KEY ([SeasonId])
		REFERENCES [Hurace].[Season] ([SeasonId]),
	CONSTRAINT SeasonPlan_Venue_fk FOREIGN KEY ([Venue])
		REFERENCES [Hurace].[Venue] ([Name])
);

CREATE TABLE [Hurace].[Country]
(
    [Name] VARCHAR(50) NOT NULL,
    CONSTRAINT Country_pk PRIMARY KEY ([Name])
);

CREATE TABLE [Hurace].[Image]
(
    [ImageId] INT NOT NULL IDENTITY,
    [Content] VARBINARY(MAX) NOT NULL,
    CONSTRAINT Image_pk PRIMARY KEY ([ImageId])
);

CREATE TABLE [Hurace].[Sex]
(
	[Label] VARCHAR(25) NOT NULL,
	CONSTRAINT Sex_pk PRIMARY KEY ([Label])
);

CREATE TABLE [Hurace].[StartList]
(
    [StartListId] INT NOT NULL IDENTITY,
    CONSTRAINT StartList_pk PRIMARY KEY ([StartListId])
);

CREATE TABLE [Hurace].[Skier]
(
    [SkierId] INT NOT NULL IDENTITY,
    [FirstName] VARCHAR(50) NOT NULL,
    [LastName] VARCHAR(50) NOT NULL,
    [DateOfBirth] DATE NOT NULL,
    [Country] VARCHAR(50) NOT NULL,
    [ImageId] INT NULL,
    [Sex] VARCHAR(25) NOT NULL,
    CONSTRAINT Skier_pk PRIMARY KEY ([SkierId]),
	CONSTRAINT Skier_Country_fk FOREIGN KEY ([Country])
		REFERENCES [Hurace].[Country] ([Name]),
	CONSTRAINT Skier_Image_fk FOREIGN KEY ([ImageId])
		REFERENCES [Hurace].[Image] ([ImageId]),
	CONSTRAINT Skier_Sex_fk FOREIGN KEY ([Sex])
		REFERENCES [Hurace].[Sex] ([Label])
);

CREATE TABLE [Hurace].[StartPosition]
(
    [SkierId] INT NOT NULL,
    [StartListId] INT NOT NULL,
    [Position] INT NOT NULL,
    CONSTRAINT StartPosition_pk PRIMARY KEY ([SkierId], [StartListId]),
	CONSTRAINT StartPosition_Skier_fk FOREIGN KEY ([SkierId])
		REFERENCES [Hurace].[Skier] ([SkierId]),
	CONSTRAINT StartPosition_StartList_fk FOREIGN KEY ([StartListId])
		REFERENCES [Hurace].[StartList] ([StartListId])
);

CREATE TABLE [Hurace].[RaceType]
(
    [Label] VARCHAR(20) NOT NULL,
    CONSTRAINT RaceType_pk PRIMARY KEY ([Label])
);

CREATE TABLE [Hurace].[Race]
(
    [RaceId] INT NOT NULL IDENTITY,
    [RaceType] VARCHAR(20) NOT NULL,
    [Venue] VARCHAR(30) NOT NULL,
    [FirstStartListId] INT NULL,
    [SecondStartListId] INT NULL,
    [NumberOfSensors] INT NOT NULL,
    [Description] VARCHAR(280) NOT NULL,
    CONSTRAINT Race_pk PRIMARY KEY ([RaceId]),
	CONSTRAINT Race_RaceType_fk FOREIGN KEY ([RaceType])
		REFERENCES [Hurace].[RaceType] ([Label]),
	CONSTRAINT Race_Venue_fk  FOREIGN KEY ([Venue])
		REFERENCES [Hurace].[Venue] ([Name]),
	CONSTRAINT Race_StartList_1_fk FOREIGN KEY ([FirstStartListId])
		REFERENCES [Hurace].[StartList] ([StartListId]),
	CONSTRAINT Race_FirstSL_uq UNIQUE ([FirstStartListId]),
	CONSTRAINT Race_SecondSL_uq UNIQUE ([SecondStartListId]),
	CONSTRAINT Race_StartList_2_fk FOREIGN KEY ([SecondStartListId])
		REFERENCES [Hurace].[StartList] ([StartListId]),
	CONSTRAINT Race_NuOfSensors_gt_th_zero CHECK ([NumberOfSensors] > 0)
);

CREATE TABLE [Hurace].[RaceState]
(
    [Label] VARCHAR(10) NOT NULL,
    CONSTRAINT RaceState_pk PRIMARY KEY ([Label])
);

CREATE TABLE [Hurace].[RaceData]
(
    [RaceId] INT NOT NULL,
    [StartListId] INT NOT NULL,
    [SkierId] INT NOT NULL,
    [RaceState] VARCHAR(10) NOT NULL,
    CONSTRAINT RaceData_pk PRIMARY KEY ([RaceId], [StartListId], [SkierId]),
	CONSTRAINT RaceData_Race_fk FOREIGN KEY ([RaceId])
		REFERENCES [Hurace].[Race] ([RaceId]),
	CONSTRAINT RaceData_StartList_fk FOREIGN KEY ([StartListId])
		REFERENCES [Hurace].[StartList] ([StartListId]),
	CONSTRAINT RaceData_Skier_fk FOREIGN KEY ([SkierId])
		REFERENCES [Hurace].[Skier] ([SkierId]),
	CONSTRAINT RaceData_RaceState_fk FOREIGN KEY ([RaceState])
		REFERENCES [Hurace].[RaceState] ([Label])
);

CREATE TABLE [Hurace].[TimeMeasurement] (
	[TimeMeasurementId] INT NOT NULL IDENTITY,
    [RaceId] INT NOT NULL,
    [StartListId] INT NOT NULL,
    [SkierId] INT NOT NULL,
    [SensorId] INT NOT NULL,
    [Measurement] DATETIME NOT NULL,
	CONSTRAINT TimeMeasurement_pk PRIMARY KEY ([TimeMeasurementId]),
    CONSTRAINT TimeMeasurement_unique UNIQUE ([RaceId], [StartListId], [SkierId], [SensorId]),
	CONSTRAINT TimeMeasurement_RaceData_fk FOREIGN KEY ([RaceId], [StartListId], [SkierId])
		REFERENCES [Hurace].[RaceData] ([RaceId], [StartListId], [SkierId]),
	CONSTRAINT TimeMeasurement_sensorid_gt_or_eq_zero CHECK ([SensorId] >= 0)
);