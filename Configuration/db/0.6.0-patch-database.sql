USE reminders;

-- Add the Status field to support archiving of reminders

ALTER TABLE reminders
	ADD COLUMN Status TINYINT(4) NOT NULL DEFAULT 0 AFTER LastActioned,
	DROP INDEX idxUserId,
	ADD INDEX idxUserId (UserID, Status);

-- Add the preferences table to support user settings

CREATE TABLE IF NOT EXISTS preferences (
	ID				INT UNSIGNED NOT NULL AUTO_INCREMENT,
	UserID			VARCHAR(128) NOT NULL,
	SoonDays		SMALLINT UNSIGNED NOT NULL,
	ImminentDays	SMALLINT UNSIGNED NOT NULL,
	PRIMARY KEY		(ID),
	UNIQUE INDEX	idxUserID (UserID)
);

GRANT SELECT, INSERT, UPDATE ON reminders.preferences TO 'reminders'@'localhost';