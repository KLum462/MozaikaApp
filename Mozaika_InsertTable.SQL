-- Убедитесь, что используется правильная база данных
-- USE MosaicDB;
-- GO

-- =============================================
-- 1. Заполнение независимых таблиц (Справочники, Контрагенты)
-- =============================================

-- Таблица: Roles
INSERT INTO Roles (RoleName) VALUES
(N'Администратор'),            -- ID 1
(N'Менеджер по продажам'),     -- ID 2
(N'Мастер производства'),      -- ID 3
(N'Кладовщик'),                -- ID 4
(N'Аналитик');                 -- ID 5
GO

-- Таблица: ProductCategories
INSERT INTO ProductCategories (CategoryName) VALUES
(N'Настенная плитка'),         -- ID 1
(N'Напольная плитка'),         -- ID 2
(N'Керамогранит');             -- ID 3
GO

-- Таблица: Suppliers (Поставщики сырья)
INSERT INTO Suppliers (CompanyName, ContactPerson, Phone, Email, Address) VALUES
(N'ООО "ГлинаРесурс"', N'Алексей Иванов', '+74951002030', 'info@clayres.ru', N'г. Москва, ул. Промышленная, 1'), -- ID 1
(N'ЗАО "ХимПигмент"', N'Елена Петрова', '+78125006070', 'sales@himpigment.ru', N'г. Санкт-Петербург, Химический пер., 5'); -- ID 2
GO

-- Таблица: Partners (Партнеры/Дилеры)
INSERT INTO Partners (CompanyName, ContactPerson, Phone, Email, Rating, DiscountPercent) VALUES
(N'ИП Смирнов К.А.', N'Константин Смирнов', '+79101234567', 'smirnov@mail.ru', 150, 5), -- ID 1 (Скидка 5%)
(N'ООО "СтройМаркет"', N'Ольга Новикова', '+74997654321', 'zakupki@stroymarket.ru', 80, 3), -- ID 2 (Скидка 3%)
(N'Сеть "Интерьер Дизайн"', N'Виктор Орлов', '+78005553535', 'info@interier.ru', 20, 0); -- ID 3 (Скидка 0%)
GO

-- =============================================
-- 2. Заполнение зависимых таблиц (Уровень 1)
-- =============================================

-- Таблица: Employees
-- RoleID: 1=Админ, 2=Менеджер, 3=Мастер, 4=Кладовщик
INSERT INTO Employees (FullName, RoleID, CardNumber, Login, Password) VALUES
(N'Козлов Дмитрий Сергеевич', 1, 'CARD001', 'admin', 'adminpass'),        -- ID 1
(N'Ветрова Анна Ивановна', 2, 'CARD002', 'vetrova_a', 'managerpass'),   -- ID 2 (Менеджер)
(N'Семенов Игорь Петрович', 3, 'CARD003', 'semenov_i', 'masterpass'),   -- ID 3
(N'Зайцев Олег Викторович', 4, 'CARD004', 'zaytsev_o', 'storepass');    -- ID 4
GO

-- Таблица: Materials
-- SupplierID: 1=ГлинаРесурс, 2=ХимПигмент
-- QuantityInStock использует точность DECIMAL(18, 3) согласно диаграмме
INSERT INTO Materials (MaterialName, Unit, QuantityInStock, CostPerUnit, SupplierID) VALUES
(N'Белая глина (Каолин)', N'кг', 5000.000, 25.50, 1),      -- ID 1
(N'Кварцевый песок', N'кг', 3000.000, 10.00, 1),           -- ID 2
(N'Глазурь прозрачная', N'л', 850.500, 150.00, 2),         -- ID 3
(N'Пигмент Серый', N'кг', 150.000, 320.00, 2);             -- ID 4
GO

