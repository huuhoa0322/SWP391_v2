USE master
GO
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'OnlineShopping')
    DROP DATABASE OnlineShopping
GO

CREATE DATABASE OnlineShopping
GO

USE OnlineShopping
GO

--Inter 1
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
  [review] NVARCHAR(255),
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
  [createdAt] DATETIME NOT NULL,
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
ALTER TABLE [RatingAndReview] ADD CONSTRAINT [ratingandreview_createdby_foreign] FOREIGN KEY ([createdBy]) REFERENCES [User] ([id])
ALTER TABLE [Category] ADD CONSTRAINT [category_parentid_foreign] FOREIGN KEY ([parentId]) REFERENCES [Category] ([id])
ALTER TABLE [Shop] ADD CONSTRAINT [shop_ownerid_foreign] FOREIGN KEY ([ownerId]) REFERENCES [User] ([id])
ALTER TABLE [RatingAndReview] ADD CONSTRAINT [ratingandreview_productid_foreign] FOREIGN KEY ([productId]) REFERENCES [Product] ([id])
ALTER TABLE [Product] ADD CONSTRAINT [product_sellerid_foreign] FOREIGN KEY ([sellerId]) REFERENCES [Shop] ([id])
ALTER TABLE [Product] ADD CONSTRAINT [product_categoryid_foreign] FOREIGN KEY ([categoryId]) REFERENCES [Category] ([id])
GO


--Inter 2
CREATE TABLE [Cart] (
  [id] INT PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [userId] INT NOT NULL,
  [productId] INT NOT NULL,
  [quantity] INT NOT NULL
)
ALTER TABLE [Cart]
ADD CONSTRAINT [cart_userid_foreign] FOREIGN KEY ([userId]) REFERENCES [User]([id]),
    CONSTRAINT [cart_productid_foreign] FOREIGN KEY ([productId]) REFERENCES [Product]([id])
GO


CREATE TABLE [Direct] (
  [id] INT PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [userId] INT NOT NULL,
  [directer] NVARCHAR(255) NOT NULL,
  [phoneNumber] VARCHAR(255) NOT NULL,
  [name] NVARCHAR(255) NOT NULL
)
ALTER TABLE [Direct]
ADD CONSTRAINT [direct_userid_foreign] FOREIGN KEY ([userId]) REFERENCES [User]([id])
GO


CREATE TABLE [Order] (
  [id] INT PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [userId] INT NOT NULL,
  [total] FLOAT(53) NOT NULL,
  [createAt] DATETIME NOT NULL,
  [status] NVARCHAR(50) NOT NULL,
  [directId] INT NOT NULL,
  [paymentMethod] NVARCHAR(50) NOT NULL
)
ALTER TABLE [Order]
ADD CONSTRAINT [order_userid_foreign] FOREIGN KEY ([userId]) REFERENCES [User]([id]),
    CONSTRAINT [order_directid_foreign] FOREIGN KEY ([directId]) REFERENCES [Direct]([id])
GO


CREATE TABLE [OrderDetail] (
  [id] INT PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [orderId] INT NOT NULL,
  [productId] INT NOT NULL,
  [quantity] INT NOT NULL
)
ALTER TABLE [OrderDetail]
ADD CONSTRAINT [orderdetail_orderid_foreign] FOREIGN KEY ([orderId]) REFERENCES [Order]([id]),
    CONSTRAINT [orderdetail_productid_foreign] FOREIGN KEY ([productId]) REFERENCES [Product]([id])
GO


CREATE TABLE [Report] (
  [id] INT PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [productId] INT NOT NULL,
  [userId] INT NOT NULL,
  [createAt] DATETIME NOT NULL,
  [detail] NVARCHAR(255) NOT NULL
)
ALTER TABLE [Report]
ADD CONSTRAINT [report_productid_foreign] FOREIGN KEY ([productId]) REFERENCES [Product]([id]),
    CONSTRAINT [report_userid_foreign] FOREIGN KEY ([userId]) REFERENCES [User]([id])
GO
select * from Category c join Product p on c.id = p.categoryId join Shop s on s.id = p.sellerId


