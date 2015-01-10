ALTER TABLE `user` 
CHANGE COLUMN `email` `email` VARCHAR(200) NULL ;

UPDATE `user`
SET `email` = NULL
WHERE `email` = '';

UPDATE `user`
SET `email` = `login_name`
WHERE `email` IS NULL;

UPDATE `user`
SET `phone` = NULL
WHERE `phone` = '';

ALTER TABLE `user` 
ADD UNIQUE INDEX `email_UNIQUE` (`email` ASC),
CHANGE COLUMN `email` `email` VARCHAR(200) NOT NULL,
CHANGE COLUMN `login_name` `login_name` VARCHAR(45) NULL,
DROP INDEX `login_name_UNIQUE`;