-- Таблица: Products
-- CategoryID: 1=Настенная, 2=Напольная, 3=Керамогранит
-- MinCost используется как базовая цена реализации для примера
INSERT INTO Products (ProductName, CategoryID, MinCost, StandardProductionTime) VALUES
(N'Плитка "Арктика Белая" 30x60', 1, 450.00, 120),   -- ID 1
(N'Плитка "Дуб Сонома" 45x45', 2, 680.50, 150),      -- ID 2
(N'Керамогранит "Лофт Грей" 60x60', 3, 990.00, 200); -- ID 3
GO

-- =============================================
-- 3. Заполнение зависимых таблиц (Уровень 2 и 3)
-- =============================================

-- Таблица: ProductMaterials (Спецификация/BOM)
INSERT INTO ProductMaterials (ProductID, MaterialID, QuantityRequired) VALUES
(1, 1, 0.500), -- Арктика: Глина
(1, 3, 0.050), -- Арктика: Глазурь
(3, 1, 0.400), -- Лофт Грей: Глина
(3, 2, 0.100), -- Лофт Грей: Песок
(3, 3, 0.060), -- Лофт Грей: Глазурь
(3, 4, 0.010); -- Лофт Грей: Пигмент
GO

-- Таблица: MaterialHistory
-- Кладовщик (ID 4) и Мастер (ID 3). Используем GETDATE() для динамических дат.
INSERT INTO MaterialHistory (MaterialID, OperationDate, OperationType, QuantityChange, Comment, EmployeeID) VALUES
(1, DATEADD(day, -1, GETDATE()), N'Приход', 5000.000, N'Поставка по накладной №123 от ГлинаРесурс', 4),
(1, GETDATE(), N'Расход', -100.000, N'Списание в производство (Партия П-001)', 3);
GO

-- Таблица: AccessLogs
-- EmployeeID: 1=Дмитрий, 2=Анна
INSERT INTO AccessLogs (EmployeeID, EventTime, EventType) VALUES
(1, DATEADD(hour, -2, GETDATE()), N'Вход'),
(2, DATEADD(minute, -110, GETDATE()), N'Вход'),
(1, DATEADD(hour, -1, GETDATE()), N'Выход'),
(1, DATEADD(minute, -50, GETDATE()), N'Вход');
GO

-- =============================================
-- 4. Заполнение Заказов и Детализации (с обеспечением целостности суммы)
-- =============================================

-- Таблица: Orders
-- Вставляем заказы с TotalAmount = 0, обновим позже.
-- PartnerID: 1 (5%), 2 (3%). ManagerID: 2.
INSERT INTO Orders (PartnerID, ManagerID, OrderDate, Status, TotalAmount, PaymentDeadline) VALUES
(1, 2, DATEADD(hour, -1, GETDATE()), N'Ожидание оплаты', 0.00, DATEADD(day, 3, GETDATE())), -- ID 1
(2, 2, GETDATE(), N'Новый', 0.00, DATEADD(day, 3, GETDATE()));          -- ID 2
GO

-- Таблица: OrderDetails
-- Расчет цены: Базовая цена * (1 - Скидка/100)
INSERT INTO OrderDetails (OrderID, ProductID, Quantity, PricePerUnit) VALUES
-- Заказ 1 (Партнер 1, Скидка 5%)
-- Product 1 (450.00). Цена: 427.50
(1, 1, 100, 450.00 * 0.95),
-- Product 3 (990.00). Цена: 940.50
(1, 3, 10, 990.00 * 0.95),
-- Заказ 2 (Партнер 2, Скидка 3%)
-- Product 3 (990.00). Цена: 960.30
(2, 3, 20, 990.00 * 0.97);
GO

-- Обновление TotalAmount в таблице Orders на основе данных из OrderDetails
UPDATE O
SET O.TotalAmount = (
    SELECT SUM(OD.Quantity * OD.PricePerUnit)
    FROM OrderDetails OD
    WHERE OD.OrderID = O.OrderID
)
FROM Orders O;
GO

PRINT N'Тестовые данные успешно добавлены во все таблицы. Суммы заказов рассчитаны.';
GO