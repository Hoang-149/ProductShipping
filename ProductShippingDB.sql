-- SQL Server Script
CREATE DATABASE ProductShippingDB;
GO

USE ProductShippingDB;
GO

-- Table for Products
CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100),
    Price DECIMAL(18, 2),
    Weight INT
);

-- Table for Courier Charges
CREATE TABLE CourierCharges (
    Id INT PRIMARY KEY IDENTITY,
    MinWeight INT,
    MaxWeight INT,
    Charge DECIMAL(18, 2)
);

-- Insert sample data for Products
INSERT INTO Products (Name, Price, Weight) VALUES
('Item 1', 10, 200),
('Item 2', 100, 20),
('Item 3', 30, 300),
('Item 4', 50, 500),
('Item 5', 30, 250),
('Item 6', 10, 10),
('Item 7', 200, 10),
('Item 8', 120, 500),
('Item 9', 10, 200),
('Item 10', 20, 50),
('Item 11', 4, 800),
('Item 12', 4, 20),
('Item 13', 5, 200),
('Item 14', 240, 123);

-- Insert sample data for Courier Charges
INSERT INTO CourierCharges (MinWeight, MaxWeight, Charge) VALUES
(0, 200, 5),
(200, 500, 10),
(500, 1000, 15),
(1000, 5000, 20);