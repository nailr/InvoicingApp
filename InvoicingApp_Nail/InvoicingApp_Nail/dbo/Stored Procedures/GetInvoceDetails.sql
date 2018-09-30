CREATE PROCEDURE [dbo].[GetInvoceDetails]
(
	@ClientId int
)
AS
SELECT * FROM vw_InvoicePrint WHERE CompanyId = @ClientId