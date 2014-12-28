ALTER TABLE `user_trip` 
ADD COLUMN `attending` BIT(1) NOT NULL DEFAULT b'0' AFTER `trip_ID`;

UPDATE `user_trip`
SET `attending` = b'1';

ALTER TABLE `user_trip_recurrence` 
ADD COLUMN `attending` BIT(1) NOT NULL DEFAULT b'0' AFTER `trip_id`;

UPDATE `user_trip_recurrence`
SET `attending` = b'1';