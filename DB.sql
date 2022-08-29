CREATE SCHEMA dbo;

CREATE TABLE BuildingTypes (
	Id int NOT NULL,
	Name nvarchar(MAX) COLLATE Cyrillic_General_CI_AS NOT NULL,
	Description nvarchar(MAX) COLLATE Cyrillic_General_CI_AS NOT NULL,
	InitionalCost int NOT NULL,
	InitionalCoins int NOT NULL,
	CONSTRAINT PK_BuildingTypes PRIMARY KEY (Id)
);



CREATE TABLE Buildings (
	Id int IDENTITY(1,1) NOT NULL,
	UserId int NOT NULL,
	BuildingTypeId int NOT NULL,
	X int NOT NULL,
	Y int NOT NULL,
	[Level] int NOT NULL,
	CONSTRAINT PK_Buildings PRIMARY KEY (Id)
);


CREATE TABLE Users (
	Id int IDENTITY(1,1) NOT NULL,
	Name nvarchar(MAX) COLLATE Cyrillic_General_CI_AS NOT NULL,
	Uid nvarchar(450) COLLATE Cyrillic_General_CI_AS NOT NULL,
	Coins bigint NOT NULL,
	CoinsPerSecond int NOT NULL,
	LastActive datetime2 NOT NULL,
	CONSTRAINT PK_Users PRIMARY KEY (Id)
);
 CREATE  UNIQUE NONCLUSTERED INDEX IX_Users_Uid ON dbo.Users (  Uid ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


CREATE TABLE [__EFMigrationsHistory] (
	MigrationId nvarchar(150) COLLATE Cyrillic_General_CI_AS NOT NULL,
	ProductVersion nvarchar(32) COLLATE Cyrillic_General_CI_AS NOT NULL,
	CONSTRAINT PK___EFMigrationsHistory PRIMARY KEY (MigrationId)
);


INSERT INTO [__EFMigrationsHistory]
(MigrationId, ProductVersion)
VALUES(N'20220811151732_Init', N'6.0.8');
