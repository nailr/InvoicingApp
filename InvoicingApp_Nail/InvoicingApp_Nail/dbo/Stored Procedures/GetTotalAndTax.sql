

CREATE PROCEDURE GetTotalAndTax
(
	@ClientId int
)
AS
SELECT SUM(Tax) AS Tax, SUM(Total) AS Total FROM vw_InvoicePrint WHERE CompanyId = @ClientId