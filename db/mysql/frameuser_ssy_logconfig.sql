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
-- Table structure for table `ssy_logconfig`
--

DROP TABLE IF EXISTS `ssy_logconfig`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ssy_logconfig` (
  `logconfigid` varchar(50) NOT NULL COMMENT 'ID',
  `domainiden` varchar(50) DEFAULT NULL COMMENT '日志配置域识别',
  `domainname` varchar(50) DEFAULT NULL COMMENT '日志配置域名称',
  `optioniden` varchar(50) DEFAULT NULL COMMENT '日志配置项识别',
  `optionname` varchar(50) DEFAULT NULL COMMENT '日志配置项名称',
  `remarks` varchar(100) DEFAULT NULL COMMENT '备注',
  `timestampss` varchar(50) DEFAULT NULL COMMENT '时间戳',
  PRIMARY KEY (`logconfigid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='日志配置';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ssy_logconfig`
--

LOCK TABLES `ssy_logconfig` WRITE;
/*!40000 ALTER TABLE `ssy_logconfig` DISABLE KEYS */;
INSERT INTO `ssy_logconfig` VALUES ('1','LogTypeDomain','日志类别','UI','界面','',''),('10','LogLevelOption','日志等级','ExecptionErr','异常','',''),('2','LogTypeDomain','日志类别','Biz','业务','',''),('3','LogTypeDomain','日志类别','Data','数据','',''),('4','LogTypeDomain','日志类别','Sys','系统','',''),('5','LogTypeDomain','日志类别','ExceptionErr','异常','',''),('6','LogLevelOption','日志等级','HighErr','严重错误','',''),('7','LogLevelOption','日志等级','NormalErr','普通错误','',''),('8','LogLevelOption','日志等级','Warning','警告','',''),('9','LogLevelOption','日志等级','Normal','正常','','');
/*!40000 ALTER TABLE `ssy_logconfig` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-05-29  9:50:48
