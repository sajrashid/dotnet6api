﻿
CREATE TABLE IF NOT EXISTS testdb.Users (
Id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
Email VARCHAR(256) NOT NULL,
Salt BINARY (16), Hash VARCHAR(65) NOT NULL,
LastVisit DATETIME,
INDEX (Email, Hash)
);

INSERT INTO testdb.Users (Email, Salt, Hash, LastVisit) VALUES
	('testUser@test.com', _binary 0x056d4070a37614c8f3e56194ea6ec8a5, 'uU+nNAfOxSUm3yox/nXJHMd/F41c1VMUDJisUp9oFMk=', '2021-10-20 17:58:57');

CREATE TABLE IF NOT EXISTS testdb.Roles (
Id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
UserId Int UNSIGNED NOT NULL, Role VARCHAR(16) NOT NULL, CONSTRAINT `FK_UserId`
FOREIGN KEY (`UserId`) REFERENCES testdb.Users(`Id`) ON DELETE CASCADE

);


CREATE TABLE IF NOT EXISTS testdb.Visitors (
	`Id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
	`Hash` VARCHAR(128) NOT NULL,
	`UserAgent` VARCHAR(256) NOT NULL,
	`IP` VARCHAR(30) NOT NULL,
	`CanvasId` VARCHAR(8) NULL DEFAULT NULL,
	`LastVisit` DATETIME NULL DEFAULT NULL,
	`Count` INT UNSIGNED NULL DEFAULT NULL,
	INDEX (Hash, IP)
);

CREATE TABLE IF NOT EXISTS testdb.Products (
Id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
Company VARCHAR(128) NOT NULL, Phone VARCHAR(40) NOT NULL,
 Price INT UNSIGNED NOT NULL, InStock BOOLEAN NOT NULL,
 StockCount INT UNSIGNED, NewStockDate  DATETIME);







