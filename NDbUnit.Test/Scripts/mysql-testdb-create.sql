DROP DATABASE IF EXISTS `testdb`;
CREATE DATABASE `testdb`;

SET FOREIGN_KEY_CHECKS = 0;

DROP TABLE IF EXISTS `testdb`.`role`;
CREATE TABLE  `testdb`.`role` (
  `ID` bigint(20) NOT NULL auto_increment,
  `Name` varchar(45) default NULL,
  `Description` varchar(45) default NULL,
  PRIMARY KEY  (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `testdb`.`user`;
CREATE TABLE  `testdb`.`user` (
  `ID` bigint(20) NOT NULL auto_increment,
  `FirstName` varchar(45) default NULL,
  `LastName` varchar(45) default NULL,
  `Age` bigint(20) unsigned default NULL,
  `SupervisorID` bigint(20) default NULL,
  PRIMARY KEY  (`ID`),
  KEY `FK_user_supervisor` (`SupervisorID`),
  CONSTRAINT `FK_user_supervisor` FOREIGN KEY (`SupervisorID`) REFERENCES `user` (`ID`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `testdb`.`userrole`;
CREATE TABLE  `testdb`.`userrole` (
  `UserID` bigint(20) NOT NULL,
  `RoleID` bigint(20) NOT NULL,
  PRIMARY KEY  (`UserID`,`RoleID`),
  KEY `FK_userrole_role` (`RoleID`),
  CONSTRAINT `FK_userrole_role` FOREIGN KEY (`RoleID`) REFERENCES `role` (`ID`),
  CONSTRAINT `FK_userrole_user` FOREIGN KEY (`UserID`) REFERENCES `user` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

SET FOREIGN_KEY_CHECKS = 1;