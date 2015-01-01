ALTER TABLE `user_trip` 
ADD COLUMN `attending` BIT(1) NOT NULL DEFAULT b'0' AFTER `trip_ID`;

UPDATE `user_trip`
SET `attending` = b'1';

ALTER TABLE `user_trip_recurrence` 
ADD COLUMN `attending` BIT(1) NOT NULL DEFAULT b'0' AFTER `trip_id`;

UPDATE `user_trip_recurrence`
SET `attending` = b'1';

ALTER TABLE `user_trip` 
DROP FOREIGN KEY `fk_usertrip_user`;
ALTER TABLE `user_trip_instance` 
DROP FOREIGN KEY `fk_usertripinstance_user`;
ALTER TABLE `user_trip_recurrence` 
DROP FOREIGN KEY `fk_usertriprecurrence_user`;

ALTER TABLE `user` 
DROP PRIMARY KEY,
CHANGE COLUMN `id` `login_name` VARCHAR(45) NOT NULL;
ALTER TABLE `user` 
ADD COLUMN `id` BIGINT(20) NOT NULL FIRST;
ALTER TABLE `user` 
ADD PRIMARY KEY (`id`),
ADD UNIQUE INDEX `login_name_UNIQUE` (`login_name` ASC);
ALTER TABLE `user` 
CHANGE COLUMN `id` `id` BIGINT(20) NOT NULL AUTO_INCREMENT;

ALTER TABLE `user_trip_instance` 
DROP FOREIGN KEY `fk_usertripinstance_usertrip`;
ALTER TABLE `user_trip_instance` 
DROP INDEX `fk_usertripinstance_usertrip_idx` ;
ALTER TABLE `user_trip_recurrence` 
DROP FOREIGN KEY `fk_usertriprecurrence_usertrip`;
ALTER TABLE `user_trip_recurrence` 
DROP INDEX `fk_usertriprecurrence_usertrip_idx` ;

ALTER TABLE `user_trip`
DROP PRIMARY KEY,
CHANGE COLUMN `user_id` `login_name` VARCHAR(45) NOT NULL;
ALTER TABLE `user_trip`
ADD COLUMN `user_id` BIGINT(20) NULL FIRST;
UPDATE `user_trip` ut
  JOIN `user` u ON ut.login_name = u.login_name
SET ut.user_id = u.id;
ALTER TABLE `user_trip`
DROP COLUMN `login_name`;
ALTER TABLE `user_trip` 
CHANGE COLUMN `user_id` `user_id` BIGINT(20) NOT NULL ,
ADD PRIMARY KEY (`trip_ID`, `user_id`);
ALTER TABLE `user_trip` 
ADD INDEX `fk_usertrip_user_idx` (`user_id` ASC);
ALTER TABLE `user_trip` 
ADD CONSTRAINT `fk_usertrip_user`
  FOREIGN KEY (`user_id`)
  REFERENCES `user` (`id`)
  ON DELETE CASCADE
  ON UPDATE CASCADE;
  
ALTER TABLE `user_trip_instance`
DROP PRIMARY KEY,
CHANGE COLUMN `user_id` `login_name` VARCHAR(45) NOT NULL;
ALTER TABLE `user_trip_instance`
ADD COLUMN `user_id` BIGINT(20) NULL FIRST;
UPDATE `user_trip_instance` uti
  JOIN `user` u ON uti.login_name = u.login_name
SET uti.user_id = u.id;
ALTER TABLE `user_trip_instance`
DROP COLUMN `login_name`;
ALTER TABLE `user_trip_instance` 
CHANGE COLUMN `user_id` `user_id` BIGINT(20) NOT NULL ,
ADD PRIMARY KEY (`trip_instance_id`, `user_id`);
ALTER TABLE `user_trip_instance` 
ADD INDEX `fk_usertripinstance_usertrip_idx` (`user_id` ASC, `trip_id` ASC);
ALTER TABLE `user_trip_instance` 
ADD CONSTRAINT `fk_usertripinstance_usertrip`
  FOREIGN KEY (`user_id` , `trip_id`)
  REFERENCES `user_trip` (`user_id` , `trip_ID`)
  ON DELETE CASCADE
  ON UPDATE CASCADE,
ADD CONSTRAINT `fk_usertripinstance_user`
  FOREIGN KEY (`user_id`)
  REFERENCES `user` (`id`)
  ON DELETE CASCADE
  ON UPDATE CASCADE;

ALTER TABLE `user_trip_recurrence`
DROP PRIMARY KEY,
CHANGE COLUMN `user_id` `login_name` VARCHAR(45) NOT NULL;
ALTER TABLE `user_trip_recurrence`
ADD COLUMN `user_id` BIGINT(20) NULL FIRST;
UPDATE `user_trip_recurrence` utr
  JOIN `user` u ON utr.login_name = u.login_name
SET utr.user_id = u.id;
ALTER TABLE `user_trip_recurrence`
DROP COLUMN `login_name`;
ALTER TABLE `user_trip_recurrence` 
CHANGE COLUMN `user_id` `user_id` BIGINT(20) NOT NULL ,
ADD PRIMARY KEY (`trip_recurrence_id`, `user_id`);
ALTER TABLE `user_trip_recurrence` 
ADD INDEX `fk_usertriprecurrence_usertrip_idx` (`user_id` ASC, `trip_id` ASC);
ALTER TABLE `user_trip_recurrence` 
ADD CONSTRAINT `fk_usertriprecurrence_usertrip`
  FOREIGN KEY (`user_id` , `trip_id`)
  REFERENCES `user_trip` (`user_id` , `trip_ID`)
  ON DELETE CASCADE
  ON UPDATE CASCADE,
ADD CONSTRAINT `fk_usertriprecurrence_user`
  FOREIGN KEY (`user_id`)
  REFERENCES `user` (`id`)
  ON DELETE CASCADE
  ON UPDATE CASCADE;

ALTER TABLE `log`
CHANGE COLUMN `user_id` `login_name` VARCHAR(45) NULL;
ALTER TABLE `log`
ADD COLUMN `user_id` BIGINT(20) NULL FIRST;
UPDATE `log` l
  JOIN `user` u ON l.login_name = u.login_name
SET l.user_id = u.id;
ALTER TABLE `log`
DROP COLUMN `login_name`;