USE [Shipping_12_12_2016]
GO
/****** Object:  StoredProcedure [dbo].[CustStatement]    Script Date: 12/14/2016 15:16:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CustStatement]

AS
BEGIN
	SELECT     dbo.JobGeneration.JobID AS InvoiceID, dbo.JobGeneration.InvoiceToID, dbo.JobGeneration.JobCode, dbo.JobGeneration.InvoiceNo, 
                      dbo.JobGeneration.InvoiceDate, ISNULL
                          ((SELECT     SUM(SalesHome) AS AmtToBeReceived
                              FROM         dbo.JInvoice
                              WHERE     (JobID = dbo.JobGeneration.JobID)), 0) AS AmtToBeReceived, ISNULL
                          ((SELECT     SUM(Amount) AS AmtAlreadyReceived
                              FROM         dbo.RecPayDetails
                              WHERE     (InvoiceID = dbo.JobGeneration.JobID) AND (StatusInvoice = 'C')), 0) AS AmtAlreadyReceived,
                          (SELECT     TOP (1) dbo.RecPay.Remarks
                            FROM          dbo.RecPay INNER JOIN
                                                   dbo.RecPayDetails AS RecPayDetails_1 ON dbo.RecPay.RecPayID = RecPayDetails_1.RecPayID
                            WHERE      (RecPayDetails_1.InvoiceID = dbo.JobGeneration.JobID) AND (dbo.RecPay.StatusRec = 1)) AS REMARKS, 
                      dbo.JobGeneration.EmployeeID,dbo.CUSTOMER.Customer
FROM         dbo.JobGeneration INNER JOIN
                      dbo.JInvoice AS JInvoice_1 ON dbo.JobGeneration.JobID = JInvoice_1.JobID  inner join CUSTOMER on JobGeneration.InvoiceToID=CUSTOMER.CustomerID where Jobgeneration.invoiceno <> 0
GROUP BY dbo.JobGeneration.JobID, dbo.JobGeneration.InvoiceToID, dbo.JobGeneration.JobCode, dbo.JobGeneration.InvoiceNo, 
                      dbo.JobGeneration.InvoiceDate, dbo.JobGeneration.EmployeeID,CUSTOMER.Customer
END
GO
/****** Object:  StoredProcedure [dbo].[SupStatement]    Script Date: 12/14/2016 15:16:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SupStatement]
	
AS
BEGIN
	
SELECT     dbo.CostUpdation.CostUpdationID AS InvoiceID, dbo.CostUpdation.InvoiceNo, dbo.CostUpdation.InvoiceDate, dbo.CostUpdation.SupplierID, 
                      ISNULL
                          ((SELECT     SUM(Cost) AS Expr1
                              FROM         dbo.CostUpdationDetails
                              WHERE     (CostUpdationID = dbo.CostUpdation.CostUpdationID)), 0) AS AmtToBeReceived, ISNULL
                          ((SELECT     SUM(Amount) AS Expr1
                              FROM         dbo.RecPayDetails
                              WHERE     (InvoiceID = dbo.CostUpdation.CostUpdationID) AND (StatusInvoice = 'S')), 0) AS AmtAlreadyReceived, dbo.JobGeneration.JobCode,
                      dbo.CostUpdation.EmployeeID, dbo.CostUpdation.JobID,dbo.Supplier.SupplierName
FROM         dbo.CostUpdation LEFT OUTER JOIN
                      dbo.JobGeneration ON dbo.CostUpdation.JobID = dbo.JobGeneration.JobID inner join Supplier on CostUpdation.SupplierID=Supplier.SupplierID


END
GO
/****** Object:  StoredProcedure [dbo].[SupOutstanding]    Script Date: 12/14/2016 15:16:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SupOutstanding]
(
	@SupID int,
	@fromdate datetime,
	@todate datetime
	)
AS
BEGIN
Declare @Sid int
set @Sid=@SupID

if(@Sid=0)
BEGIN
	Select SUM(AmtToBeReceived) as AmtTRec,SUM(AmtAlreadyReceived) as AmtAlRec,SupplierName from qrySupplierOutstanding inner join Supplier on qrySupplierOutstanding.SupplierID=Supplier.SupplierID where (qrySupplierOutstanding.InvoiceDate>=@fromdate AND qrySupplierOutstanding.InvoiceDate<=@todate) group by qrySupplierOutstanding.SupplierID,Supplier.SupplierName
	END
	
	if(@Sid>0)
	BEGIN
	Select SUM(AmtToBeReceived) as AmtTRec,SUM(AmtAlreadyReceived) as AmtAlRec,SupplierName from qrySupplierOutstanding inner join Supplier on qrySupplierOutstanding.SupplierID=Supplier.SupplierID where (qrySupplierOutstanding.SupplierID=@SupID AND (qrySupplierOutstanding.InvoiceDate>=@fromdate AND qrySupplierOutstanding.InvoiceDate<=@todate)) group by qrySupplierOutstanding.SupplierID,Supplier.SupplierName
	END
END
GO
/****** Object:  StoredProcedure [dbo].[CustOutstanding]    Script Date: 12/14/2016 15:16:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[CustOutstanding]
(
	@CustID int,
	@fromdate datetime,
	@todate datetime
	)
AS
BEGIN
Declare @Cid int
set @Cid=@CustID

if(@Cid=0)
BEGIN
	Select SUM(AmtToBeReceived) as AmtTRec,SUM(AmtAlreadyReceived) as AmtAlRec,CUSTOMER.Customer from qryCustomerStatement inner join CUSTOMER on qryCustomerStatement.InvoiceToID=CUSTOMER.CustomerID where (qryCustomerStatement.InvoiceDate>=@fromdate AND qryCustomerStatement.InvoiceDate<=@todate) group by qryCustomerStatement.InvoiceToID,CUSTOMER.Customer
	END
	
	if(@Cid>0)
	BEGIN
	Select SUM(AmtToBeReceived) as AmtTRec,SUM(AmtAlreadyReceived) as AmtAlRec,CUSTOMER.Customer from qryCustomerStatement inner join CUSTOMER on qryCustomerStatement.InvoiceToID=CUSTOMER.CustomerID where (qryCustomerStatement.InvoiceToID=@Cid AND (qryCustomerStatement.InvoiceDate>=@fromdate AND qryCustomerStatement.InvoiceDate<=@todate)) group by qryCustomerStatement.InvoiceToID,CUSTOMER.Customer
	END
END
GO
