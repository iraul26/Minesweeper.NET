-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema minesweeperplayers
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema minesweeperplayers
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `minesweeperplayers` DEFAULT CHARACTER SET utf8 ;
USE `minesweeperplayers` ;

-- -----------------------------------------------------
-- Table `minesweeperplayers`.`minesweeperplayers`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `minesweeperplayers`.`minesweeperplayers` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `FirstName` VARCHAR(50) NOT NULL,
  `LastName` VARCHAR(50) NOT NULL,
  `Sex` VARCHAR(50) NOT NULL,
  `Age` INT(11) NOT NULL,
  `State` VARCHAR(50) NOT NULL,
  `Email` VARCHAR(50) NOT NULL,
  `Username` VARCHAR(50) NOT NULL,
  `Password` VARCHAR(50) NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
