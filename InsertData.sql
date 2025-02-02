INSERT INTO [User] (name, username, password, gender, email, role, dob, [loginBy], isDeleted, deletedBy, deletedAt)
VALUES
    (N'John Doe', 'johndoe', 'password123', 1, 'johndoe@example.com', 'customer', '1990-05-15', 0, 0, NULL, NULL),
    (N'admin', 'admin', 'admin', 1, 'admin@example.com', 'admin', '1990-05-15', 0, 0, NULL, NULL),
	('Alice Smith', 'alice', 'password123', 1, 'alice@example.com', 'Seller', '1990-05-15',0,0, null, null);
	select *from [user]

-- Insert sample data into the [Category] table

-- Insert sample data into the [Shop] table
INSERT INTO [Shop] ([ownerId], [name], [description], [createdAt], [logo], [isDeleted])
VALUES
(3, 'Alices Electronics', 'Best electronics in town', '2023-01-01', 'alice_logo.png', 0),
(1, 'Breath', 'Best elen town', '2023-01-01', 'alice_logo.png', 0);

-- Insert sample data into the [Discount] table
INSERT INTO [Discount] ([productId], [value], [startAt], [endAt], [isActive])
VALUES
(1, 10, GETDATE(), DATEADD(DAY, 10, GETDATE()), 1),
(2, 15, GETDATE(), DATEADD(DAY, 7, GETDATE()), 1);

-- Thêm danh mục chính 
INSERT INTO [Category] ([name], [parentId]) VALUES ('Fashion', NULL);
INSERT INTO [Category] ([name], [parentId]) VALUES ('Electronics & Technology', NULL);
INSERT INTO [Category] ([name], [parentId]) VALUES ('Toys', NULL);
INSERT INTO [Category] ([name], [parentId]) VALUES ('Sports', NULL);
INSERT INTO [Category] ([name], [parentId]) VALUES ('Mother & Baby', NULL);

select *from Category

-- Thêm danh mục con đúng vào danh mục cha tương ứng
INSERT INTO [Category] ([name], [parentId]) VALUES 
('Men Clothing', (SELECT id FROM [Category] WHERE [name] = 'Fashion')),
('Women Clothing', (SELECT id FROM [Category] WHERE [name] = 'Fashion')),
('Shoes & Accessories', (SELECT id FROM [Category] WHERE [name] = 'Fashion')),

('Mobile Phones', (SELECT id FROM [Category] WHERE [name] = 'Electronics & Technology')),
('Laptops', (SELECT id FROM [Category] WHERE [name] = 'Electronics & Technology')),
('Smartwatches', (SELECT id FROM [Category] WHERE [name] = 'Electronics & Technology')),

('Educational Toys', (SELECT id FROM [Category] WHERE [name] = 'Toys')),
('Action Figures', (SELECT id FROM [Category] WHERE [name] = 'Toys')),
('Board Games', (SELECT id FROM [Category] WHERE [name] = 'Toys')),

('Fitness Equipment', (SELECT id FROM [Category] WHERE [name] = 'Sports')),
('Outdoor Sports', (SELECT id FROM [Category] WHERE [name] = 'Sports')),
('Sportswear', (SELECT id FROM [Category] WHERE [name] = 'Sports')),

('Baby Clothes', (SELECT id FROM [Category] WHERE [name] = 'Mother & Baby')),
('Diapers & Baby Care', (SELECT id FROM [Category] WHERE [name] = 'Mother & Baby')),
('Strollers & Carriers', (SELECT id FROM [Category] WHERE [name] = 'Mother & Baby'));

select *from category

