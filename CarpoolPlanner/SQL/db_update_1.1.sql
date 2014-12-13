ALTER TABLE `climbing`.`user` 
CHANGE COLUMN `is_driver` `commute_method` SMALLINT(6) NOT NULL DEFAULT '0',
ADD COLUMN `can_drive_if_needed` bit(1) NOT NULL DEFAULT b'0' AFTER `commute_method`,
ADD COLUMN `email_notify` BIT(1) NOT NULL DEFAULT b'0' AFTER `email`,
ADD COLUMN `email_visible` BIT(1) NOT NULL DEFAULT b'1' AFTER `email_notify`,
ADD COLUMN `phone_notify` BIT(1) NOT NULL DEFAULT b'1' AFTER `phone`,
ADD COLUMN `phone_visible` BIT(1) NOT NULL DEFAULT b'1' AFTER `phone_notify`,
ADD COLUMN `last_text_message_id` BIGINT(20) NULL AFTER `name`;

ALTER TABLE `climbing`.`trip` 
ADD COLUMN `location` varchar(5000) NULL AFTER `name`;

ALTER TABLE `climbing`.`user_trip` 
DROP COLUMN `seats`,
DROP COLUMN `is_driver`;

ALTER TABLE `climbing`.`user_trip_recurrence` 
DROP COLUMN `seats`,
DROP COLUMN `is_driver`;

ALTER TABLE `climbing`.`user_trip_instance` 
DROP FOREIGN KEY `fk_usertripinstance_usertrip`;
ALTER TABLE `climbing`.`user_trip_instance` 
ADD CONSTRAINT `fk_usertripinstance_usertrip`
  FOREIGN KEY (`user_id` , `trip_id`)
  REFERENCES `climbing`.`user_trip` (`user_id` , `trip_ID`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;
  
ALTER TABLE `climbing`.`user_trip_instance` 
CHANGE COLUMN `is_driver` `commute_method` SMALLINT(6) NOT NULL DEFAULT '0',
ADD COLUMN `can_drive_if_needed` bit(1) NOT NULL DEFAULT b'0' AFTER `commute_method`;

CREATE TABLE `climbing`.`log` (
  `id` BIGINT NOT NULL AUTO_INCREMENT,
  `level` VARCHAR(10) NOT NULL,
  `message` VARCHAR(5000) NOT NULL,
  `logger` VARCHAR(500) NULL,
  `user_id` VARCHAR(45) NULL DEFAULT NULL,
  `date` DATETIME NOT NULL,
  `ndc` VARCHAR(500) NULL,
  PRIMARY KEY (`id`));