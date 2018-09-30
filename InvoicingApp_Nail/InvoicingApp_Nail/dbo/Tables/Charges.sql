CREATE TABLE [dbo].[Charges] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [ChargeSymbol] NVARCHAR (450) NULL,
    CONSTRAINT [PK_dbo.Charges] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ChargeSymbol]
    ON [dbo].[Charges]([ChargeSymbol] ASC);

