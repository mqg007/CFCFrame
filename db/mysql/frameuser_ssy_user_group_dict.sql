-- MySQL dump 10.13  Distrib 5.7.12, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: frameuser
-- ------------------------------------------------------
-- Server version	5.7.17-log

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
-- Table structure for table `ssy_user_group_dict`
--

DROP TABLE IF EXISTS `ssy_user_group_dict`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ssy_user_group_dict` (
  `userid` varchar(50) NOT NULL COMMENT '角色',
  `groupid` varchar(50) NOT NULL COMMENT '用户',
  `timestampss` varchar(50) DEFAULT NULL COMMENT '时间戳',
  KEY `FK_SSY_USER_DIC_idx` (`userid`),
  KEY `FK_SSY_GROUP_DICT` (`groupid`),
  CONSTRAINT `FK_SSY_GROUP_DICT` FOREIGN KEY (`groupid`) REFERENCES `ssy_group_dict` (`groupid`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `FK_SSY_USER_DIC` FOREIGN KEY (`userid`) REFERENCES `ssy_user_dict` (`userid`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='用户和组';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ssy_user_group_dict`
--

LOCK TABLES `ssy_user_group_dict` WRITE;
/*!40000 ALTER TABLE `ssy_user_group_dict` DISABLE KEYS */;
INSERT INTO `ssy_user_group_dict` VALUES ('test6','grp001','2017-09-05 14:06:37'),('mqg','grp001','2016-03-07 19:49:21'),('u1n','grp002','2018-01-16 15:44:47');
/*!40000 ALTER TABLE `ssy_user_group_dict` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-05-29  9:50:50
