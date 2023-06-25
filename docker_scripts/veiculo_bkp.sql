-- --------------------------------------------------------
-- Servidor:                     127.0.0.1
-- Versão do servidor:           10.4.22-MariaDB - mariadb.org binary distribution
-- OS do Servidor:               Win64
-- HeidiSQL Versão:              12.0.0.6468
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Copiando estrutura do banco de dados para veiculo_info
CREATE DATABASE IF NOT EXISTS `veiculo_info` /*!40100 DEFAULT CHARACTER SET utf8mb4 */;
USE `veiculo_info`;

-- Copiando estrutura para tabela veiculo_info.tbl_arquivo_upload
CREATE TABLE IF NOT EXISTS `tbl_arquivo_upload` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `legenda` varchar(40) NOT NULL,
  `nome_arquivo` varchar(40) NOT NULL,
  `id_veiculo` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_bl_arquivo_upload_tbl_veiculo` (`id_veiculo`),
  CONSTRAINT `FK_bl_arquivo_upload_tbl_veiculo` FOREIGN KEY (`id_veiculo`) REFERENCES `tbl_veiculo` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8mb4;

-- Exportação de dados foi desmarcado.

-- Copiando estrutura para tabela veiculo_info.tbl_veiculo
CREATE TABLE IF NOT EXISTS `tbl_veiculo` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nome_proprietario` varchar(70) NOT NULL,
  `modelo_veiculo` varchar(30) NOT NULL,
  `fabricante_veiculo` varchar(30) NOT NULL,
  `ano_veiculo` varchar(4) NOT NULL,
  `cor_veiculo` varchar(20) NOT NULL,
  `estado` varchar(20) NOT NULL,
  `cidade` varchar(50) NOT NULL,
  `informacoes_gerais` text DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8mb4;

-- Exportação de dados foi desmarcado.

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
