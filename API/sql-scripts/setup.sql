CREATE TABLE Usersdb.Users (
Id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
UserAgent VARCHAR(256) NOT NULL,IP VARCHAR(30) NOT NULL,
CanvasId VARCHAR(8),
LastVisit DATETIME, Count INT);