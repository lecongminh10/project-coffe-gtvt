CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;
DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260917150108_InitialCreate') THEN

    ALTER DATABASE CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260917150108_InitialCreate') THEN

    CREATE TABLE `Categories` (
        `CategoryId` int NOT NULL AUTO_INCREMENT,
        `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `Description` varchar(255) CHARACTER SET utf8mb4 NULL,
        `Icon` longtext CHARACTER SET utf8mb4 NULL,
        `DisplayOrder` int NOT NULL,
        CONSTRAINT `PK_Categories` PRIMARY KEY (`CategoryId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260917150108_InitialCreate') THEN

    CREATE TABLE `CoffeeTables` (
        `TableId` int NOT NULL AUTO_INCREMENT,
        `TableName` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `Capacity` int NOT NULL,
        `Area` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `Status` varchar(30) CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_CoffeeTables` PRIMARY KEY (`TableId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260917150108_InitialCreate') THEN

    CREATE TABLE `Users` (
        `UserId` int NOT NULL AUTO_INCREMENT,
        `Username` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `PasswordHash` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `FullName` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `Email` varchar(100) CHARACTER SET utf8mb4 NULL,
        `PhoneNumber` varchar(15) CHARACTER SET utf8mb4 NULL,
        `Role` longtext CHARACTER SET utf8mb4 NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        CONSTRAINT `PK_Users` PRIMARY KEY (`UserId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260917150108_InitialCreate') THEN

    CREATE TABLE `Products` (
        `ProductId` int NOT NULL AUTO_INCREMENT,
        `Name` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
        `Price` decimal(18,2) NOT NULL,
        `Description` varchar(1000) CHARACTER SET utf8mb4 NULL,
        `ImageUrl` longtext CHARACTER SET utf8mb4 NULL,
        `IsAvailable` tinyint(1) NOT NULL,
        `IsFeatured` tinyint(1) NOT NULL,
        `CategoryId` int NOT NULL,
        CONSTRAINT `PK_Products` PRIMARY KEY (`ProductId`),
        CONSTRAINT `FK_Products_Categories_CategoryId` FOREIGN KEY (`CategoryId`) REFERENCES `Categories` (`CategoryId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260917150108_InitialCreate') THEN

    CREATE TABLE `Orders` (
        `OrderId` int NOT NULL AUTO_INCREMENT,
        `OrderDate` datetime(6) NOT NULL,
        `CustomerName` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `CustomerPhone` varchar(15) CHARACTER SET utf8mb4 NULL,
        `TableId` int NULL,
        `UserId` int NULL,
        `TotalAmount` decimal(18,2) NOT NULL,
        `Status` varchar(30) CHARACTER SET utf8mb4 NOT NULL,
        `PaymentMethod` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `IsPaid` tinyint(1) NOT NULL,
        `Notes` varchar(500) CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_Orders` PRIMARY KEY (`OrderId`),
        CONSTRAINT `FK_Orders_CoffeeTables_TableId` FOREIGN KEY (`TableId`) REFERENCES `CoffeeTables` (`TableId`),
        CONSTRAINT `FK_Orders_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`UserId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260917150108_InitialCreate') THEN

    CREATE TABLE `OrderDetails` (
        `OrderDetailId` int NOT NULL AUTO_INCREMENT,
        `OrderId` int NOT NULL,
        `ProductId` int NOT NULL,
        `Quantity` int NOT NULL,
        `UnitPrice` decimal(18,2) NOT NULL,
        CONSTRAINT `PK_OrderDetails` PRIMARY KEY (`OrderDetailId`),
        CONSTRAINT `FK_OrderDetails_Orders_OrderId` FOREIGN KEY (`OrderId`) REFERENCES `Orders` (`OrderId`) ON DELETE CASCADE,
        CONSTRAINT `FK_OrderDetails_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `Products` (`ProductId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260917150108_InitialCreate') THEN

    INSERT INTO `Categories` (`CategoryId`, `Description`, `DisplayOrder`, `Icon`, `Name`)
    VALUES (1, 'Cà phê pha phin truyền thống Việt Nam đậm đà bản sắc', 1, 'fa-coffee', 'Cà Phê Truyền Thống'),
    (2, 'Espresso, Cappuccino, Latte phong cách Ý thượng hạng', 2, 'fa-mug-hot', 'Cà Phê Pha Máy (Espresso)'),
    (3, 'Trà đào cam sả, trà vải, trà sen thanh mát ngọt dịu', 3, 'fa-leaf', 'Trà & Trà Trái Cây'),
    (4, 'Matcha, Socola, Sinh tố xoài béo ngậy thơm ngon', 4, 'fa-glass-water', 'Đá Xay & Sinh Tố (Ice Blended)'),
    (5, 'Tiramisu, Croissant, Bánh phô mai nướng thơm lừng', 5, 'fa-cookie-bite', 'Bánh Ngọt & Tráng Miệng');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260917150108_InitialCreate') THEN

    INSERT INTO `CoffeeTables` (`TableId`, `Area`, `Capacity`, `Status`, `TableName`)
    VALUES (1, 'Tầng 1', 2, 'Available', 'Bàn 01'),
    (2, 'Tầng 1', 4, 'Available', 'Bàn 02'),
    (3, 'Tầng 1', 4, 'Occupied', 'Bàn 03'),
    (4, 'Tầng 1', 6, 'Available', 'Bàn 04'),
    (5, 'Tầng 2', 2, 'Available', 'Bàn 05'),
    (6, 'Tầng 2', 4, 'Reserved', 'Bàn 06'),
    (7, 'Tầng 2', 6, 'Available', 'Bàn 07'),
    (8, 'Sân Vườn', 4, 'Available', 'Bàn Sân Vườn 01'),
    (9, 'Sân Vườn', 8, 'Available', 'Bàn Sân Vườn 02');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260917150108_InitialCreate') THEN

    INSERT INTO `Users` (`UserId`, `CreatedAt`, `Email`, `FullName`, `PasswordHash`, `PhoneNumber`, `Role`, `Username`)
    VALUES (1, TIMESTAMP '2026-01-01 00:00:00', 'admin@cafe.vn', 'Quản Trị Viên', 'JAvlGPq9JyTdtvBO6x2llnRI1+gxwIyPqCKAn3THIKk=', '0901234567', 'Admin', 'admin'),
    (2, TIMESTAMP '2026-01-01 00:00:00', 'staff@cafe.vn', 'Nhân Viên Thu Ngân', 'EBdue3sk0xes/PjSBkz9LyThVPe1qWYDB31e+BPWprY=', '0908888999', 'Staff', 'staff'),
    (3, TIMESTAMP '2026-01-01 00:00:00', 'khach@gmail.com', 'Nguyễn Văn Khách', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', '0912345678', 'Customer', 'khachhang');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260917150108_InitialCreate') THEN

    INSERT INTO `Products` (`ProductId`, `CategoryId`, `Description`, `ImageUrl`, `IsAvailable`, `IsFeatured`, `Name`, `Price`)
    VALUES (1, 1, 'Robusta nguyên chất Đắk Lắk pha phin truyền thống, vị đắng thanh đậm đà.', '/images/products/ca-phe-den.jpg', TRUE, TRUE, 'Cà Phê Đen Đá', 25000.0),
    (2, 1, 'Cà phê đậm đà hòa quyện cùng sữa đặc béo ngậy, thức uống quốc dân.', '/images/products/ca-phe-sua.jpg', TRUE, TRUE, 'Cà Phê Sữa Đá', 29000.0),
    (3, 1, 'Nhiều sữa đặc và sữa tươi kèm một chút cà phê thơm dịu dàng.', '/images/products/bac-xiu.jpg', TRUE, FALSE, 'Bạc Xỉu Sài Gòn', 32000.0),
    (4, 2, 'Chiết xuất áp suất cao từ hạt Arabica Cầu Đất nguyên chất.', '/images/products/espresso.jpg', TRUE, FALSE, 'Espresso Đậm Vị', 35000.0),
    (5, 2, 'Espresso kết hợp sữa nóng và lớp bọt sữa dày mịn rắc bột cacao.', '/images/products/cappuccino.jpg', TRUE, TRUE, 'Cappuccino Ý', 45000.0),
    (6, 2, 'Sốt caramel béo ngọt quyện cùng vị đắng nhẹ của cà phê pha máy.', '/images/products/macchiato.jpg', TRUE, TRUE, 'Caramel Macchiato', 49000.0),
    (7, 3, 'Trà đen hương đào thanh mát, sả tươi ngát hương cùng miếng đào giòn ngọt.', '/images/products/tra-dao.jpg', TRUE, TRUE, 'Trà Đào Cam Sả', 39000.0),
    (8, 3, 'Trà lài thơm nức kết hợp quả vải ngâm ngọt mọng nước.', '/images/products/tra-vai.jpg', TRUE, FALSE, 'Trà Vải Hoa Nhài', 39000.0),
    (9, 4, 'Bột trà xanh Nhật Bản xay nhuyễn với đá viên và lớp kem tươi whipping cream.', '/images/products/matcha-ice.jpg', TRUE, TRUE, 'Matcha Đá Xay Kem Béo', 49000.0),
    (10, 4, 'Hương vị sô cô la đậm đà the mát cùng tinh chất bạc hà sảng khoái.', '/images/products/choco-mint.jpg', TRUE, FALSE, 'Socola Bạc Hà Đá Xay', 49000.0),
    (11, 5, 'Bánh kem phô mai mascarpone đượm vị cà phê rượu nhẹ rắc bột cacao.', '/images/products/tiramisu.jpg', TRUE, TRUE, 'Bánh Tiramisu Ý', 38000.0),
    (12, 5, 'Bánh sừng bò nướng giòn rụm với nhiều lớp bơ thơm ngát.', '/images/products/croissant.jpg', TRUE, FALSE, 'Bánh Croissant Bơ Pháp', 32000.0);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260917150108_InitialCreate') THEN

    CREATE INDEX `IX_OrderDetails_OrderId` ON `OrderDetails` (`OrderId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260917150108_InitialCreate') THEN

    CREATE INDEX `IX_OrderDetails_ProductId` ON `OrderDetails` (`ProductId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260917150108_InitialCreate') THEN

    CREATE INDEX `IX_Orders_TableId` ON `Orders` (`TableId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260917150108_InitialCreate') THEN

    CREATE INDEX `IX_Orders_UserId` ON `Orders` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260917150108_InitialCreate') THEN

    CREATE INDEX `IX_Products_CategoryId` ON `Products` (`CategoryId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260917150108_InitialCreate') THEN

    CREATE UNIQUE INDEX `IX_Users_Username` ON `Users` (`Username`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260917150108_InitialCreate') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260917150108_InitialCreate', '9.0.0');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    CREATE TABLE `News` (
        `NewsId` int NOT NULL AUTO_INCREMENT,
        `Title` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `Slug` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `Summary` varchar(500) CHARACTER SET utf8mb4 NULL,
        `Content` longtext CHARACTER SET utf8mb4 NOT NULL,
        `ImageUrl` varchar(255) CHARACTER SET utf8mb4 NULL,
        `AuthorId` int NULL,
        `PublishedAt` datetime(6) NOT NULL,
        `ViewCount` int NOT NULL,
        `IsActive` tinyint(1) NOT NULL,
        CONSTRAINT `PK_News` PRIMARY KEY (`NewsId`),
        CONSTRAINT `FK_News_Users_AuthorId` FOREIGN KEY (`AuthorId`) REFERENCES `Users` (`UserId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    CREATE TABLE `Reviews` (
        `ReviewId` int NOT NULL AUTO_INCREMENT,
        `ProductId` int NOT NULL,
        `UserId` int NULL,
        `CustomerName` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `Rating` int NOT NULL,
        `Comment` varchar(1000) CHARACTER SET utf8mb4 NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `IsApproved` tinyint(1) NOT NULL,
        CONSTRAINT `PK_Reviews` PRIMARY KEY (`ReviewId`),
        CONSTRAINT `FK_Reviews_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `Products` (`ProductId`) ON DELETE CASCADE,
        CONSTRAINT `FK_Reviews_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`UserId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    CREATE TABLE `Shifts` (
        `ShiftId` int NOT NULL AUTO_INCREMENT,
        `ShiftName` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `StartTime` time(6) NOT NULL,
        `EndTime` time(6) NOT NULL,
        `HourlyWage` decimal(18,2) NOT NULL,
        CONSTRAINT `PK_Shifts` PRIMARY KEY (`ShiftId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    CREATE TABLE `Suppliers` (
        `SupplierId` int NOT NULL AUTO_INCREMENT,
        `Name` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
        `ContactPerson` varchar(100) CHARACTER SET utf8mb4 NULL,
        `PhoneNumber` varchar(20) CHARACTER SET utf8mb4 NULL,
        `Email` varchar(100) CHARACTER SET utf8mb4 NULL,
        `Address` varchar(255) CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_Suppliers` PRIMARY KEY (`SupplierId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    CREATE TABLE `Vouchers` (
        `VoucherId` int NOT NULL AUTO_INCREMENT,
        `Code` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `DiscountPercent` int NOT NULL,
        `MaxDiscountAmount` decimal(18,2) NOT NULL,
        `MinOrderAmount` decimal(18,2) NOT NULL,
        `StartDate` datetime(6) NOT NULL,
        `EndDate` datetime(6) NOT NULL,
        `UsageLimit` int NOT NULL,
        `UsedCount` int NOT NULL,
        `IsActive` tinyint(1) NOT NULL,
        CONSTRAINT `PK_Vouchers` PRIMARY KEY (`VoucherId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    CREATE TABLE `Wishlists` (
        `WishlistId` int NOT NULL AUTO_INCREMENT,
        `UserId` int NOT NULL,
        `ProductId` int NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        CONSTRAINT `PK_Wishlists` PRIMARY KEY (`WishlistId`),
        CONSTRAINT `FK_Wishlists_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `Products` (`ProductId`) ON DELETE CASCADE,
        CONSTRAINT `FK_Wishlists_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`UserId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    CREATE TABLE `EmployeeSchedules` (
        `ScheduleId` int NOT NULL AUTO_INCREMENT,
        `UserId` int NOT NULL,
        `ShiftId` int NOT NULL,
        `WorkDate` datetime(6) NOT NULL,
        `Status` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `Note` varchar(255) CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_EmployeeSchedules` PRIMARY KEY (`ScheduleId`),
        CONSTRAINT `FK_EmployeeSchedules_Shifts_ShiftId` FOREIGN KEY (`ShiftId`) REFERENCES `Shifts` (`ShiftId`) ON DELETE CASCADE,
        CONSTRAINT `FK_EmployeeSchedules_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`UserId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    CREATE TABLE `Ingredients` (
        `IngredientId` int NOT NULL AUTO_INCREMENT,
        `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `Unit` varchar(30) CHARACTER SET utf8mb4 NOT NULL,
        `QuantityInStock` decimal(18,2) NOT NULL,
        `MinimumStock` decimal(18,2) NOT NULL,
        `UnitPrice` decimal(18,2) NOT NULL,
        `SupplierId` int NULL,
        CONSTRAINT `PK_Ingredients` PRIMARY KEY (`IngredientId`),
        CONSTRAINT `FK_Ingredients_Suppliers_SupplierId` FOREIGN KEY (`SupplierId`) REFERENCES `Suppliers` (`SupplierId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    CREATE TABLE `Recipes` (
        `RecipeId` int NOT NULL AUTO_INCREMENT,
        `ProductId` int NOT NULL,
        `IngredientId` int NOT NULL,
        `AmountNeeded` decimal(18,2) NOT NULL,
        `Unit` varchar(30) CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_Recipes` PRIMARY KEY (`RecipeId`),
        CONSTRAINT `FK_Recipes_Ingredients_IngredientId` FOREIGN KEY (`IngredientId`) REFERENCES `Ingredients` (`IngredientId`) ON DELETE CASCADE,
        CONSTRAINT `FK_Recipes_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `Products` (`ProductId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    INSERT INTO `News` (`NewsId`, `AuthorId`, `Content`, `ImageUrl`, `IsActive`, `PublishedAt`, `Slug`, `Summary`, `Title`, `ViewCount`)
    VALUES (1, 1, '<p>Cà phê phin từ lâu đã trở thành biểu tượng của sự tĩnh lặng và kiên nhẫn. Những giọt Robusta sánh đậm hòa cùng sữa đặc béo ngọt tạo nên bản hòa ca vị giác khó quên...</p>', '/images/products/ca-phe-den.jpg', TRUE, TIMESTAMP '2026-09-15 00:00:00', 'nghe-thuat-thuong-thuc-ca-phe-pha-phin', 'Từng giọt cà phê nhỏ chậm rãi qua chiếc phin nhôm không chỉ là cách chiết xuất hương vị, mà là cả một nét văn hóa sống chậm thanh tao của người Việt.', 'Nghệ Thuật Thưởng Thức Cà Phê Pha Phin Truyền Thống Việt Nam', 385),
    (2, 1, '<p>Những trái cà phê chín đỏ được người nông dân hái thủ công, sơ chế ướt và rang mộc ở mức vừa phải để lưu giữ trọn vẹn hương hoa cỏ thanh nhã...</p>', '/images/products/espresso.jpg', TRUE, TIMESTAMP '2026-09-18 00:00:00', 'hanh-trinh-hat-arabica-cau-dat', 'Nằm ở độ cao trên 1500m so với mực nước biển, Cầu Đất (Đà Lạt) được thiên nhiên ưu ái khí hậu ôn đới lý tưởng để ươm mầm những hạt Arabica thơm ngọt dịu dàng.', 'Hành Trình Hạt Arabica Từ Vùng Đất Cầu Đất Đến Tách Cà Phê Của Bạn', 290),
    (3, 1, '<p>Với mong muốn mang lại nguồn cảm hứng bất tận khi làm việc và trò chuyện, khu vực sân vườn được trang bị hệ thống phun sương mát mẻ và ổ cắm điện tiện lợi...</p>', '/images/products/tra-dao.jpg', TRUE, TIMESTAMP '2026-09-20 00:00:00', 'khai-truong-khong-gian-rooftop-san-vuon', 'Coffee Paradise chính thức ra mắt khu vực sân vườn ngoài trời ngập tràn cây xanh và góc rooftop ngắm hoàng hôn cực chill dành cho các bạn trẻ.', 'Khai Trương Không Gian Rooftop & Sân Vườn Xanh Mát', 512);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    INSERT INTO `Reviews` (`ReviewId`, `Comment`, `CreatedAt`, `CustomerName`, `IsApproved`, `ProductId`, `Rating`, `UserId`)
    VALUES (1, 'Cà phê sữa đá rất đậm đà, chuẩn vị Tây Nguyên, không bị ngọt gắt. Rất hài lòng!', TIMESTAMP '2026-09-18 00:00:00', 'Nguyễn Văn Khách', TRUE, 2, 5, 3),
    (2, 'Trà đào cam sả thơm lừng vị sả tươi, miếng đào giòn ngọt và thanh mát.', TIMESTAMP '2026-09-19 00:00:00', 'Trần Hoàng Anh', TRUE, 7, 5, 3),
    (3, 'Cappuccino bọt sữa vẽ hình rất đẹp, ấm nóng và béo mịn. Sẽ quay lại thử thêm món khác!', TIMESTAMP '2026-09-19 00:00:00', 'Lê Thu Thảo', TRUE, 5, 4, NULL),
    (4, 'Matcha đá xay thơm chuẩn matcha Nhật, kem whipping cream béo ngậy ăn cực mê!', TIMESTAMP '2026-09-20 00:00:00', 'Phạm Minh Đức', TRUE, 9, 5, NULL),
    (5, 'Bánh Tiramisu mềm mịn, đậm đà vị cà phê rượu và không bị ngấy xíu nào.', TIMESTAMP '2026-09-20 00:00:00', 'Vũ Mai Linh', TRUE, 11, 5, NULL);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    INSERT INTO `Shifts` (`ShiftId`, `EndTime`, `HourlyWage`, `ShiftName`, `StartTime`)
    VALUES (1, TIME '12:00:00', 26000.0, 'Ca Sáng (Mở Cửa & Chuẩn Bị)', TIME '07:00:00'),
    (2, TIME '17:30:00', 26000.0, 'Ca Chiều (Phục Vụ Cao Điểm)', TIME '12:00:00'),
    (3, TIME '22:30:00', 29000.0, 'Ca Tối (Chill & Dọn Dẹp)', TIME '17:30:00');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    INSERT INTO `Suppliers` (`SupplierId`, `Address`, `ContactPerson`, `Email`, `Name`, `PhoneNumber`)
    VALUES (1, 'Xã Xuân Trường, TP. Đà Lạt, Lâm Đồng', 'Nguyễn Hữu Đạt', 'dat@caudatfarm.vn', 'Nông Trại Cà Phê Cầu Đất Farm', '0987111222'),
    (2, 'Khu công nghiệp Lộc Phát, Lâm Đồng', 'Trần Thị Mai', 'mai.tt@vinamilk.com.vn', 'Công Ty Sữa Vinamilk Chi Nhánh Đà Lạt', '0988222333'),
    (3, 'Quận Tân Phú, TP. Hồ Chí Minh', 'Lê Quang Minh', 'minh@tannhathuong.com', 'Công Ty Nguyên Liệu Pha Chế Tân Nhất Hương', '0909333444'),
    (4, 'Gia Lâm, Hà Nội', 'Phạm Gia Huy', 'contact@ecocup.vn', 'Công Ty Bao Bì & Cốc Giấy Thân Thiện Eco Cup', '0918444555');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    INSERT INTO `Vouchers` (`VoucherId`, `Code`, `DiscountPercent`, `EndDate`, `IsActive`, `MaxDiscountAmount`, `MinOrderAmount`, `StartDate`, `UsageLimit`, `UsedCount`)
    VALUES (1, 'CHAOBAN', 10, TIMESTAMP '2026-12-31 00:00:00', TRUE, 30000.0, 50000.0, TIMESTAMP '2026-01-01 00:00:00', 500, 42),
    (2, 'COFFEE20', 20, TIMESTAMP '2026-12-31 00:00:00', TRUE, 50000.0, 80000.0, TIMESTAMP '2026-01-01 00:00:00', 200, 88),
    (3, 'FREESHIP', 15, TIMESTAMP '2026-12-31 00:00:00', TRUE, 25000.0, 60000.0, TIMESTAMP '2026-01-01 00:00:00', 1000, 156),
    (4, 'VIPSTUDENT', 15, TIMESTAMP '2026-12-31 00:00:00', TRUE, 40000.0, 40000.0, TIMESTAMP '2026-01-01 00:00:00', 300, 65);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    INSERT INTO `Wishlists` (`WishlistId`, `CreatedAt`, `ProductId`, `UserId`)
    VALUES (1, TIMESTAMP '2026-09-18 00:00:00', 2, 3),
    (2, TIMESTAMP '2026-09-19 00:00:00', 7, 3),
    (3, TIMESTAMP '2026-09-20 00:00:00', 5, 3);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    INSERT INTO `EmployeeSchedules` (`ScheduleId`, `Note`, `ShiftId`, `Status`, `UserId`, `WorkDate`)
    VALUES (1, 'Trực quầy thu ngân và kiểm tra quầy bánh', 1, 'Completed', 2, TIMESTAMP '2026-09-21 00:00:00'),
    (2, 'Phụ trách pha chế máy Espresso', 2, 'Scheduled', 2, TIMESTAMP '2026-09-22 00:00:00'),
    (3, 'Trực bàn và kiểm kê kho cuối ca', 3, 'Scheduled', 2, TIMESTAMP '2026-09-23 00:00:00');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    INSERT INTO `Ingredients` (`IngredientId`, `MinimumStock`, `Name`, `QuantityInStock`, `SupplierId`, `Unit`, `UnitPrice`)
    VALUES (1, 10.0, 'Hạt Cà Phê Arabica Cầu Đất', 50.0, 1, 'kg', 280000.0),
    (2, 15.0, 'Hạt Cà Phê Robusta Buôn Ma Thuột', 80.0, 1, 'kg', 160000.0),
    (3, 20.0, 'Sữa Đặc Có Đường Ông Thọ', 120.0, 2, 'hộp', 24000.0),
    (4, 15.0, 'Sữa Tươi Thanh Trùng 100%', 60.0, 2, 'lít', 35000.0),
    (5, 5.0, 'Trà Đen Hương Đào Cao Cấp', 25.0, 3, 'kg', 220000.0),
    (6, 10.0, 'Đào Miếng Ngâm Nước Đường', 45.0, 3, 'hộp', 42000.0),
    (7, 3.0, 'Bột Trà Xanh Matcha Uji Nhật Bản', 15.0, 3, 'kg', 650000.0),
    (8, 5.0, 'Sốt Caramel Torani Nhập Khẩu', 20.0, 3, 'chai', 185000.0),
    (9, 5.0, 'Bột Cacao Nguyên Chất', 18.0, 3, 'kg', 210000.0),
    (10, 300.0, 'Cốc Giấy Take-away & Nắp Sinh Học', 2000.0, 4, 'cái', 1500.0);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    INSERT INTO `Recipes` (`RecipeId`, `AmountNeeded`, `IngredientId`, `ProductId`, `Unit`)
    VALUES (1, 25.0, 2, 1, 'g'),
    (2, 25.0, 2, 2, 'g'),
    (3, 30.0, 3, 2, 'g'),
    (4, 18.0, 1, 4, 'g'),
    (5, 18.0, 1, 5, 'g'),
    (6, 150.0, 4, 5, 'ml'),
    (7, 15.0, 5, 7, 'g'),
    (8, 50.0, 6, 7, 'g'),
    (9, 10.0, 7, 9, 'g'),
    (10, 100.0, 4, 9, 'ml');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    CREATE INDEX `IX_EmployeeSchedules_ShiftId` ON `EmployeeSchedules` (`ShiftId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    CREATE INDEX `IX_EmployeeSchedules_UserId` ON `EmployeeSchedules` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    CREATE INDEX `IX_Ingredients_SupplierId` ON `Ingredients` (`SupplierId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    CREATE INDEX `IX_News_AuthorId` ON `News` (`AuthorId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    CREATE INDEX `IX_Recipes_IngredientId` ON `Recipes` (`IngredientId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    CREATE INDEX `IX_Recipes_ProductId` ON `Recipes` (`ProductId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    CREATE INDEX `IX_Reviews_ProductId` ON `Reviews` (`ProductId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    CREATE INDEX `IX_Reviews_UserId` ON `Reviews` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    CREATE UNIQUE INDEX `IX_Vouchers_Code` ON `Vouchers` (`Code`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    CREATE INDEX `IX_Wishlists_ProductId` ON `Wishlists` (`ProductId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    CREATE INDEX `IX_Wishlists_UserId` ON `Wishlists` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260921022245_AddExpandedTables') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260921022245_AddExpandedTables', '9.0.0');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

