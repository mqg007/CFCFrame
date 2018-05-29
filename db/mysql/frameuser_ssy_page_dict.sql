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
-- Table structure for table `ssy_page_dict`
--

DROP TABLE IF EXISTS `ssy_page_dict`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ssy_page_dict` (
  `pageid` varchar(50) NOT NULL COMMENT 'ID',
  `pagename` varchar(50) NOT NULL COMMENT '功能名称',
  `pageparentid` varchar(15) NOT NULL COMMENT '功能编号',
  `pageurl` varchar(100) DEFAULT NULL COMMENT 'Web页面地址',
  `pagetarget` varchar(20) DEFAULT NULL COMMENT 'Web目标框架',
  `pageimg` varchar(50) DEFAULT NULL COMMENT '功能图片',
  `isuse` varchar(1) NOT NULL DEFAULT '1' COMMENT '是否使用',
  `pagemoudal` varchar(20) DEFAULT NULL COMMENT '功能分组',
  `isflag` varchar(1) DEFAULT '1' COMMENT '标识项',
  `seqsort` varchar(10) DEFAULT NULL COMMENT '排序',
  `pageclassname` varchar(200) DEFAULT NULL COMMENT 'winform类名',
  `pageassembly` varchar(200) DEFAULT NULL COMMENT 'winform程序集',
  `wpfpageurl` varchar(200) DEFAULT NULL COMMENT 'WPF页面地址',
  `wpfpagetarget` varchar(200) DEFAULT NULL COMMENT 'WPF程序集',
  `plattypes` varchar(20) DEFAULT NULL COMMENT 'WPF WINFORM WEB',
  `timestampss` varchar(50) DEFAULT NULL COMMENT '时间戳',
  PRIMARY KEY (`pageid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='功能权限';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ssy_page_dict`
--

LOCK TABLES `ssy_page_dict` WRITE;
/*!40000 ALTER TABLE `ssy_page_dict` DISABLE KEYS */;
INSERT INTO `ssy_page_dict` VALUES ('001','业务功能','1','001','mainFrame','fa fa-meh-o','1','业务功能','1','10','','','','','WEB','2017-08-17 10:13:21'),('001001','CRM业务','001','','mainFrame','fa fa-tasks','1','业务功能','1','5','','','','','WEB','2017-08-17 10:14:33'),('001001001','客户管理','001001','_sysmanager_frmcus.html','mainFrame','fa fa-tasks','1','业务功能','1','6','','','','','WEB','2017-08-16 20:11:16'),('001005','ERP业务','001','','mainFrame','fa fa-tasks','1','业务功能','1','7','','','','','WEB',''),('001005001','进货管理','001005','_sysmanager_frmingo.html','mainFrame','fa fa-tasks','1','业务功能','1','10','','','','','WEB','2017-08-17 10:16:39'),('001005002','出货管理','001005','_sysmanager_frmoutgo.html','mainFrame','fa fa-tasks','1','业务功能','1','9','','','','','WEB',''),('1','综合系统','0','','mainFrame','fa fa-folder','1','','1','1         ','','','','','WEB',''),('800','系统管理','1','800','mainFrame','fa fa-cog','1','','1','800','','','','','WEB',''),('800001','权限管理','800','','mainFrame','fa fa-cogs','1','系统管理','1','10','','','','','WEB',''),('800001001','用户组','800001','_modules_sysmanager_html_frmOpGroups.html','mainFrame','fa fa-users','1','系统管理','1','10','','','sysmanagerWpfOpGroups.xaml','mainFrame','WEB',''),('800001005','用户','800001','_modules_sysmanager_html_frmOpUsers.html','mainFrame','fa fa-user-circle-o','1','系统管理','1','15','','','sysmanagerPagetest.xaml','mainFrame','WEB',''),('800001010','页面','800001','_modules_sysmanager_html_frmOpPages.html','mainFrame','fa fa-list-ul','1','系统管理','1','20','','','sysmanagerPage1.xaml','mainFrame','WEB',''),('800001015','权限配置','800001','_modules_sysmanager_html_frmOpPrivilege.html','mainFrame','fa fa-pencil','1','系统管理','1','25','','','','','WEB',''),('800001020','重置密码','800001','_sysmanager_frmReSetUserPwd.html','mainFrame','fa fa-key','1','系统管理','1','30','','','','','WEB',''),('800001025','修改密码','800001','_sysmanager_frmModifyUserPwd.html','mainFrame','fa fa-wrench','1','系统管理','1','35','','','','','WEB',''),('800010','日志管理','800','','mainFrame','fa fa-comments','1','系统管理','1','25','','','','','WEB',''),('800010001','日志查询','800010','_sysmanager__frmLogQuery.html','mainFrame','fa fa-comment','1','系统管理','1','10','','','','','WEB',''),('800015','维护管理','800','_sysmanager_frmHelp.html','mainFrame','fa fa-handshake-o','1','系统管理','1','30','','','','','WEB',''),('800015001','维护服务','800015','','mainFrame','fa fa-info-circle','1','系统管理','1','10','','','','','WEB',''),('800020','字典管理','800','','mainFrame','fa fa-cubes','1','系统管理','1','15','','','','','WEB',''),('800020001','公共字典','800020','_sysmanager_frmSystemDictionary.html','mainFrame','fa fa-cube','1','系统管理','1','10','','','','','WEB',''),('800020005','业务字典','800020','_sysmanager_frmBizDictionary.html','mainFrame','fa fa-cube','1','系统管理','1','15','','','','','WEB','2016-04-18 22:50:52');
/*!40000 ALTER TABLE `ssy_page_dict` ENABLE KEYS */;
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
