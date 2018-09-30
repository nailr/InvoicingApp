
CREATE VIEW [dbo].[vw_InvoicePrint]
AS
SELECT I.*, C.CompanyName, C.Contract, C.Id AS CompanyId, CA.ChargeSymbol 
FROM Invoices AS I 
	INNER JOIN Clients AS C ON I.ClientId = C.Id	
		INNER JOIN Charges AS CA ON I.ChargeId = CA.Id