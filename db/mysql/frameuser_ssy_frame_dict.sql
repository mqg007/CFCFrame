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
-- Table structure for table `ssy_frame_dict`
--

DROP TABLE IF EXISTS `ssy_frame_dict`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ssy_frame_dict` (
  `ssy_frame_dictid` varchar(50) NOT NULL COMMENT 'ID',
  `domainnameiden` varchar(50) NOT NULL COMMENT '数据域配置识别',
  `domainnames` varchar(200) DEFAULT NULL COMMENT '数据域配置名称',
  `optioniden` varchar(50) NOT NULL COMMENT '数据域配置项识别',
  `optionnames` varchar(200) DEFAULT NULL COMMENT '数据域配置项名称',
  `optioniden_cut` varchar(20) DEFAULT NULL COMMENT '数据域配置项识别简称',
  `optionnames_cut` varchar(50) DEFAULT NULL COMMENT '数据域配置项名称简称',
  `pym` varchar(50) DEFAULT NULL COMMENT '拼音助记码',
  `remarks` varchar(200) DEFAULT NULL COMMENT '备注',
  `timestampss` varchar(50) DEFAULT NULL COMMENT '时间戳',
  PRIMARY KEY (`ssy_frame_dictid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='框架通用字典管理';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ssy_frame_dict`
--

LOCK TABLES `ssy_frame_dict` WRITE;
/*!40000 ALTER TABLE `ssy_frame_dict` DISABLE KEYS */;
INSERT INTO `ssy_frame_dict` VALUES ('0201709080733403437','test','测试字典','t3','项目3','123','儿童3','343','erer333','2017-09-08 07:33:40'),('0201709080927289637','test','测试字典','t2','项目2','1122','1522','12322','test22','2017-09-08 09:27:28'),('020170908092855915','test2','测试字典二','r4','r44','r44','r44','r44','r44','2017-09-08 09:28:55'),('0201709080930025318','newdict','测试新','n2','n2name','n2name','n2name','n2name','n2name','2017-09-08 09:30:02'),('0201709080945039940','test2','测试字典二','tt6','tt6','tt6','tt6','tt6','tt6','2017-09-08 09:45:03'),('0201709081138398974','test','测试字典','t4','项目4','1234','儿童4','344','erer444','2017-09-08 11:38:39'),('1201709081138398974','test','测试字典','t6','t66','t66','t66','6666','66666','2017-09-08 11:38:39'),('201605142042020','test','测试字典','t1','项目一','11','15','123','test','2016-05-14 20:42:03');
/*!40000 ALTER TABLE `ssy_frame_dict` ENABLE KEYS */;
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
