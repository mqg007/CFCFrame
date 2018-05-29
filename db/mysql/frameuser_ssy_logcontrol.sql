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
-- Table structure for table `ssy_logcontrol`
--

DROP TABLE IF EXISTS `ssy_logcontrol`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ssy_logcontrol` (
  `logcontrolid` varchar(50) NOT NULL COMMENT 'ID',
  `domainiden` varchar(50) DEFAULT NULL COMMENT '日志配置类别',
  `optioniden` varchar(50) DEFAULT NULL COMMENT '日志配置等级',
  `isrecord` varchar(50) DEFAULT '1' COMMENT '是否记录 1 记录 0 不记录 默认记录1',
  `timestampss` varchar(50) DEFAULT NULL COMMENT '时间戳',
  PRIMARY KEY (`logcontrolid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='日志控制是否记录控制';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ssy_logcontrol`
--

LOCK TABLES `ssy_logcontrol` WRITE;
/*!40000 ALTER TABLE `ssy_logcontrol` DISABLE KEYS */;
INSERT INTO `ssy_logcontrol` VALUES ('1','UI','HighErr','1',''),('10','Biz','ExecptionErr','1',''),('11','Data','HighErr','1',''),('12','Data','NormalErr','1',''),('13','Data','Warning','1',''),('14','Data','Normal','1',''),('15','Data','ExecptionErr','1',''),('16','Sys','HighErr','1',''),('17','Sys','NormalErr','1',''),('18','Sys','Warning','1',''),('19','Sys','Normal','1',''),('2','UI','NormalErr','1',''),('20','Sys','ExecptionErr','1',''),('21','ExceptionErr','HighErr','1',''),('22','ExceptionErr','NormalErr','1',''),('23','ExceptionErr','Warning','1',''),('24','ExceptionErr','Normal','1',''),('25','ExceptionErr','ExecptionErr','1',''),('3','UI','Warning','1',''),('4','UI','Normal','1',''),('5','UI','ExecptionErr','1',''),('6','Biz','HighErr','1',''),('7','Biz','NormalErr','1',''),('8','Biz','Warning','1',''),('9','Biz','Normal','1','');
/*!40000 ALTER TABLE `ssy_logcontrol` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-05-29  9:50:47
