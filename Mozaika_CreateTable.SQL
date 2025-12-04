-- Убедитесь, что используется правильная база данных
-- USE MosaicDB;
-- GO

-- =============================================
-- 1. Создание независимых таблиц (Справочники, Контрагенты)
-- Таблицы создаются в первую очередь, так как другие таблицы ссылаются на них.
-- =============================================

-- Таблица: Роли (Roles)
CREATE TABLE Roles (
    RoleID INT IDENTITY(1,1) NOT NULL,
    RoleName NVARCHAR(50) NOT NULL, -- NN (Not Null) согласно диаграмме
    CONSTRAINT PK_Roles PRIMARY KEY (RoleID)
);
GO

-- Таблица: Категории продукции (ProductCategories)
CREATE TABLE ProductCategories (
    CategoryID INT IDENTITY(1,1) NOT NULL,
    -- На диаграмме отсутствует метка NN, поэтому поле допускает NULL
    CategoryName NVARCHAR(50) NULL,
    CONSTRAINT PK_ProductCategories PRIMARY KEY (CategoryID)
);
GO

-- Таблица: Партнеры (Partners)
CREATE TABLE Partners (
    PartnerID INT IDENTITY(1,1) NOT NULL,
    CompanyName NVARCHAR(100) NOT NULL,
    ContactPerson NVARCHAR(100) NULL,
    Phone NVARCHAR(20) NULL,
    Email NVARCHAR(100) NULL,
    Rating INT NULL,
    DiscountPercent INT NULL,
    CONSTRAINT PK_Partners PRIMARY KEY (PartnerID)
);
GO

-- Таблица: Поставщики (Suppliers)
CREATE TABLE Suppliers (
    SupplierID INT IDENTITY(1,1) NOT NULL,
    CompanyName NVARCHAR(100) NOT NULL,
    ContactPerson NVARCHAR(100) NULL,
    Phone NVARCHAR(20) NULL,
    Email NVARCHAR(100) NULL,
    Address NVARCHAR(200) NULL,
    CONSTRAINT PK_Suppliers PRIMARY KEY (SupplierID)
);
GO

-- =============================================
-- 2. Создание зависимых таблиц (Уровень 1)
-- =============================================

-- Таблица: Сотрудники (Employees)
CREATE TABLE Employees (
    EmployeeID INT IDENTITY(1,1) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    RoleID INT NOT NULL,
    CardNumber NVARCHAR(50) NULL,
    Login NVARCHAR(50) NULL,
    Password NVARCHAR(50) NULL,
    CONSTRAINT PK_Employees PRIMARY KEY (EmployeeID),
    -- Определение связи
    CONSTRAINT FK_Employees_Roles FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
);
GO

-- Таблица: Продукция (Products)
CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) NOT NULL,
    ProductName NVARCHAR(100) NOT NULL,
    CategoryID INT NOT NULL,
    MinCost DECIMAL(18, 2) NULL,
    StandardProductionTime INT NULL,
    CONSTRAINT PK_Products PRIMARY KEY (ProductID),
    -- Определение связи
    CONSTRAINT FK_Products_ProductCategories FOREIGN KEY (CategoryID) REFERENCES ProductCategories(CategoryID)
);
GO

-- Таблица: Материалы (Materials)
CREATE TABLE Materials (
    MaterialID INT IDENTITY(1,1) NOT NULL,
    MaterialName NVARCHAR(100) NOT NULL,
    Unit NVARCHAR(10) NULL,
    -- Точность (18, 3) согласно диаграмме
    QuantityInStock DECIMAL(18, 3) NULL,
    CostPerUnit DECIMAL(18, 2) NULL,
    SupplierID INT NULL,
    CONSTRAINT PK_Materials PRIMARY KEY (MaterialID),
    -- Определение связи
    CONSTRAINT FK_Materials_Suppliers FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID)
);
GO

-- =============================================
-- 3. Создание зависимых таблиц (Уровень 2 и 3, Журналы, Заказы)
-- =============================================

-- Таблица: Заказы (Orders)
CREATE TABLE Orders (
    OrderID INT IDENTITY(1,1) NOT NULL,
    PartnerID INT NOT NULL,
    ManagerID INT NULL, -- FK на Employees
    OrderDate DATETIME NULL,
    Status NVARCHAR(50) NULL,
    -- Точность (15, 2) согласно диаграмме
    TotalAmount DECIMAL(15, 2) NULL,
    PaymentDeadline DATETIME NULL,
    CONSTRAINT PK_Orders PRIMARY KEY (OrderID),
    -- Определение связей
    CONSTRAINT FK_Orders_Partners FOREIGN KEY (PartnerID) REFERENCES Partners(PartnerID),
    CONSTRAINT FK_Orders_Employees_Manager FOREIGN KEY (ManagerID) REFERENCES Employees(EmployeeID)
);
GO

-- Таблица: Детализация заказа (OrderDetails)
CREATE TABLE OrderDetails (
    DetailID INT IDENTITY(1,1) NOT NULL,
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NULL,
    PricePerUnit DECIMAL(18, 2) NULL,
    CONSTRAINT PK_OrderDetails PRIMARY KEY (DetailID),
    -- Определение связей
    CONSTRAINT FK_OrderDetails_Orders FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    CONSTRAINT FK_OrderDetails_Products FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);
GO

-- Таблица: Спецификация продукции (ProductMaterials)
-- Таблица связи Многие-ко-Многим с составным ключом
CREATE TABLE ProductMaterials (
    ProductID INT NOT NULL,
    MaterialID INT NOT NULL,
    QuantityRequired DECIMAL(18, 3) NULL,
    -- Определение составного первичного ключа
    CONSTRAINT PK_ProductMaterials PRIMARY KEY (ProductID, MaterialID),
    -- Определение связей
    CONSTRAINT FK_ProductMaterials_Products FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    CONSTRAINT FK_ProductMaterials_Materials FOREIGN KEY (MaterialID) REFERENCES Materials(MaterialID)
);
GO

-- Таблица: История движения материалов (MaterialHistory)
CREATE TABLE MaterialHistory (
    HistoryID INT IDENTITY(1,1) NOT NULL,
    MaterialID INT NOT NULL,
    OperationDate DATETIME NULL,
    OperationType NVARCHAR(50) NULL,
    QuantityChange DECIMAL(18, 3) NULL,
    Comment NVARCHAR(255) NULL,
    EmployeeID INT NULL,
    CONSTRAINT PK_MaterialHistory PRIMARY KEY (HistoryID),
    -- Определение связей
    CONSTRAINT FK_MaterialHistory_Materials FOREIGN KEY (MaterialID) REFERENCES Materials(MaterialID),
    CONSTRAINT FK_MaterialHistory_Employees FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID)
);
GO

-- Таблица: Журнал доступа (AccessLogs)
CREATE TABLE AccessLogs (
    LogID INT IDENTITY(1,1) NOT NULL,
    EmployeeID INT NOT NULL,
    EventTime DATETIME NULL,
    EventType NVARCHAR(10) NULL,
    CONSTRAINT PK_AccessLogs PRIMARY KEY (LogID),
    -- Определение связи
    CONSTRAINT FK_AccessLogs_Employees FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID)
);
GO