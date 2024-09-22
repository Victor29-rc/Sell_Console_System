Sell System

This is a NON-REAL Sell System made for practical purposes only.

Project Overview

The project consists of two modules:

Product Module:

Add, list, edit, and delete products.

Invoice Module:

Register sales as invoices, list them, and delete them if necessary.

Database Setup

To try the project, create a database with the following structure for a SQL Server database:

CREATE DATABASE sell_system
GO

USE sell_system
GO

CREATE TABLE product (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    name VARCHAR(255) NOT NULL UNIQUE,
    price DECIMAL(11,2) NOT NULL CHECK (price > 0),
    description VARCHAR(512)
)
GO

CREATE TABLE invoice (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    customer VARCHAR(255) NOT NULL,
    subtotal DECIMAL(11,2) NOT NULL CHECK (subtotal > 0),
    total DECIMAL(11,2) NOT NULL CHECK (total > 0),
    discount DECIMAL(11,2) DEFAULT 0,
    CONSTRAINT chk_discount CHECK (discount < total)
)
GO

CREATE TABLE product_invoice (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    invoiceId INT NOT NULL FOREIGN KEY REFERENCES invoice (id) ON DELETE CASCADE,
    productId INT NOT NULL FOREIGN KEY REFERENCES product (id),
    quantity INT NOT NULL CHECK (quantity > 0),
    total DECIMAL(11,2) CHECK (total > 0)
)
GO

If you want to use a different database, modify the above queries to fit your specific DB requirements.

Configuration

After creating the database structure, navigate to the DBModel.cs file located in the Models folder. Update the connectionString variable within the class to match your database connection string.

Getting Started

Once you've completed the above steps, you should be able to use the program.
