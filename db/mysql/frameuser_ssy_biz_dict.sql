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
-- Table structure for table `ssy_biz_dict`
--

DROP TABLE IF EXISTS `ssy_biz_dict`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ssy_biz_dict` (
  `ssy_biz_dictid` varchar(50) NOT NULL COMMENT 'ID',
  `domainnameiden` varchar(50) NOT NULL COMMENT '数据域配置识别',
  `domainnames` varchar(200) DEFAULT NULL COMMENT '数据域配置名称',
  `optioniden` varchar(50) NOT NULL COMMENT '数据域配置项识别',
  `optionnames` varchar(200) DEFAULT NULL COMMENT '数据域配置项名称',
  `optioniden_cut` varchar(20) DEFAULT NULL COMMENT '数据域配置项识别简称',
  `optionnames_cut` varchar(50) DEFAULT NULL COMMENT '数据域配置项名称简称',
  `pym` varchar(50) DEFAULT NULL COMMENT '拼音助记码',
  `remarks` varchar(200) DEFAULT NULL COMMENT '备注',
  `timestampss` varchar(50) DEFAULT NULL COMMENT '时间戳',
  PRIMARY KEY (`ssy_biz_dictid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='业务通用字典管理';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ssy_biz_dict`
--

LOCK TABLES `ssy_biz_dict` WRITE;
/*!40000 ALTER TABLE `ssy_biz_dict` DISABLE KEYS */;
INSERT INTO `ssy_biz_dict` VALUES ('0201709081131296011','biz1','业务字典1','b3','业务项3','bt3','btt3','4563','test3','2017-09-08 11:31:29'),('0201709081135528699','biz1','业务字典1','b4','业务项4','bt4','btt4','4564','test4','2017-09-08 11:35:52'),('0201709081454433328','biz1','业务字典1','b1','业务项1','bt2','btt22','456','test','2017-09-08 14:54:43'),('020170908150645160','biz1','业务字典1','b8','业务项8','bt2','btt22','456','test','2017-09-08 15:06:45'),('0201801151037484510','biz1','业务字典1','b9','业务项9','bt9','btt229','4569','test9','2018-01-15 10:37:48');
/*!40000 ALTER TABLE `ssy_biz_dict` ENABLE KEYS */;
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
