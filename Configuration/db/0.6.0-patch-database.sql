-- Add the Status field to support archiving of reminders
USE reminders;
ALTER TABLE reminders
	ADD COLUMN Status TINYINT(4) NOT NULL DEFAULT 0 AFTER LastActioned,
	DROP INDEX idxUserId,
	ADD INDEX idxUserId (UserID, Status);
