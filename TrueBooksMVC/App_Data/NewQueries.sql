-- sep 21
alter table RevenueType add TaxPercentage decimal(5,2) null

-- sep 25

/****** Object:  StoredProcedure [dbo].[SP_GetChargesbyJobIDandUser]    Script Date: 25-09-2019 15:15:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Proc [dbo].[SP_GetChargesbyJobIDandUser]
(
@JobId int,
@UserID int
)
AS
Begin

select JI.*,RT.RevenueType,CM.CurrencyName as ProvisionCurrency,CM1.CurrencyName as SalesCurrency,
S.SupplierName, IU.ItemUnit,IU.ItemUnitID,RT.RevenueType as RevenueTypeName 
from dbo.JInvoice JI
left join RevenueType RT on JI.RevenueTypeID = RT.RevenueTypeID
left join CurrencyMaster CM on JI.ProvisionCurrencyID = CM.CurrencyID
left join CurrencyMaster CM1 on JI.SalesCurrencyID = CM1.CurrencyID
left join Supplier S on JI.SupplierID = S.SupplierID
left join ItemUnit IU on JI.UnitID = IU.ItemUnitID
where JI.JobID=@JobId

End

GO
--------------

alter table JInvoice add Tax decimal(5,2);
alter table JInvoice add TaxAmount decimal(15,2);
alter table  JInvoice add Margin decimal(15,2);

--------------------

/****** Object:  StoredProcedure [dbo].[SP_InsertCharges]    Script Date: 25-09-2019 15:57:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Proc [dbo].[SP_InsertCharges]
(
			@JobID  int
           ,@RevenueTypeID  int
           ,@ProvisionCurrencyID  int
           ,@ProvisionExchangeRate  money
           ,@ProvisionForeign  money
           ,@ProvisionHome  money
           ,@SalesCurrencyID  int
           ,@SalesExchangeRate  money
           ,@SalesForeign  money
           ,@SalesHome money
           ,@Cost money
           ,@SupplierID  int
           ,@RevenueCode  nvarchar(200)
         
           ,@Quantity  float
           ,@UnitID  int
           ,@ProvisionRate money
           ,@SalesRate  money
           ,@AmtInWords  nvarchar(750)
           ,@InvoiceStatus  char(2)=null
           ,@CostUpdationStatus  char(2)=null
           ,@UserID int
		   ,@Description nvarchar(200)
		   ,@Tax  numeric(5,2)
		   ,@TaxAmount  numeric(15,2)
		   ,@Margin  numeric(15,2)
)
AS

Begin
Declare @MAxInvoideID int
SET @MAxInvoideID=(Select MAX(InvoiceID) from [JInvoice])+1
 
if(@MAxInvoideID is NULL)
Begin
set @MAxInvoideID=1
End

 



INSERT INTO [JInvoice]
           (InvoiceID,[JobID]
           ,[RevenueTypeID]
           ,[ProvisionCurrencyID]
           ,[ProvisionExchangeRate]
           ,[ProvisionForeign]
           ,[ProvisionHome]
           ,[SalesCurrencyID]
           ,[SalesExchangeRate]
           ,[SalesForeign]
           ,[SalesHome]
           ,[Cost]
           ,[SupplierID]
           ,[RevenueCode]
 
           ,[Quantity]
           ,[UnitID]
           ,[ProvisionRate]
           ,[SalesRate]
           ,[AmtInWords]
           ,[InvoiceStatus]
           ,[CostUpdationStatus]
           ,[UserID]
		   ,[Description]
		   ,[Tax]
		   ,[TaxAmount]
		   ,[Margin])
     VALUES
           (@MAxInvoideID
		    ,@JobID   
           ,@RevenueTypeID   
           ,@ProvisionCurrencyID   
           ,@ProvisionExchangeRate   
           ,@ProvisionForeign   
           ,@ProvisionHome   
           ,@SalesCurrencyID   
           ,@SalesExchangeRate   
           ,@SalesForeign   
           ,@SalesHome  
           ,@Cost  
           ,@SupplierID   
           ,@RevenueCode  
           ,@Quantity  
           ,@UnitID   
           ,@ProvisionRate  
           ,@SalesRate   
           ,@AmtInWords 
           ,@InvoiceStatus  
           ,@CostUpdationStatus 
           ,@UserID
		   ,@Description
		   ,@Tax
		   ,@TaxAmount
		   ,@Margin )
End



/****** Object:  StoredProcedure [dbo].[SP_GetChargesbyJobIDandUser]    Script Date: 25-09-2019 19:47:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Proc [dbo].[SP_GetChargesbyJobIDandUser]
(
@JobId int,
@UserID int
)
AS
/************
EXEC SP_GetChargesbyJobIDandUser @JobId=31146,@UserID =0
**************/
Begin

