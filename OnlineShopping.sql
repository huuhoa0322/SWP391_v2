USE master
GO
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'OnlineShopping')
    DROP DATABASE OnlineShopping
GO

CREATE DATABASE OnlineShopping
GO

USE OnlineShopping
GO

CREATE TABLE [User] (
  [id] INT PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [name] NVARCHAR(255) NOT NULL,
  [username] VARCHAR(255) NOT NULL,
  [password] VARCHAR(255) NOT NULL,
  [gender] BIT NOT NULL,
  [email] VARCHAR(255) NOT NULL,
  [role] VARCHAR(255) NOT NULL,
  [dob] DATE NOT NULL,
  [loginBy] BIT NOT NULL,
  [isDeleted] BIT NOT NULL,
  [deletedBy] INT,
  [deletedAt] DATETIME
)
GO

CREATE TABLE [Product] (
  [id] INT PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [name] NVARCHAR(255) NOT NULL,
  [price] FLOAT(53) NOT NULL,
  [description] NVARCHAR(255) NOT NULL,
  [image] VARCHAR(255) NOT NULL,
  [sellerId] INT NOT NULL,
  [categoryId] INT NOT NULL,
  [quantitySold] INT NOT NULL,
  [inventory] INT NOT NULL,
  [isDeleted] BIT NOT NULL,
  [deletedBy] INT,
  [deletedAt] DATETIME
)
GO

CREATE TABLE [RatingAndReview] (
  [id] INT PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [productId] INT NOT NULL,
  [rating] INT NOT NULL,
  [review] NVARCHAR(255) NOT NULL,
  [createdBy] INT NOT NULL,
  [createdAt] DATETIME NOT NULL
)
GO

CREATE TABLE [Category] (
  [id] INT PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [name] NVARCHAR(255) NOT NULL,
  [parentId] INT
)
GO

CREATE TABLE [Shop] (
  [id] INT PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [ownerId] INT NOT NULL,
  [name] NVARCHAR(255) NOT NULL,
  [description] NVARCHAR(255) NOT NULL,
  [createdAt] DATE NOT NULL,
  [logo] VARCHAR(255) NOT NULL,
  [isDeleted] BIT NOT NULL,
  [deletedBy] INT,
  [deletedAt] DATETIME
)
GO

CREATE TABLE [Discount] (
  [id] INT PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [productId] INT NOT NULL,
  [value] INT NOT NULL,
  [startAt] DATETIME NOT NULL,
  [endAt] DATETIME NOT NULL,
  [isActive] BIT NOT NULL
)
GO

ALTER TABLE [Discount] ADD CONSTRAINT [discount_productid_foreign] FOREIGN KEY ([productId]) REFERENCES [Product] ([id])
GO

ALTER TABLE [RatingAndReview] ADD CONSTRAINT [ratingandreview_createdby_foreign] FOREIGN KEY ([createdBy]) REFERENCES [User] ([id])
GO

ALTER TABLE [Category] ADD CONSTRAINT [category_parentid_foreign] FOREIGN KEY ([parentId]) REFERENCES [Category] ([id])
GO

ALTER TABLE [Shop] ADD CONSTRAINT [shop_ownerid_foreign] FOREIGN KEY ([ownerId]) REFERENCES [User] ([id])
GO

ALTER TABLE [RatingAndReview] ADD CONSTRAINT [ratingandreview_productid_foreign] FOREIGN KEY ([productId]) REFERENCES [Product] ([id])
GO

ALTER TABLE [Product] ADD CONSTRAINT [product_sellerid_foreign] FOREIGN KEY ([sellerId]) REFERENCES [Shop] ([id])
GO

ALTER TABLE [Product] ADD CONSTRAINT [product_categoryid_foreign] FOREIGN KEY ([categoryId]) REFERENCES [Category] ([id])
GO

INSERT INTO [User] (name, username, password, gender, email, role, dob, loginBy, isDeleted, deletedBy, deletedAt)
VALUES
    (N'John Doe', 'johndoe', 'password123', 1, 'johndoe@example.com', 'customer', '1990-05-15', 0, 0, NULL, NULL),
    (N'admin', 'admin', 'admin', 1, 'admin@example.com', 'admin', '1990-05-15', 0, 0, NULL, NULL);

select * from [User]