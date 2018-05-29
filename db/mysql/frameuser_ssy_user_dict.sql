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
-- Table structure for table `ssy_user_dict`
--

DROP TABLE IF EXISTS `ssy_user_dict`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ssy_user_dict` (
  `userid` varchar(50) NOT NULL COMMENT '用户ID',
  `username` varchar(50) DEFAULT NULL COMMENT '用户名称',
  `password` varchar(80) DEFAULT NULL COMMENT '用户口令',
  `registerdate` varchar(50) DEFAULT NULL COMMENT '注册时间',
  `telephone` varchar(16) DEFAULT NULL COMMENT '电话',
  `email` varchar(100) DEFAULT NULL COMMENT '电子邮件',
  `isuse` varchar(1) NOT NULL DEFAULT '1' COMMENT '是否使用',
  `isflag` varchar(1) DEFAULT NULL COMMENT '标识项',
  `note` varchar(255) DEFAULT NULL COMMENT '备注',
  `timestampss` varchar(50) DEFAULT NULL COMMENT '时间戳',
  `islonin` varchar(1) DEFAULT 'N' COMMENT '是否登录',
  `fromplat` varchar(50) DEFAULT NULL COMMENT '登录系统',
  `lastlogintime` varchar(50) DEFAULT NULL COMMENT '上次登录时间',
  `isfirstlogin` varchar(1) DEFAULT 'Y' COMMENT '是否首次登录',
  `failtcnt` varchar(2) DEFAULT '0' COMMENT '登录错误次数',
  `locked` varchar(1) DEFAULT '0' COMMENT '是否锁定用户, 1锁定 0 未锁定 默认0',
  PRIMARY KEY (`userid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='用户';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ssy_user_dict`
--

LOCK TABLES `ssy_user_dict` WRITE;
/*!40000 ALTER TABLE `ssy_user_dict` DISABLE KEYS */;
INSERT INTO `ssy_user_dict` VALUES ('010','wwwe','t+yxp4/91wOTWywTOUFr6w==','2017-09-18 14:40:27','13593068627','23123847@qq.com','1','','test','2017-09-18 14:40:27','N','','2017-09-18 16:27:59','N','0','0'),('021ff','021ff','t+yxp4/91wOTWywTOUFr6w==','2017-09-18 13:49:47','13793068428','18223242567@qq.com','1','','dfsfd测试dd','2017-09-18 13:49:47','N','','','Y','0','0'),('022','dddd','t+yxp4/91wOTWywTOUFr6w==','2017-09-07 21:34:39','13693068428','17223242567@qq.com','1','','dfsfd测试dd','2017-09-07 21:34:39','N','','','Y','0','0'),('027027','test6tt','t+yxp4/91wOTWywTOUFr6w==','2018-01-12 15:33:17','13693068623','23122332@qq.com','1','','testdddtest','2018-01-12 15:33:17','N','','','Y','0','0'),('028','ddd','t+yxp4/91wOTWywTOUFr6w==','2017-08-18 09:45:59','13693068628','17292432567@qq.com','1','','dfsfd测试dd','2017-08-18 09:45:59','N','','','Y','0','0'),('08','dddds','t+yxp4/91wOTWywTOUFr6w==','2017-09-07 21:38:15','13693068228','17233242567@qq.com','1','','dfsfd测试dd','2017-09-07 21:38:15','N','','','Y','0','0'),('mqg','mqg','t+yxp4/91wOTWywTOUFr6w==','2016-04-04 16:20:10','13693068625','23123245@qq.com','1','','dddf','2016-04-06 12:46:07','N','','2018-01-16 16:53:56','N','0','0'),('test5','test5','t+yxp4/91wOTWywTOUFr6w==','2017-02-02 21:41:54','13693068621','23132341@qq.com','1','','testddd','2017-02-02 21:41:54','N','','','Y','0','0'),('test6','test6','t+yxp4/91wOTWywTOUFr6w==','2017-02-02 21:41:54','13693068622','23122342@qq.com','1','','testddd','2017-02-02 21:41:54','N','','','Y','0','0'),('u1n','u1n','t+yxp4/91wOTWywTOUFr6w==','2017-09-14 08:47:57','13645687845','111444555@qq.com','1','','test','2017-09-14 08:47:57','N','','','Y','0','0'),('ud20','www','t+yxp4/91wOTWywTOUFr6w==','2016-07-03 09:53:43','13693068627','23123147@qq.com','1','','test','2016-07-03 09:53:44','N','','','N','0','0'),('ud21','m2','t+yxp4/91wOTWywTOUFr6w==','2016-04-17 10:56:17','13693068626','23125346@qq.com','1','','test','2016-04-17 12:23:28','N','','','N','0','0'),('ww2','www2','t+yxp4/91wOTWywTOUFr6w==','2017-09-18 14:14:49','13623068627','23133147@qq.com','1','','test','2017-09-18 14:14:53','N','','','Y','0','0');
/*!40000 ALTER TABLE `ssy_user_dict` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-05-29  9:50:49
