CREATE DATABASE IF NOT EXISTS reminders;

DROP TABLE reminders.reminders;

CREATE TABLE IF NOT EXISTS reminders.reminders (
  ID int(10) unsigned NOT NULL AUTO_INCREMENT,
  UserID varchar(128) NOT NULL,
  Title varchar(64) NOT NULL,
  Description text,
  ForDate datetime NOT NULL,
  Created datetime NOT NULL,
  Recurrence tinyint(4) NOT NULL,
  Importance tinyint(4) NOT NULL,
  LastActioned datetime DEFAULT NULL,
  PRIMARY KEY (ID),
  KEY idxUserId (UserID)
) ENGINE=InnoDB;

GRANT SELECT, INSERT, UPDATE ON reminders.reminders TO `reminders`@`localhost`;
