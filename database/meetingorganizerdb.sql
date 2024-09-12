-- MySQL dump 10.13  Distrib 8.0.38, for Win64 (x86_64)
--
-- Host: localhost    Database: meetingorganizerdb
-- ------------------------------------------------------
-- Server version	9.0.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `meetingparticipants`
--

DROP TABLE IF EXISTS `meetingparticipants`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `meetingparticipants` (
  `meeting_id` int NOT NULL,
  `participant_id` int NOT NULL,
  PRIMARY KEY (`meeting_id`,`participant_id`),
  KEY `participant_id` (`participant_id`),
  CONSTRAINT `meetingparticipants_ibfk_1` FOREIGN KEY (`meeting_id`) REFERENCES `meetings` (`meeting_id`) ON DELETE CASCADE,
  CONSTRAINT `meetingparticipants_ibfk_2` FOREIGN KEY (`participant_id`) REFERENCES `participants` (`participant_id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `meetingparticipants`
--

LOCK TABLES `meetingparticipants` WRITE;
/*!40000 ALTER TABLE `meetingparticipants` DISABLE KEYS */;
INSERT INTO `meetingparticipants` VALUES (1,1),(11,1),(1,2),(11,2),(9,11),(11,13),(1,20),(9,21),(9,22),(6,23),(18,24),(18,25);
/*!40000 ALTER TABLE `meetingparticipants` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `meetings`
--

DROP TABLE IF EXISTS `meetings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `meetings` (
  `meeting_id` int NOT NULL AUTO_INCREMENT,
  `title` varchar(255) NOT NULL,
  `date` date NOT NULL,
  `start_time` time NOT NULL,
  `end_time` time NOT NULL,
  PRIMARY KEY (`meeting_id`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `meetings`
--

LOCK TABLES `meetings` WRITE;
/*!40000 ALTER TABLE `meetings` DISABLE KEYS */;
INSERT INTO `meetings` VALUES (1,'FAI Raporları Güncel','2024-09-15','09:00:00','11:00:00'),(6,'Ömür Testleri Toplantısı','2024-09-14','14:50:00','17:50:00'),(9,'Daily Meeting (Opsiyonel)','2025-11-11','10:30:00','11:30:00'),(11,'Makina Bakım Takip Toplantısı','2024-09-15','09:00:00','11:00:00'),(18,'IK Yıllık İzin Duyuruları ','2024-09-15','15:00:00','15:30:00');
/*!40000 ALTER TABLE `meetings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `participants`
--

DROP TABLE IF EXISTS `participants`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `participants` (
  `participant_id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  PRIMARY KEY (`participant_id`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `participants`
--

LOCK TABLES `participants` WRITE;
/*!40000 ALTER TABLE `participants` DISABLE KEYS */;
INSERT INTO `participants` VALUES (1,'Ahmet Yılmaz'),(2,'Ayşe Demir'),(3,'Mehmet Can'),(4,'Atıl Can'),(5,'Mert Köse'),(6,'Beyza Coşkun'),(7,'mert cansuz'),(8,'pelin yaşaran'),(9,'Melike Şahin'),(10,'Mert Can Elim'),(11,'Melih Süre'),(12,'Neslihan Çakır'),(13,'Seyhun Koçak'),(14,'cihan cengiz'),(15,'asd'),(16,'ceyhun koçak'),(17,'asdasd'),(18,'velican kokoo'),(19,'velicako'),(20,'Ali Can'),(21,'Can Yıldırım'),(22,'Filiz Cansu Koç'),(23,'Mert Ali Fidan'),(24,'Şirket Çalışanları'),(25,'Yönetim Kurulu Üyeleri'),(26,'Mert Demir'),(27,'Ayşe Nur Yılmaz');
/*!40000 ALTER TABLE `participants` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-09-12 15:07:23
