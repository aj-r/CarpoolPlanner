ALTER TABLE `user` 
ADD COLUMN `time_zone` VARCHAR(45) NULL AFTER `last_text_message_id`;

UPDATE `user`
SET `time_zone` = 'America/New_York';

ALTER TABLE `trip` 
ADD COLUMN `time_zone` VARCHAR(45) NULL;

UPDATE `trip`
SET `time_zone` = 'America/New_York';