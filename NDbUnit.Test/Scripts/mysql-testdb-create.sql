DROP DATABASE IF EXISTS `testdb`;
CREATE DATABASE `testdb`;

SET FOREIGN_KEY_CHECKS = 0;

DROP TABLE IF EXISTS `testdb`.`Role`;
CREATE TABLE  `testdb`.`Role` (
  `ID` bigint(20) NOT NULL auto_increment,
  `Name` varchar(45) default NULL,
  `Description` varchar(45) default NULL,
  PRIMARY KEY  (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `testdb`.`User`;
CREATE TABLE  `testdb`.`User` (
  `ID` bigint(20) NOT NULL auto_increment,
  `FirstName` varchar(45) default NULL,
  `LastName` varchar(45) default NULL,
  `Age` bigint(20) unsigned default NULL,
  `SupervisorID` bigint(20) default NULL,
  PRIMARY KEY  (`ID`),
  KEY `FK_User_supervisor` (`SupervisorID`),
  CONSTRAINT `FK_User_supervisor` FOREIGN KEY (`SupervisorID`) REFERENCES `User` (`ID`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `testdb`.`UserRole`;
CREATE TABLE  `testdb`.`UserRole` (
  `UserID` bigint(20) NOT NULL,
  `RoleID` bigint(20) NOT NULL,
  PRIMARY KEY  (`UserID`,`RoleID`),
  KEY `FK_UserRole_Role` (`RoleID`),
  CONSTRAINT `FK_UserRole_Role` FOREIGN KEY (`RoleID`) REFERENCES `Role` (`ID`),
  CONSTRAINT `FK_UserRole_User` FOREIGN KEY (`UserID`) REFERENCES `User` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `testdb`.`DateAsPrimaryKey`;
CREATE TABLE  `testdb`.`DateAsPrimaryKey` (
  `PrimaryKey` date NOT NULL,
  PRIMARY KEY  (`PrimaryKey`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

SET FOREIGN_KEY_CHECKS = 1;