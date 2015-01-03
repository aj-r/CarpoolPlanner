-- MySQL dump 10.13  Distrib 5.6.17, for Win64 (x86_64)
--
-- Host: localhost    Database: climbing
-- ------------------------------------------------------
-- Server version	5.6.21-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `trip`
--

DROP TABLE IF EXISTS `trip`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `trip` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `status` smallint(6) NOT NULL DEFAULT '0',
  `name` varchar(500) NOT NULL,
  `description` varchar(5000) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `trip`
--

LOCK TABLES `trip` WRITE;
/*!40000 ALTER TABLE `trip` DISABLE KEYS */;
/*!40000 ALTER TABLE `trip` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `trip_instance`
--

DROP TABLE IF EXISTS `trip_instance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `trip_instance` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `trip_id` bigint(20) NOT NULL,
  `date` datetime NOT NULL,
  `skip` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`id`),
  KEY `trip_id_idx` (`trip_id`),
  CONSTRAINT `fk_tripinstance_trip` FOREIGN KEY (`trip_id`) REFERENCES `trip` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `trip_instance`
--

LOCK TABLES `trip_instance` WRITE;
/*!40000 ALTER TABLE `trip_instance` DISABLE KEYS */;
/*!40000 ALTER TABLE `trip_instance` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `trip_recurrence`
--

DROP TABLE IF EXISTS `trip_recurrence`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `trip_recurrence` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `trip_id` bigint(20) NOT NULL,
  `every` smallint(6) NOT NULL DEFAULT '1',
  `start` datetime NOT NULL,
  `end` datetime DEFAULT NULL,
  `type` smallint(6) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_triprecurrence_trip_idx` (`trip_id`),
  CONSTRAINT `fk_triprecurrence_trip` FOREIGN KEY (`trip_id`) REFERENCES `trip` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `trip_recurrence`
--

LOCK TABLES `trip_recurrence` WRITE;
/*!40000 ALTER TABLE `trip_recurrence` DISABLE KEYS */;
/*!40000 ALTER TABLE `trip_recurrence` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user` (
  `id` varchar(45) NOT NULL,
  `email` varchar(500) DEFAULT NULL,
  `phone` varchar(45) DEFAULT NULL,
  `password` binary(24) DEFAULT NULL,
  `status` smallint(6) NOT NULL DEFAULT '0',
  `is_driver` bit(1) NOT NULL DEFAULT b'0',
  `seats` smallint(6) NOT NULL DEFAULT '0',
  `salt` binary(24) DEFAULT NULL,
  `iterations` int(11) NOT NULL DEFAULT '0',
  `is_admin` bit(1) NOT NULL DEFAULT b'0',
  `name` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES ('Admin','aj.ric.ar@gmail.com','226-868-3049','ÖÜ\'|OÐq¸Ú6%’Â°KúD:5\ZÀ',0,'\0',0,'sÎ’m©Øýpgæù‰«þ4æóßÄï\\¹–',1000,'','AJ');
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_trip`
--

DROP TABLE IF EXISTS `user_trip`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user_trip` (
  `user_id` varchar(45) NOT NULL,
  `trip_ID` bigint(20) NOT NULL,
  `is_driver` bit(1) NOT NULL DEFAULT b'0',
  `seats` smallint(6) NOT NULL DEFAULT '0',
  PRIMARY KEY (`user_id`,`trip_ID`),
  KEY `fk_usertrip_trip_idx` (`trip_ID`),
  CONSTRAINT `fk_usertrip_trip` FOREIGN KEY (`trip_ID`) REFERENCES `trip` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_usertrip_user` FOREIGN KEY (`user_id`) REFERENCES `user` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_trip`
--

LOCK TABLES `user_trip` WRITE;
/*!40000 ALTER TABLE `user_trip` DISABLE KEYS */;
/*!40000 ALTER TABLE `user_trip` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_trip_instance`
--

DROP TABLE IF EXISTS `user_trip_instance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user_trip_instance` (
  `user_id` varchar(45) NOT NULL,
  `trip_instance_id` bigint(20) NOT NULL,
  `is_driver` bit(1) NOT NULL DEFAULT b'0',
  `seats` smallint(6) NOT NULL DEFAULT '0',
  `attending` bit(1) NOT NULL DEFAULT b'1',
  `note` varchar(5000) DEFAULT NULL,
  `trip_id` bigint(20) NOT NULL,
  PRIMARY KEY (`user_id`,`trip_instance_id`),
  KEY `fk_usertripinstance_tripinstance_idx` (`trip_instance_id`),
  KEY `fk_usertripinstance_usertrip_idx` (`user_id`,`trip_id`),
  CONSTRAINT `fk_usertripinstance_tripinstance` FOREIGN KEY (`trip_instance_id`) REFERENCES `trip_instance` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_usertripinstance_user` FOREIGN KEY (`user_id`) REFERENCES `user` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_usertripinstance_usertrip` FOREIGN KEY (`user_id`, `trip_id`) REFERENCES `user_trip` (`user_id`, `trip_ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_trip_instance`
--

LOCK TABLES `user_trip_instance` WRITE;
/*!40000 ALTER TABLE `user_trip_instance` DISABLE KEYS */;
/*!40000 ALTER TABLE `user_trip_instance` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_trip_recurrence`
--

DROP TABLE IF EXISTS `user_trip_recurrence`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user_trip_recurrence` (
  `user_id` varchar(45) NOT NULL,
  `trip_recurrence_id` bigint(20) NOT NULL,
  `is_driver` bit(1) NOT NULL DEFAULT b'0',
  `seats` smallint(6) NOT NULL DEFAULT '0',
  `trip_id` bigint(20) NOT NULL,
  PRIMARY KEY (`user_id`,`trip_recurrence_id`),
  KEY `fk_usertriprecurrence_triprecurrence_idx` (`trip_recurrence_id`),
  KEY `fk_usertriprecurrence_usertrip_idx` (`user_id`,`trip_id`),
  CONSTRAINT `fk_usertriprecurrence_triprecurrence` FOREIGN KEY (`trip_recurrence_id`) REFERENCES `trip_recurrence` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_usertriprecurrence_user` FOREIGN KEY (`user_id`) REFERENCES `user` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_usertriprecurrence_usertrip` FOREIGN KEY (`user_id`, `trip_id`) REFERENCES `user_trip` (`user_id`, `trip_ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_trip_recurrence`
--

LOCK TABLES `user_trip_recurrence` WRITE;
/*!40000 ALTER TABLE `user_trip_recurrence` DISABLE KEYS */;
/*!40000 ALTER TABLE `user_trip_recurrence` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2014-11-22 19:23:53
