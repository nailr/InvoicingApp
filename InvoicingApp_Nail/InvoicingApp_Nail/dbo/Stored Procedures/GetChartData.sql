CREATE PROCEDURE [dbo].[GetChartData]
(
	@From DATE,
	@To DATE
)
AS
SELECT SUM(AmountSum) AS Amount, ChargeSymbol, StratDate, EndDate 
FROM vw_InvoiceChart 
WHERE StratDate >= @From AND EndDate <=  @To
GROUP BY ChargeSymbol, StratDate, EndDate