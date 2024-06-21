-- Drop and create the Employees table
IF OBJECT_ID('Employees', 'U') IS NOT NULL DROP TABLE Employees;
CREATE TABLE [dbo].[Employees] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [FirstName]   VARCHAR (50)  NOT NULL,
    [LastName]    VARCHAR (50)  NOT NULL,
    [Email]       VARCHAR (100) NOT NULL,
    [PhoneNumber] VARCHAR (20)  NULL,
    [Department]  VARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Email] ASC)
);

-- Drop and create the Employers table
IF OBJECT_ID('Employers', 'U') IS NOT NULL DROP TABLE Employers;
CREATE TABLE [dbo].[Employers] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]    NVARCHAR (50)  NOT NULL,
    [LastName]     NVARCHAR (50)  NOT NULL,
    [Email]        NVARCHAR (100) NOT NULL,
    [PhoneNumber]  NVARCHAR (20)  NULL,
    [Department]   NVARCHAR (50)  NULL,
    [PasswordHash] NVARCHAR (200) NOT NULL,
    [PasswordSalt] NVARCHAR (200) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Email] ASC),
    CONSTRAINT [unique_email] UNIQUE NONCLUSTERED ([Email] ASC)
);

-- Drop and create the EmployeeEmployers table
IF OBJECT_ID('EmployeeEmployers', 'U') IS NOT NULL DROP TABLE EmployeeEmployers;
CREATE TABLE [dbo].[EmployeeEmployers] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [EmployeeId] INT NULL,
    [EmployerId] INT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employees] ([Id]),
    FOREIGN KEY ([EmployerId]) REFERENCES [dbo].[Employers] ([Id])
);

-- Drop and create the Contracts table
IF OBJECT_ID('Contracts', 'U') IS NOT NULL DROP TABLE Contracts;
CREATE TABLE [dbo].[Contracts] (
    [Id]                  INT             IDENTITY (1, 1) NOT NULL,
    [ContractType]        NVARCHAR (50)   NOT NULL,
    [StartDate]           DATE            NOT NULL,
    [EndDate]             DATE            NULL,
    [Salary]              DECIMAL (18, 2) NOT NULL,
    [EmployeeEmployersId] INT             NOT NULL,
    [Description]         NVARCHAR (4000) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([EmployeeEmployersId]) REFERENCES [dbo].[EmployeeEmployers] ([Id])
);

-- Insert seed data into Employees
INSERT INTO [dbo].[Employees] ([FirstName], [LastName], [Email], [PhoneNumber], [Department])
VALUES 
('John', 'Doe', 'john.doe@example.com', '1234567890', 'HR'),
('Jane', 'Smith', 'jane.smith@example.com', '0987654321', 'Finance');

-- Insert seed data into Employers
INSERT INTO [dbo].[Employers] ([FirstName], [LastName], [Email], [PhoneNumber], [Department], [PasswordHash], [PasswordSalt])
VALUES 
('Alice', 'Johnson', 'alice.johnson@example.com', '1122334455', 'IT', 'hash1', 'salt1'),
('Bob', 'Brown', 'bob.brown@example.com', '2233445566', 'Marketing', 'hash2', 'salt2');

-- Insert seed data into EmployeeEmployers
INSERT INTO [dbo].[EmployeeEmployers] ([EmployeeId], [EmployerId])
VALUES 
(1, 1),
(2, 2);

-- Insert seed data into Contracts
INSERT INTO [dbo].[Contracts] ([ContractType], [StartDate], [EndDate], [Salary], [EmployeeEmployersId], [Description])
VALUES 
('Full-Time', '2023-01-01', '2024-01-01', 60000, 1, 'Full-time employment contract for John Doe with Alice Johnson'),
('Part-Time', '2023-02-01', NULL, 30000, 2, 'Part-time employment contract for Jane Smith with Bob Brown');