-- Insert products for Men Clothing
INSERT INTO [Product] ([name], [price], [description], [image], [sellerId], [categoryId], [quantitySold], [inventory], [isDeleted]) VALUES
('Men Formal Shirt', 29.99, 'Slim fit cotton shirt', 'men_shirt.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Men Clothing'), 0, 50, 0),
('Casual T-Shirt', 19.99, 'Comfortable summer wear', 'casual_tshirt.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Men Clothing'), 0, 50, 0),
('Denim Jeans', 49.99, 'Blue slim fit jeans', 'denim_jeans.jpg', 2, (SELECT id FROM [Category] WHERE [name] = 'Men Clothing'), 0, 50, 0),
('Leather Jacket', 89.99, 'Classic black leather jacket', 'leather_jacket.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Men Clothing'), 0, 50, 0),
('Chino Pants', 39.99, 'Casual chino pants', 'chino_pants.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Men Clothing'), 0, 50, 0);


-- Insert products for Women Clothing
INSERT INTO [Product] ([name], [price], [description], [image], [sellerId], [categoryId], [quantitySold], [inventory], [isDeleted]) VALUES
('Summer Dress', 35.99, 'Lightweight floral dress', 'summer_dress.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Women Clothing'), 0, 50, 0),
('High-Waist Jeans', 45.99, 'Classic blue high-waist jeans', 'highwaist_jeans.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Women Clothing'), 0, 50, 0),
('Women Blazer', 55.99, 'Elegant office blazer', 'women_blazer.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Women Clothing'), 0, 50, 0),
('Crop Top', 25.99, 'Trendy cotton crop top', 'crop_top.jpg', 2, (SELECT id FROM [Category] WHERE [name] = 'Women Clothing'), 0, 50, 0),
('Maxi Skirt', 39.99, 'Flowy long skirt', 'maxi_skirt.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Women Clothing'), 0, 50, 0);

-- Insert products for Shoes & Accessories
INSERT INTO [Product] ([name], [price], [description], [image], [sellerId], [categoryId], [quantitySold], [inventory], [isDeleted]) VALUES
('Leather Belt', 15.99, 'Brown genuine leather belt', 'leather_belt.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Shoes & Accessories'), 0, 50, 0),
('Sneakers', 59.99, 'Casual white sneakers', 'sneakers.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Shoes & Accessories'), 0, 50, 0),
('Running Shoes', 79.99, 'Breathable sports shoes', 'running_shoes.jpg', 2, (SELECT id FROM [Category] WHERE [name] = 'Shoes & Accessories'), 0, 50, 0),
('Handbag', 49.99, 'Fashionable leather handbag', 'handbag.jpg', 2, (SELECT id FROM [Category] WHERE [name] = 'Shoes & Accessories'), 0, 50, 0),
('Sunglasses', 29.99, 'UV protection sunglasses', 'sunglasses.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Shoes & Accessories'), 0, 50, 0);

-- Insert products for Mobile Phones
INSERT INTO [Product] ([name], [price], [description], [image], [sellerId], [categoryId], [quantitySold], [inventory], [isDeleted]) VALUES
('iPhone 14', 999.99, '128GB, Midnight', 'iphone14.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Mobile Phones'), 0, 50, 0),
('Samsung Galaxy S23', 899.99, '256GB, Phantom Black', 'galaxy_s23.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Mobile Phones'), 0, 50, 0),
('Google Pixel 7', 799.99, '128GB, Snow', 'pixel7.jpg', 2, (SELECT id FROM [Category] WHERE [name] = 'Mobile Phones'), 0, 50, 0),
('OnePlus 11', 699.99, '12GB RAM, 256GB Storage', 'oneplus_11.jpg', 2, (SELECT id FROM [Category] WHERE [name] = 'Mobile Phones'), 0, 50, 0),
('Xiaomi 13 Pro', 599.99, 'Snapdragon 8 Gen 2', 'xiaomi_13pro.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Mobile Phones'), 0, 50, 0);

