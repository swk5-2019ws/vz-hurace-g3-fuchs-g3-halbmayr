-- ****************** SqlDBM: Microsoft SQL Server ******************
-- ******************************************************************

CREATE SCHEMA [Hurace];
GO

-- ****************** SqlDBM: Microsoft SQL Server ******************
-- ******************************************************************

-- ************************************** [Hurace].[StartList]

CREATE TABLE [Hurace].[StartList]
(
 [Id] int IDENTITY (1, 1) NOT NULL ,


 CONSTRAINT [StartList_pk] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);
GO








-- ************************************** [Hurace].[Sex]

CREATE TABLE [Hurace].[Sex]
(
 [Label] varchar(25) NOT NULL ,
 [Id]    int IDENTITY (1, 1) NOT NULL ,


 CONSTRAINT [Sex_pk] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);
GO








-- ************************************** [Hurace].[Season]

CREATE TABLE [Hurace].[Season]
(
 [Id]        int IDENTITY (1, 1) NOT NULL ,
 [Name]      varchar(30) NOT NULL ,
 [StartDate] date NOT NULL ,
 [EndDate]   date NOT NULL ,


 CONSTRAINT [Season_pk] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);
GO








-- ************************************** [Hurace].[RaceType]

CREATE TABLE [Hurace].[RaceType]
(
 [Id]    int IDENTITY (1, 1) NOT NULL ,
 [Label] varchar(20) NOT NULL ,


 CONSTRAINT [RaceType_pk] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);
GO








-- ************************************** [Hurace].[RaceState]

CREATE TABLE [Hurace].[RaceState]
(
 [Label] varchar(20) NOT NULL ,
 [Id]    int IDENTITY (1, 1) NOT NULL ,


 CONSTRAINT [RaceState_pk] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);
GO








-- ************************************** [Hurace].[Image]

CREATE TABLE [Hurace].[Image]
(
 [Id]      int IDENTITY (1, 1) NOT NULL ,
 [Content] varbinary(max) NOT NULL ,


 CONSTRAINT [Image_pk] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);
GO








-- ************************************** [Hurace].[Country]

CREATE TABLE [Hurace].[Country]
(
 [Id]   int IDENTITY (1, 1) NOT NULL ,
 [Name] varchar(50) NOT NULL ,


 CONSTRAINT [Country_pk] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);
GO








-- ************************************** [Hurace].[Venue]

CREATE TABLE [Hurace].[Venue]
(
 [Id]        int IDENTITY (1, 1) NOT NULL ,
 [Name]      varchar(30) NOT NULL ,
 [CountryId] int NOT NULL ,


 CONSTRAINT [Venue_pk] PRIMARY KEY NONCLUSTERED ([Id] ASC),
 CONSTRAINT [FK_132] FOREIGN KEY ([CountryId])  REFERENCES [Hurace].[Country]([Id])
);
GO


CREATE NONCLUSTERED INDEX [fkIdx_132] ON [Hurace].[Venue] 
 (
  [CountryId] ASC
 )

GO







-- ************************************** [Hurace].[Skier]

CREATE TABLE [Hurace].[Skier]
(
 [Id]          int IDENTITY (1, 1) NOT NULL ,
 [FirstName]   varchar(50) NOT NULL ,
 [LastName]    varchar(50) NOT NULL ,
 [DateOfBirth] date NOT NULL ,
 [CountryId]   int NOT NULL ,
 [SexId]       int NOT NULL ,
 [ImageId]     int NOT NULL ,


 CONSTRAINT [Skier_pk] PRIMARY KEY NONCLUSTERED ([Id] ASC),
 CONSTRAINT [Skier_ImageId_uq] UNIQUE NONCLUSTERED ([ImageId] ASC),
 CONSTRAINT [Skier_Country_fk] FOREIGN KEY ([CountryId])  REFERENCES [Hurace].[Country]([Id]),
 CONSTRAINT [Skier_Image_fk] FOREIGN KEY ([ImageId])  REFERENCES [Hurace].[Image]([Id]),
 CONSTRAINT [Skier_Sex_fk] FOREIGN KEY ([SexId])  REFERENCES [Hurace].[Sex]([Id])
);
GO








-- ************************************** [Hurace].[StartPosition]

CREATE TABLE [Hurace].[StartPosition]
(
 [Id]          int IDENTITY (1, 1) NOT NULL ,
 [SkierId]     int NOT NULL ,
 [StartListId] int NOT NULL ,
 [Position]    int NOT NULL ,


 CONSTRAINT [StartPosition_pk] PRIMARY KEY NONCLUSTERED ([Id] ASC),
 CONSTRAINT [SkierId_StartListId_uq] UNIQUE NONCLUSTERED ([SkierId] ASC, [StartListId] ASC),
 CONSTRAINT [StartPosition_Skier_fk] FOREIGN KEY ([SkierId])  REFERENCES [Hurace].[Skier]([Id]),
 CONSTRAINT [StartPosition_StartList_fk] FOREIGN KEY ([StartListId])  REFERENCES [Hurace].[StartList]([Id])
);
GO








