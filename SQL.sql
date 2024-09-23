IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'FarmDbEntities')
BEGIN
    CREATE DATABASE FarmDbEntities;
END

USE FarmDbEntities;


CREATE TABLE AnimalType (
    AnimalTypeId INT PRIMARY KEY IDENTITY(1,1),
    AnimalName VARCHAR(50) NOT NULL,
    Sound VARCHAR(50) NOT NULL
);

CREATE TABLE Animal (
    AnimalId INT PRIMARY KEY IDENTITY(1,1),
    AnimalTypeId INT FOREIGN KEY REFERENCES AnimalType(AnimalTypeId),
    MilkProduced FLOAT NOT NULL,
    OffspringCount INT NOT NULL
);

CREATE TABLE MilkStatistics (
    StatisticsId INT PRIMARY KEY IDENTITY(1,1),
    AnimalId INT FOREIGN KEY REFERENCES Animal(AnimalId),
    Date DATETIME NOT NULL,
    MilkProduced FLOAT NOT NULL
);

CREATE TABLE OffspringStatistics (
    StatisticsId INT PRIMARY KEY IDENTITY(1,1),
    AnimalId INT FOREIGN KEY REFERENCES Animal(AnimalId),
    Date DATETIME NOT NULL,
    OffspringCount INT NOT NULL
);

CREATE TABLE FarmStatistics (
    StatisticsId INT PRIMARY KEY IDENTITY(1,1),
    Date DATETIME NOT NULL,
    TotalMilk FLOAT NOT NULL,
    TotalOffspring INT NOT NULL
);

INSERT INTO AnimalType (AnimalName, Sound) VALUES ('Cow', 'Moo');
INSERT INTO AnimalType (AnimalName, Sound) VALUES ('Sheep', 'Baa');
INSERT INTO AnimalType (AnimalName, Sound) VALUES ('Goat', 'Bleat');

INSERT INTO Animal (AnimalTypeId, MilkProduced, OffspringCount) VALUES (1, 15.0, 1);
INSERT INTO Animal (AnimalTypeId, MilkProduced, OffspringCount) VALUES (2, 3.0, 2);
INSERT INTO Animal (AnimalTypeId, MilkProduced, OffspringCount) VALUES (3, 7.0, 3);

INSERT INTO MilkStatistics (AnimalId, Date, MilkProduced) VALUES (1, GETDATE(), 15.0);
INSERT INTO MilkStatistics (AnimalId, Date, MilkProduced) VALUES (2, GETDATE(), 3.0);
INSERT INTO MilkStatistics (AnimalId, Date, MilkProduced) VALUES (3, GETDATE(), 7.0);

INSERT INTO OffspringStatistics (AnimalId, Date, OffspringCount) VALUES (1, GETDATE(), 1);
INSERT INTO OffspringStatistics (AnimalId, Date, OffspringCount) VALUES (2, GETDATE(), 2);
INSERT INTO OffspringStatistics (AnimalId, Date, OffspringCount) VALUES (3, GETDATE(), 3);

INSERT INTO FarmStatistics (Date, TotalMilk, TotalOffspring) VALUES (GETDATE(), 25.0, 6);