select JI.*,RT.RevenueType,CM.CurrencyName as ProvisionCurrency,CM1.CurrencyName as SalesCurrency,
S.SupplierName, IU.ItemUnit,IU.ItemUnitID 
from dbo.JInvoice JI
left join RevenueType RT on JI.RevenueTypeID = RT.RevenueTypeID
left join CurrencyMaster CM on JI.ProvisionCurrencyID = CM.CurrencyID
left join CurrencyMaster CM1 on JI.SalesCurrencyID = CM1.CurrencyID
left join Supplier S on JI.SupplierID = S.SupplierID
left join ItemUnit IU on JI.UnitID = IU.ItemUnitID
where JI.JobID=@JobId

End

--------------------oct-10 ram


/****** Object:  StoredProcedure [dbo].[SP_InsertJob]    Script Date: 04-10-2019 15:53:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[SP_InsertSalesInvoice]
(
@SalesInvoiceId int=0,
@SalesInvoiceNo nvarchar(50),
@SalesInvoiceDate datetime,
@Reference nvarchar(50),
@LPOReference nvarchar(50),
@CustomerID int,
@EmployeeeID int,
@QuotationID int,
@CurrencyID int,
@EXchangeRate decimal,
@CreditDays int,
@DueDate datetime,
@AcJouranalID int,
@BranchID int,
@Discount decimal,
@StatusDiscountAmt bit,
@OtherCharges decimal,
@PaymentTerm nvarchar,
@Remarks nvarchar,
@FYearID int
)
AS
Begin
--Declare @MaxJObID int

 INSERT INTO [SalesInvoice] 
           ([SalesInvoiceNo],
		   [SalesInvoiceDate],
		     [Reference],
			   [LPOReference],
			   [CustomerID],
			   [EmployeeID],
			   [QuotationID],
			   [CurrencyID],
			   [ExchangeRate],
			    [CreditDays],
			   [DueDate],
			   [AcJournalID],
			   [BranchID],
			   [Discount],
			   [StatusDiscountAmt],
			   [OtherCharges],
			   [PaymentTerm],
			   [Remarks],
			   [FYearID]
			
         )
     VALUES
           (
          @SalesInvoiceNo, 
@SalesInvoiceDate,
@Reference,
@LPOReference,
@CustomerID,
@EmployeeeID,
@QuotationID,
@CurrencyID,
@EXchangeRate,
@CreditDays,
@DueDate,
@AcJouranalID,
@BranchID,
@Discount,
@StatusDiscountAmt,
@OtherCharges,
@PaymentTerm,
@Remarks,
@FYearID
         )

END
GO


-----oct10 ram



/****** Object:  StoredProcedure [dbo].[SP_InsertJob]    Script Date: 04-10-2019 15:53:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[SP_InsertPurchaseInvoice]
(
@PurchaseInvoiceId int=0,
@PurchaseInvoiceNo nvarchar(50),
@PurchaseInvoiceDate datetime,
@Reference nvarchar(50),
@LPOReference nvarchar(50),
@SupplierID int,
@EmployeeeID int,
@QuotationID int,
@CurrencyID int,
@ExchangeRate decimal,
@CreditDays int,
@DueDate datetime,
@AcJouranalID int,
@BranchID int,
@Discount decimal,
@StatusDiscountAmt bit,
@OtherCharges decimal,
@PaymentTerm nvarchar,
@Remarks nvarchar,
@FYearID int
)
AS
Begin
--Declare @MaxJObID int

 INSERT INTO [PurchaseInvoice] 
           ([PurchaseInvoiceNo],
		   [PurchaseInvoiceDate],
		     [Reference],
			   [LPOReference],
			   [SupplierID],
			   [EmployeeID],
			   [QuotationID],
			   [CurrencyID],
			   [ExchangeRate],
			    [CreditDays],
			   [DueDate],
			   [AcJournalID],
			   [BranchID],
			   [Discount],
			   [StatusDiscountAmt],
			   [OtherCharges],
			   [PaymentTerm],
			   [Remarks],
			   [FYearID]
			
         )
     VALUES
           (
          @PurchaseInvoiceNo, 
@PurchaseInvoiceDate,
@Reference,
@LPOReference,
@SupplierID,
@EmployeeeID,
@QuotationID,
@CurrencyID,
@EXchangeRate,
@CreditDays,
@DueDate,
@AcJouranalID,
@BranchID,
@Discount,
@StatusDiscountAmt,
@OtherCharges,
@PaymentTerm,
@Remarks,
@FYearID
         )

END
GO


-----------------------
USE [DB_9F57C4_ShippingTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertSalesInvoice]    Script Date: 10-10-2019 17:36:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[SP_InsertSalesInvoice]
(
@SalesInvoiceId int=0,
@SalesInvoiceNo nvarchar(50),
@SalesInvoiceDate datetime,
@Reference nvarchar(50),
@LPOReference nvarchar(50),
@CustomerID int,
@EmployeeeID int,
@QuotationID int,
@CurrencyID int,
@EXchangeRate decimal,
@CreditDays int,
@DueDate datetime,
@AcJouranalID int,
@BranchID int,
@Discount decimal,
@StatusDiscountAmt bit,
@OtherCharges decimal,
@PaymentTerm nvarchar,
@Remarks nvarchar,
@FYearID int
)
AS
Begin
--Declare @MaxJObID int

 INSERT INTO [SalesInvoice] 
           ([SalesInvoiceNo],
		   [SalesInvoiceDate],
		     [Reference],
			   [LPOReference],
			   [CustomerID],
			   [EmployeeID],
			   [QuotationID],
			   [CurrencyID],
			   [ExchangeRate],
			    [CreditDays],
			   [DueDate],
			   [AcJournalID],
			   [BranchID],
			   [Discount],
			   [StatusDiscountAmt],
			   [OtherCharges],
			   [PaymentTerm],
			   [Remarks],
			   [FYearID]
			
         )
     VALUES
           (
          @SalesInvoiceNo, 
@SalesInvoiceDate,
@Reference,
@LPOReference,
@CustomerID,
@EmployeeeID,
@QuotationID,
@CurrencyID,
@EXchangeRate,
@CreditDays,
@DueDate,
@AcJouranalID,
@BranchID,
@Discount,
@StatusDiscountAmt,
@OtherCharges,
@PaymentTerm,
@Remarks,
@FYearID
         )

END

----------

USE [DB_9F57C4_ShippingTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateJob]    Script Date: 10-10-2019 19:29:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[SP_UpdatePurchaseInvoice]
(
@PurchaseInvoiceId int=0,
@PurchaseInvoiceNo nvarchar(50),
@PurchaseInvoiceDate datetime,
@Reference nvarchar(50),
@LPOReference nvarchar(50),
@SupplierID int,
@EmployeeeID int,
@QuotationID int,
@CurrencyID int,
@ExchangeRate decimal,
@CreditDays int,
@DueDate datetime,
@AcJouranalID int,
@BranchID int,
@Discount decimal,
@StatusDiscountAmt bit,
@OtherCharges decimal,
@PaymentTerm nvarchar,
@Remarks nvarchar,
@FYearID int
)
AS
Begin

UPDATE [PurchaseInvoice]  SET 
         [PurchaseInvoiceNo]=@PurchaseInvoiceNo,
		   [PurchaseInvoiceDate]=@PurchaseInvoiceDate,
		     [Reference]=@Reference,
			   [LPOReference]=@LPOReference,
			   [SupplierID]=@SupplierID,
			   [EmployeeID]=@EmployeeeID,
			   [QuotationID]=@QuotationID,
			   [CurrencyID]=@CurrencyID,
			   [ExchangeRate]=@ExchangeRate,
			    [CreditDays]=@CreditDays,
			   [DueDate]=@DueDate,
			   [AcJournalID]=@AcJouranalID,
			   [BranchID]=@BranchID,
			   [Discount]=@Discount,
			   [StatusDiscountAmt]=@StatusDiscountAmt,
			   [OtherCharges]=@OtherCharges,
			   [PaymentTerm]=@PaymentTerm,
			   [Remarks]=@Remarks,
			   [FYearID]=@FYearID
		WHERE PurchaseInvoiceID=@PurchaseInvoiceId
  
END

---------------------

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[SP_DeletePurchaseInvoice]
(
@PurchaseInvoiceId int
)
AS
BEGIN
Delete from PurchaseInvoice where PurchaseInvoiceID=@PurchaseInvoiceId

END
-----------------------------

USE [DB_9F57C4_ShippingTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdatePurchaseInvoice]    Script Date: 11-10-2019 17:52:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[SP_UpdateSalesInvoice]
(
@SalesInvoiceId int=0,
@SalesInvoiceNo nvarchar(50),
@SalesInvoiceDate datetime,
@Reference nvarchar(50),
@LPOReference nvarchar(50),
@CustomerID int,
@EmployeeeID int,
@QuotationID int,
@CurrencyID int,
@EXchangeRate decimal,
@CreditDays int,
@DueDate datetime,
@AcJouranalID int,
@BranchID int,
@Discount decimal,
@StatusDiscountAmt bit,
@OtherCharges decimal,
@PaymentTerm nvarchar,
@Remarks nvarchar,
@FYearID int
)
AS
Begin

UPDATE [SalesInvoice]  SET 
         [SalesInvoiceNo]=@SalesInvoiceNo,
		   [SalesInvoiceDate]=@SalesInvoiceDate,
		     [Reference]=@Reference,
			   [LPOReference]=@LPOReference,
			   [CustomerID]=@CustomerID,
			   [EmployeeID]=@EmployeeeID,
			   [QuotationID]=@QuotationID,
			   [CurrencyID]=@CurrencyID,
			   [ExchangeRate]=@ExchangeRate,
			    [CreditDays]=@CreditDays,
			   [DueDate]=@DueDate,
			   [AcJournalID]=@AcJouranalID,
			   [BranchID]=@BranchID,
			   [Discount]=@Discount,
			   [StatusDiscountAmt]=@StatusDiscountAmt,
			   [OtherCharges]=@OtherCharges,
			   [PaymentTerm]=@PaymentTerm,
			   [Remarks]=@Remarks,
			   [FYearID]=@FYearID
		WHERE SalesInvoiceID=@SalesInvoiceId
  
END
