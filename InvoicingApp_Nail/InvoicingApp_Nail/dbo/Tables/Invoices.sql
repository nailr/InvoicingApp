CREATE TABLE [dbo].[Invoices] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [InvoiceNumber] NVARCHAR (400)  NOT NULL,
    [ClientId]      INT             NOT NULL,
    [InvoiceDate]   DATETIME        NULL,
    [StratDate]     DATETIME        NULL,
    [EndDate]       DATETIME        NULL,
    [ChargeId]      INT             NOT NULL,
    [Rate]          DECIMAL (18, 2) NULL,
    [Units]         INT             NULL,
    [Amount]        DECIMAL (18, 2) NULL,
    [Tax]           DECIMAL (18, 2) NULL,
    [Total]         DECIMAL (18, 2) NULL,
    CONSTRAINT [PK_dbo.Invoices] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Invoices_dbo.Charges_ChargeId] FOREIGN KEY ([ChargeId]) REFERENCES [dbo].[Charges] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.Invoices_dbo.Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [dbo].[Clients] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ChargeId]
    ON [dbo].[Invoices]([ChargeId] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_InvoiceNumberAndClientId]
    ON [dbo].[Invoices]([InvoiceNumber] ASC, [ClientId] ASC);

