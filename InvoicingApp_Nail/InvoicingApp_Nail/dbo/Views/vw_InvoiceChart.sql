

CREATE VIEW [dbo].[vw_InvoiceChart]
AS
SELECT SUM(I.Amount) AS AmountSum, I.StratDate, I.EndDate, C.ChargeSymbol
FROM Invoices AS I 
		INNER JOIN Charges AS C ON I.ChargeId = C.Id 
GROUP BY I.StratDate, I.EndDate, C.ChargeSymbol