-- ************************************** [Hurace].[SeasonPlan]

CREATE TABLE [Hurace].[SeasonPlan]
(
 [VenueId]  int NOT NULL ,
 [Id]       int IDENTITY (1, 1) NOT NULL ,
 [SeasonId] int NOT NULL ,


 CONSTRAINT [SeasonPlan_pk] PRIMARY KEY NONCLUSTERED ([Id] ASC),
 CONSTRAINT [SeasonId_VenueId_uq] UNIQUE NONCLUSTERED ([SeasonId] ASC, [VenueId] ASC),
 CONSTRAINT [SeasonPlan_Season_fk] FOREIGN KEY ([SeasonId])  REFERENCES [Hurace].[Season]([Id]),
 CONSTRAINT [SeasonPlan_Venue_fk] FOREIGN KEY ([VenueId])  REFERENCES [Hurace].[Venue]([Id])
);
GO








-- ************************************** [Hurace].[Race]

CREATE TABLE [Hurace].[Race]
(
 [Id]                int IDENTITY (1, 1) NOT NULL ,
 [RaceTypeId]        int NOT NULL ,
 [FirstStartListId]  int NULL ,
 [SecondStartListId] int NULL ,
 [NumberOfSensors]   int NOT NULL ,
 [Description]       varchar(280) NOT NULL ,
 [VenueId]           int NOT NULL ,
 [Date]              date NOT NULL ,


 CONSTRAINT [Race_pk] PRIMARY KEY NONCLUSTERED ([Id] ASC),
 CONSTRAINT [Race_FirstSL_uq] UNIQUE NONCLUSTERED ([FirstStartListId] ASC),
 CONSTRAINT [Race_SecondSL_uq] UNIQUE NONCLUSTERED ([SecondStartListId] ASC),
 CONSTRAINT [Race_RaceType_fk] FOREIGN KEY ([RaceTypeId])  REFERENCES [Hurace].[RaceType]([Id]),
 CONSTRAINT [Race_StartList_1_fk] FOREIGN KEY ([FirstStartListId])  REFERENCES [Hurace].[StartList]([Id]),
 CONSTRAINT [Race_StartList_2_fk] FOREIGN KEY ([SecondStartListId])  REFERENCES [Hurace].[StartList]([Id]),
 CONSTRAINT [Race_Venue_fk] FOREIGN KEY ([VenueId])  REFERENCES [Hurace].[Venue]([Id]),
 CONSTRAINT [Race_NuOfSensors_gt_th_zero] CHECK ( [NumberOfSensors] > 0 )
);
GO








-- ************************************** [Hurace].[RaceData]

CREATE TABLE [Hurace].[RaceData]
(
 [RaceId]      int NOT NULL ,
 [Id]          int IDENTITY (1, 1) NOT NULL ,
 [StartListId] int NOT NULL ,
 [SkierId]     int NOT NULL ,
 [RaceStateId] int NOT NULL ,


 CONSTRAINT [RaceData_pk] PRIMARY KEY NONCLUSTERED ([Id] ASC),
 CONSTRAINT [RaceData_Race_fk] FOREIGN KEY ([RaceId])  REFERENCES [Hurace].[Race]([Id]),
 CONSTRAINT [RaceData_RaceState_fk] FOREIGN KEY ([RaceStateId])  REFERENCES [Hurace].[RaceState]([Id]),
 CONSTRAINT [RaceData_Skier_fk] FOREIGN KEY ([SkierId])  REFERENCES [Hurace].[Skier]([Id]),
 CONSTRAINT [RaceData_StartList_fk] FOREIGN KEY ([StartListId])  REFERENCES [Hurace].[StartList]([Id])
);
GO








-- ************************************** [Hurace].[TimeMeasurement]

CREATE TABLE [Hurace].[TimeMeasurement]
(
 [Id]          int IDENTITY (1, 1) NOT NULL ,
 [SensorId]    int NOT NULL ,
 [Measurement] int NOT NULL ,
 [Id_1]        int NOT NULL ,


 CONSTRAINT [TimeMeasurement_pk] PRIMARY KEY NONCLUSTERED ([Id] ASC),
 CONSTRAINT [TimeMeasurement_RaceData_fk] FOREIGN KEY ([Id_1])  REFERENCES [Hurace].[RaceData]([Id]),
 CONSTRAINT [TimeMeasurement_sensorid_gt_or_eq_zero] CHECK ( [SensorId] >= 0 )
);
GO

