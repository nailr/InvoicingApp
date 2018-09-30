CREATE TABLE [dbo].[Clients] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [CompanyName]       NVARCHAR (450) NOT NULL,
    [City]              NVARCHAR (MAX) NULL,
    [Address]           NVARCHAR (MAX) NULL,
    [Phone]             NVARCHAR (MAX) NULL,
    [Email]             NVARCHAR (MAX) NOT NULL,
    [ContactPerson]     NVARCHAR (MAX) NULL,
    [ContractDateStart] DATETIME       NULL,
    [ContractDateEnd]   DATETIME       NULL,
    [Status]            BIT            NOT NULL,
    [Contract]          NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Clients] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_CompanyName]
    ON [dbo].[Clients]([CompanyName] ASC);

