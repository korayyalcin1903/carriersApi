-- Veritabanı Oluştur
CREATE DATABASE KargoDB;

-- KargoDB Veritabanını Kullan
USE KargoDB;

-- Kargo Firmaları Tablosunu Oluştur
CREATE TABLE Carriers (
    CarrierID INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
    CarrierName VARCHAR(100) NOT NULL,
    CarrierIsActive TINYINT NOT NULL,
    CarrierPlusDesiCost INT NOT NULL
);

-- Kargo Firma Konfigürasyonları Tablosunu Oluştur
CREATE TABLE CarrierConfigurations (
    CarrierConfigurationId INT PRIMARY KEY AUTO_INCREMENT,
    CarrierID INT,
    CarrierMaxDesi INT NOT NULL,
    CarrierMinDesi INT NOT NULL,
    CarrierCost DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (CarrierID) REFERENCES Carriers(CarrierID)
);

-- Siparişler Tablosunu Oluştur
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
    CarrierID INT,
    OrderDesi INT NOT NULL,
    OrderDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    OrderCarrierCost DECIMAL(10, 2),
    FOREIGN KEY (CarrierID) REFERENCES Carriers(CarrierID)
);
