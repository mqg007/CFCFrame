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
-- Table structure for table `ssy_group_dict`
--

DROP TABLE IF EXISTS `ssy_group_dict`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ssy_group_dict` (
  `groupid` varchar(50) NOT NULL COMMENT 'ID',
  `groupname` varchar(50) NOT NULL COMMENT '角色名称',
  `groupdesc` varchar(100) DEFAULT NULL COMMENT '角色备注',
  `isuse` varchar(1) NOT NULL DEFAULT '1' COMMENT '是否使用',
  `isflag` varchar(1) DEFAULT NULL COMMENT '删除标识',
  `groupno` varchar(50) NOT NULL COMMENT '角色编码',
  `timestampss` varchar(50) DEFAULT NULL COMMENT '时间戳',
  PRIMARY KEY (`groupid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='角色';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ssy_group_dict`
--

LOCK TABLES `ssy_group_dict` WRITE;
/*!40000 ALTER TABLE `ssy_group_dict` DISABLE KEYS */;
INSERT INTO `ssy_group_dict` VALUES ('020170815225641274','554','554','1','','554','2017-08-16 09:42:14'),('120170815225641274','555','5555777','0','','555','2017-08-16 09:42:14'),('20170202083255299','test001','testmodify','1','','test001','2017-08-16 09:42:14'),('20170202083727155','test002','test','1','','test002','2017-08-17 17:45:04'),('20170202083819133','test003','test003','1','','test003','2017-02-02 08:38:19'),('20170202084824544','test004','test004','0','','test004','2018-01-15 11:43:16'),('20170815221119962','nav001','test add new data','1','','0012','2017-08-15 22:11:21'),('grp001','管理员组','管理员组','1','','grp001','2016-03-07 19:49:21'),('grp002','生产组','生产组','1','','grp002','2016-03-10 15:54:10'),('grp003','测试组','测试组','1','','grp003','2016-03-07 19:55:12'),('grp004','销售组','销售组','1','','grp004','2016-03-07 19:49:21'),('grp005','研发组','研发组','1','','grp005','2016-03-10 15:54:19'),('grp006','智能组','智能组','1','','grp006','2016-03-07 19:55:12'),('grp007','管理办公室','管理办公室test','1','','grp007','2016-03-07 19:49:21'),('grp008','项目组','项目组3','1','','grp008','2016-04-03 21:43:23'),('grp009','质量组','质量组2','1','','grp009','2016-04-03 21:43:23');
/*!40000 ALTER TABLE `ssy_group_dict` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-05-29  9:50:46