-- Insert products for Laptops
INSERT INTO [Product] ([name], [price], [description], [image], [sellerId], [categoryId], [quantitySold], [inventory], [isDeleted]) VALUES
('MacBook Pro 16"', 2499.99, 'M2 Pro Chip, 512GB SSD', 'macbook_pro.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Laptops'), 0, 50, 0),
('Dell XPS 13', 1399.99, '13" UHD, 16GB RAM', 'dell_xps.jpg', 2, (SELECT id FROM [Category] WHERE [name] = 'Laptops'), 0, 50, 0),
('HP Spectre x360', 1299.99, '2-in-1 Touchscreen', 'hp_spectre.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Laptops'), 0, 50, 0),
('Lenovo ThinkPad X1', 1499.99, 'Business Laptop', 'thinkpad_x1.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Laptops'), 0, 50, 0),
('Asus ROG Zephyrus', 1799.99, 'Gaming Laptop', 'asus_rog.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Laptops'), 0, 50, 0);

-- Insert products for Smartwatches
INSERT INTO [Product] ([name], [price], [description], [image], [sellerId], [categoryId], [quantitySold], [inventory], [isDeleted]) VALUES
('Apple Watch Series 8', 399.99, 'GPS, 45mm Midnight', 'apple_watch8.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Smartwatches'), 0, 50, 0),
('Samsung Galaxy Watch 5', 329.99, 'Bluetooth, 44mm', 'galaxy_watch5.jpg', 2, (SELECT id FROM [Category] WHERE [name] = 'Smartwatches'), 0, 50, 0),
('Garmin Forerunner 955', 499.99, 'GPS Running Watch', 'garmin_955.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Smartwatches'), 0, 50, 0),
('Fitbit Sense 2', 299.99, 'Advanced health tracking', 'fitbit_sense2.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Smartwatches'), 0, 50, 0),
('Huawei Watch GT 3', 249.99, 'Long battery life', 'huawei_gt3.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Smartwatches'), 0, 50, 0);

-- Insert products for Educational Toys
INSERT INTO [Product] ([name], [price], [description], [image], [sellerId], [categoryId], [quantitySold], [inventory], [isDeleted]) VALUES
('LEGO Classic Set', 39.99, 'Building blocks for creativity', 'lego_classic.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Educational Toys'), 0, 50, 0),
('Math Learning Kit', 24.99, 'Interactive math learning', 'math_kit.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Educational Toys'), 0, 50, 0),
('Wooden Puzzle', 14.99, 'Colorful alphabet puzzle', 'wooden_puzzle.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Educational Toys'), 0, 50, 0),
('STEM Robotics Kit', 89.99, 'DIY robotic projects', 'robotics_kit.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Educational Toys'), 0, 50, 0),
('Flashcards for Kids', 9.99, 'ABC and number flashcards', 'flashcards.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Educational Toys'), 0, 50, 0);

-- Insert products for Action Figures
INSERT INTO [Product] ([name], [price], [description], [image], [sellerId], [categoryId], [quantitySold], [inventory], [isDeleted]) VALUES
('Marvel Spider-Man Figure', 29.99, '6-inch action figure', 'spiderman_figure.jpg', 2, (SELECT id FROM [Category] WHERE [name] = 'Action Figures'), 0, 50, 0),
('Batman Collectible', 35.99, 'Limited edition Batman', 'batman_figure.jpg', 2, (SELECT id FROM [Category] WHERE [name] = 'Action Figures'), 0, 50, 0),
('Transformers Optimus Prime', 49.99, 'Transforming robot toy', 'optimus_prime.jpg', 2, (SELECT id FROM [Category] WHERE [name] = 'Action Figures'), 0, 50, 0),
('Star Wars Darth Vader', 39.99, 'Classic Star Wars figure', 'darth_vader.jpg', 2, (SELECT id FROM [Category] WHERE [name] = 'Action Figures'), 0, 50, 0),
('Pokemon Pikachu', 19.99, 'Cute Pikachu action figure', 'pikachu.jpg', 2, (SELECT id FROM [Category] WHERE [name] = 'Action Figures'), 0, 50, 0);

