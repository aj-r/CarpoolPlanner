ALTER TABLE `user_trip_instance` 
CHANGE COLUMN `attending` `attending` BIT(1) NULL DEFAULT NULL,
ADD COLUMN `confirm_time` DATETIME NULL AFTER `attending`,
ADD COLUMN `no_room` BIT(1) NOT NULL DEFAULT b'0' AFTER `trip_id`;

ALTER TABLE `user_trip_instance` 
DROP FOREIGN KEY `fk_usertripinstance_usertrip`;
ALTER TABLE `user_trip_instance` 
ADD CONSTRAINT `fk_usertripinstance_usertrip`
  FOREIGN KEY (`user_id` , `trip_id`)
  REFERENCES `user_trip` (`user_id` , `trip_ID`)
  ON DELETE CASCADE
  ON UPDATE CASCADE;
  
UPDATE `user_trip_instance`
SET `attending` = NULL
WHERE `attending` = b'1';

ALTER TABLE `trip_instance` 
ADD COLUMN `drivers_picked` BIT(1) NOT NULL DEFAULT b'0' AFTER `skip`;

