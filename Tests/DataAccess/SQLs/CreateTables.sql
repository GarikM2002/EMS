CREATE TABLE [dbo].[Employees] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [FirstName]   VARCHAR (50)  NOT NULL,
    [LastName]    VARCHAR (50)  NOT NULL,
    [Email]       VARCHAR (100) NOT NULL,
    [PhoneNumber] VARCHAR (20)  NULL,
    [Department]  VARCHAR (50)  NULL,
	[IsDeleted] Bit default 0 NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Email] ASC)
);

CREATE TABLE [dbo].[Employers] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]    NVARCHAR (50)  NOT NULL,
    [LastName]     NVARCHAR (50)  NOT NULL,
    [Email]        NVARCHAR (100) NOT NULL,
    [PhoneNumber]  NVARCHAR (20)  NULL,
    [Department]   NVARCHAR (50)  NULL,
    [PasswordHash] NVARCHAR (200) NOT NULL,
    [PasswordSalt] NVARCHAR (200) NOT NULL,
	[IsDeleted] Bit default 0 NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Email] ASC),
    CONSTRAINT [unique_email] UNIQUE NONCLUSTERED ([Email] ASC)
);

CREATE TABLE [dbo].[EmployeeEmployers] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [EmployeeId] INT NULL,
    [EmployerId] INT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employees] ([Id]),
    FOREIGN KEY ([EmployerId]) REFERENCES [dbo].[Employers] ([Id])
);

CREATE TABLE [dbo].[ContractTypes] (
    [ID] INT IDENTITY(1, 1) NOT NULL,
    [Name] NVARCHAR(50) NOT NULL,
    [Description] NVARCHAR(4000) NULL,
    CONSTRAINT PK_ContractTypes_ID PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT UQ_ContractTypes_Name UNIQUE ([Name])
);

CREATE TABLE [dbo].[Contracts] (
    [Id]                  INT             IDENTITY (1, 1) NOT NULL,
    [ContractTypeId]      INT             NOT NULL,
    [StartDate]           DATE            NOT NULL,
    [EndDate]             DATE            NULL,
    [Salary]              DECIMAL (18, 2) NOT NULL,
    [EmployeeEmployersId] INT             NOT NULL,
    [Description]         NVARCHAR (4000) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([EmployeeEmployersId]) REFERENCES [dbo].[EmployeeEmployers] ([Id]),
    FOREIGN KEY ([ContractTypeId]) REFERENCES [dbo].[ContractTypes] ([Id])    
);

INSERT INTO [dbo].[Employees] ([FirstName], [LastName], [Email], [PhoneNumber], [Department], [IsDeleted])
VALUES 
('John', 'Doe', 'john.doe@example.com', '1234567890', 'HR', 0),
('Jane', 'Smith', 'jane.smith@example.com', '0987654321', 'Finance', 0),
('John', 'Doe', 'john.deleted@example.com', '1213567890', 'HR', 1);

INSERT INTO [dbo].[Employers] ([FirstName], [LastName], [Email], [PhoneNumber], [Department], [PasswordHash], [PasswordSalt], [IsDeleted])
VALUES 
('Alice', 'Johnson', 'alice.johnson@example.com', '1122334455', 'IT', 'hash1', 'salt1', 0),
('Bob', 'Brown', 'bob.brown@example.com', '2233445566', 'Marketing', 'hash2', 'salt2', 0),
('Alice', 'Johnson', 'alice.johnsonDeleted@example.com', '1122334455', 'IT', 'hash1', 'salt1', 1);

INSERT INTO [dbo].[EmployeeEmployers] ([EmployeeId], [EmployerId])
VALUES 
(1, 1),
(2, 1),
(2, 2);

INSERT INTO [dbo].[ContractTypes] ([Name], [Description])
VALUES 
('Full-Time', 'Full-time employment contract with benefits'),
('Part-Time', 'Part-time employment contract without benefits'),
('Temporary', 'Temporary contract for a specific duration'),
('Internship', 'Internship contract for students or trainees'),
('Freelance', 'Freelance contract for independent contractors');

INSERT INTO [dbo].[Contracts] ([ContractTypeId], [StartDate], [EndDate], [Salary], [EmployeeEmployersId], [Description])
VALUES 
(1, '2023-01-01', '2024-01-01', 60000, 1, 'for John Doe with Alice Johnson'),
(2, '2023-02-01', NULL, 30000, 3, 'for Jane Smith with Bob Brown about doing some thing'),
(2, '2023-02-01', NULL, 30000, 2, 'for Jane Smith with Bob Brown');