-- Insert products for Board Games
INSERT INTO [Product] ([name], [price], [description], [image], [sellerId], [categoryId], [quantitySold], [inventory], [isDeleted]) VALUES
('Monopoly Classic', 34.99, 'Board game for family fun', 'monopoly.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Board Games'), 0, 50, 0),
('Chess Set', 19.99, 'Wooden chess game', 'chess_set.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Board Games'), 0, 50, 0),
('Scrabble', 24.99, 'Word game for all ages', 'scrabble.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Board Games'), 0, 50, 0),
('UNO Card Game', 9.99, 'Fun card game for friends', 'uno.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Board Games'), 0, 50, 0),
('Jenga', 14.99, 'Stacking block game', 'jenga.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Board Games'), 0, 50, 0);

-- Insert products for Fitness Equipment
INSERT INTO [Product] ([name], [price], [description], [image], [sellerId], [categoryId], [quantitySold], [inventory], [isDeleted]) VALUES
('Dumbbell Set', 49.99, 'Adjustable weights', 'dumbbell.jpg', 2, (SELECT id FROM [Category] WHERE [name] = 'Fitness Equipment'), 0, 50, 0),
('Treadmill', 799.99, 'High-performance treadmill', 'treadmill.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Fitness Equipment'), 0, 50, 0),
('Yoga Mat', 19.99, 'Non-slip yoga mat', 'yoga_mat.jpg', 2, (SELECT id FROM [Category] WHERE [name] = 'Fitness Equipment'), 0, 50, 0),
('Resistance Bands', 14.99, 'Full-body workout bands', 'resistance_bands.jpg', 2, (SELECT id FROM [Category] WHERE [name] = 'Fitness Equipment'), 0, 50, 0),
('Kettlebell 10kg', 34.99, 'Cast iron kettlebell', 'kettlebell.jpg', 2, (SELECT id FROM [Category] WHERE [name] = 'Fitness Equipment'), 0, 50, 0);

-- Insert products for Outdoor Sports
INSERT INTO [Product] ([name], [price], [description], [image], [sellerId], [categoryId], [quantitySold], [inventory], [isDeleted]) VALUES
('Mountain Bike', 599.99, 'All-terrain bike', 'mountain_bike.jpg', 2, (SELECT id FROM [Category] WHERE [name] = 'Outdoor Sports'), 0, 50, 0),
('Football', 19.99, 'Official size football', 'football.jpg', 2, (SELECT id FROM [Category] WHERE [name] = 'Outdoor Sports'), 0, 50, 0),
('Camping Tent', 99.99, '4-person waterproof tent', 'camping_tent.jpg', 2, (SELECT id FROM [Category] WHERE [name] = 'Outdoor Sports'), 0, 50, 0),
('Fishing Rod', 49.99, 'Carbon fiber fishing rod', 'fishing_rod.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Outdoor Sports'), 0, 50, 0),
('Badminton Racket Set', 29.99, 'Set of 2 rackets', 'badminton_racket.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Outdoor Sports'), 0, 50, 0);

-- Insert products for Baby Clothes
INSERT INTO [Product] ([name], [price], [description], [image], [sellerId], [categoryId], [quantitySold], [inventory], [isDeleted]) VALUES
('Baby Bodysuit', 12.99, 'Cotton newborn bodysuit', 'baby_bodysuit.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Baby Clothes'), 0, 50, 0),
('Toddler Dress', 18.99, 'Cute floral dress', 'toddler_dress.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Baby Clothes'), 0, 50, 0),
('Winter Jacket', 24.99, 'Warm baby jacket', 'baby_jacket.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Baby Clothes'), 0, 50, 0),
('Baby Socks Set', 9.99, 'Pack of 5 soft socks', 'baby_socks.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Baby Clothes'), 0, 50, 0),
('Cotton Pants', 14.99, 'Soft and stretchy pants', 'baby_pants.jpg', 1, (SELECT id FROM [Category] WHERE [name] = 'Baby Clothes'), 0, 50, 0);
