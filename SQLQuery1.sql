use PrimeBidDB;

CREATE TABLE Profiles (
    Id INT PRIMARY KEY IDENTITY,
    FullName NVARCHAR(MAX),
    Email NVARCHAR(MAX),
    Address NVARCHAR(MAX),
    PhoneNumber NVARCHAR(MAX),
    ProfilePicture NVARCHAR(MAX),
    PasswordHash NVARCHAR(MAX),
    Salt NVARCHAR(MAX)
);

CREATE TABLE Items (
    Id INT PRIMARY KEY IDENTITY,
    ItemName NVARCHAR(MAX),
    ItemImage NVARCHAR(MAX),
    EstimatedBid NVARCHAR(MAX),
    ItemDescription NVARCHAR(MAX),
    EndDate DATETIME DEFAULT '0001-01-01',
    Price DECIMAL(18, 2),
    Category NVARCHAR(MAX)
);

CREATE TABLE AuctionItems (
    Id INT PRIMARY KEY IDENTITY,
    ProductName NVARCHAR(MAX) NOT NULL,
    Category NVARCHAR(MAX) NOT NULL,
    ItemDescription NVARCHAR(MAX),
    StartingBid DECIMAL(18, 2) NOT NULL CHECK (StartingBid > 0),
    AuctionEndDate DATETIME,
    AdditionalTerms NVARCHAR(MAX),
    ImageUrl NVARCHAR(MAX)
);

CREATE TABLE BidHistories (
    Id INT PRIMARY KEY IDENTITY,
    UserId INT,
    ItemId INT,
    ItemName NVARCHAR(MAX),
    BidAmount DECIMAL(18, 2),
    BidDate DATETIME,
    ItemImage NVARCHAR(MAX),
    ContactName NVARCHAR(MAX),
    ContactNumber NVARCHAR(MAX),
    Address NVARCHAR(MAX),
    ZipCode NVARCHAR(MAX)
);

CREATE TABLE Payments (
    Id INT PRIMARY KEY IDENTITY,
    ContactName NVARCHAR(MAX),
    Mobile NVARCHAR(MAX),
    Address NVARCHAR(MAX),
    ZipAddress NVARCHAR(MAX),
    TotalAmount DECIMAL(18, 2) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Watchlists (
    Id INT PRIMARY KEY IDENTITY,
    UserId INT,
    ItemName NVARCHAR(MAX),
    CurrentPrice DECIMAL(18, 2) NOT NULL,
    EndDate DATETIME,
    ItemImage NVARCHAR(MAX)
);
