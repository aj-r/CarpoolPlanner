ALTER TABLE `climbing`.`user` 
ADD COLUMN `time_zone` VARCHAR(45) NULL AFTER `last_text_message_id`;

UPDATE `climbing`.`user`
SET `time_zone` = 'America/New_York';

ALTER TABLE `climbing`.`trip` 
ADD COLUMN `time_zone` VARCHAR(45) NULL;

UPDATE `climbing`.`trip`
SET `time_zone` = 'America/New_York';