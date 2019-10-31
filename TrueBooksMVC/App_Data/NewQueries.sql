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


----
  ALTER TABLE UserRegistration ADD BranchId int;
  ALTER TABLE UserRegistration ADD AcFinancialYearID int;


  ---
  
/****** Object:  StoredProcedure [dbo].[SP_InsertPurchaseInvoice]    Script Date: 15-10-2019 19:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER proc [dbo].[SP_InsertPurchaseInvoice]
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
Declare @MaxId int;
SELECT @MaxId = MAX(@PurchaseInvoiceId) FROM PurchaseInvoice;

SET @PurchaseInvoiceNo = RIGHT('0000' + CAST(@MaxId AS VARCHAR(5)),5);
SET @PurchaseInvoiceNo = 'PI-' + @PurchaseInvoiceNo;

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

-------

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_GetSalesInvoiceByID]
(
@SalesInvoiceID int
)
AS
BEGIN
SELECT * FROM SalesInvoice where SalesInvoiceID = @SalesInvoiceID
END


---



/****** Object:  Table [dbo].[PurchaseInvoice]    Script Date: 16-10-2019 03:16:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PurchaseInvoice](
	[PurchaseInvoiceID] [int] IDENTITY(1,1) NOT NULL,
	[PurchaseInvoiceNo] [varchar](50) NULL,
	[PurchaseInvoiceDate] [datetime] NULL,
	[Reference] [varchar](100) NULL,
	[LPOReference] [varchar](100) NULL,
	[SupplierID] [int] NULL,
	[EmployeeID] [int] NULL,
	[QuotationID] [int] NULL,
	[CurrencyID] [int] NULL,
	[ExchangeRate] [decimal](18, 2) NULL,
	[CreditDays] [int] NULL,
	[DueDate] [datetime] NULL,
	[AcJournalID] [int] NULL,
	[BranchID] [int] NULL,
	[Discount] [decimal](18, 2) NULL,
	[StatusDiscountAmt] [bit] NULL,
	[OtherCharges] [decimal](18, 2) NULL,
	[PaymentTerm] [varchar](max) NULL,
	[Remarks] [varchar](max) NULL,
	[FYearID] [int] NULL,
 CONSTRAINT [PK_PurchaseInvoiceNew] PRIMARY KEY CLUSTERED 
(
	[PurchaseInvoiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[PurchaseInvoice] ADD  CONSTRAINT [DF_PurchaseInvoice_ExchangeRate]  DEFAULT ((0)) FOR [ExchangeRate]
GO

ALTER TABLE [dbo].[PurchaseInvoice] ADD  CONSTRAINT [DF_PurchaseInvoice_CreditDays]  DEFAULT ((0)) FOR [CreditDays]
GO

ALTER TABLE [dbo].[PurchaseInvoice] ADD  CONSTRAINT [DF_PurchaseInvoice_Discount]  DEFAULT ((0)) FOR [Discount]
GO

ALTER TABLE [dbo].[PurchaseInvoice] ADD  CONSTRAINT [DF_PurchaseInvoice_StatusDiscountAmt]  DEFAULT ((1)) FOR [StatusDiscountAmt]
GO

ALTER TABLE [dbo].[PurchaseInvoice] ADD  CONSTRAINT [DF_PurchaseInvoice_OtherCharges]  DEFAULT ((0)) FOR [OtherCharges]
GO


-----



/****** Object:  Table [dbo].[SalesInvoice]    Script Date: 16-10-2019 03:17:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SalesInvoice](
	[SalesInvoiceID] [int] IDENTITY(1,1) NOT NULL,
	[SalesInvoiceNo] [varchar](50) NULL,
	[SalesInvoiceDate] [datetime] NULL,
	[Reference] [varchar](100) NULL,
	[LPOReference] [varchar](100) NULL,
	[CustomerID] [int] NULL,
	[EmployeeID] [int] NULL,
	[QuotationID] [int] NULL,
	[CurrencyID] [int] NULL,
	[ExchangeRate] [decimal](18, 2) NULL,
	[CreditDays] [int] NULL,
	[DueDate] [datetime] NULL,
	[AcJournalID] [int] NULL,
	[BranchID] [int] NULL,
	[Discount] [decimal](18, 2) NULL,
	[StatusDiscountAmt] [bit] NULL,
	[OtherCharges] [decimal](18, 2) NULL,
	[PaymentTerm] [varchar](max) NULL,
	[Remarks] [varchar](max) NULL,
	[FYearID] [int] NULL,
 CONSTRAINT [PK_SalesInvoiceNew] PRIMARY KEY CLUSTERED 
(
	[SalesInvoiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[SalesInvoice] ADD  CONSTRAINT [DF_SalesInvoice_ExchangeRate]  DEFAULT ((0)) FOR [ExchangeRate]
GO

ALTER TABLE [dbo].[SalesInvoice] ADD  CONSTRAINT [DF_SalesInvoice_CreditDays]  DEFAULT ((0)) FOR [CreditDays]
GO

ALTER TABLE [dbo].[SalesInvoice] ADD  CONSTRAINT [DF__SalesInvo__AcCom__2665ABE1]  DEFAULT ((1)) FOR [BranchID]
GO

ALTER TABLE [dbo].[SalesInvoice] ADD  CONSTRAINT [sALESiNVOICE_dISCOUNT]  DEFAULT ((0)) FOR [Discount]
GO

ALTER TABLE [dbo].[SalesInvoice] ADD  CONSTRAINT [sALESiNVOICE_STATUSDISCOUNTAMT]  DEFAULT ((1)) FOR [StatusDiscountAmt]
GO

ALTER TABLE [dbo].[SalesInvoice] ADD  CONSTRAINT [DF_SalesInvoice_OtherCharges]  DEFAULT ((0)) FOR [OtherCharges]
GO



----- oct 16 -- 

USE [DB_9F57C4_ShippingTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertSalesInvoice]    Script Date: 16-10-2019 15:43:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER proc [dbo].[SP_InsertSalesInvoice]
(
@SalesInvoiceId int=0 output,
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
Declare @MaxId int;
SELECT @MaxId = MAX(@SalesInvoiceId) FROM SalesInvoice;

SET @SalesInvoiceNo = RIGHT('0000' + CAST(@MaxId AS VARCHAR(5)),5);
SET @SalesInvoiceNo = 'SI-' + @SalesInvoiceNo;

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
		 SET @SalesInvoiceId = SCOPE_IDENTITY();
		 return @SalesInvoiceId;
END

------------------


/****** Object:  StoredProcedure [dbo].[SP_InsertPurchaseInvoice]    Script Date: 16-10-2019 16:43:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER proc [dbo].[SP_InsertPurchaseInvoice]
(
@PurchaseInvoiceId int=0 output,
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
Declare @MaxId int;
SELECT @MaxId = MAX(@PurchaseInvoiceId) FROM PurchaseInvoice;

SET @PurchaseInvoiceNo = RIGHT('0000' + CAST(@MaxId AS VARCHAR(5)),5);
SET @PurchaseInvoiceNo = 'PI-' + @PurchaseInvoiceNo;

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
		  SET @PurchaseInvoiceId = SCOPE_IDENTITY();
		 return @PurchaseInvoiceId;
END

----------------------------
alter table SalesInvoice add DeliveryId int;

alter table SalesInvoice Add QuotationNumber nvarchar(100);
----------------------------------------oct18


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[SP_GetAllJOB]
AS
BEGIN
Select * from dbo.Job
END
------------------------
/****** Object:  Table [dbo].[Supplier]    Script Date: 18-10-2019 03:17:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ProductServices](
	[ProductID] [int] NOT NULL,
	[ProductName] [nvarchar](100) NULL,
	[Status] [nvarchar](50) NULL,
	
	);


	USE [DB_9F57C4_ShippingTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_DeletePurchaseInvoice]    Script Date: 18-10-2019 06:39:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[SP_DeletePurchaseInvoiceDetails]
(
@PurchaseInvoiceId int
)
AS
BEGIN
Delete from PurchaseInvoiceDetails where PurchaseInvoiceID=@PurchaseInvoiceId

END

------------------------


/****** Object:  StoredProcedure [dbo].[SP_InsertPurchaseInvoice]    Script Date: 18-10-2019 07:34:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[SP_InsertPurchaseInvoiceDetails]
(
@PurchaseInvoiceDetailID int=0 output,
@PurchaseInvoiceID int,
@ProductID int,
@Quantity int,
@ItemUnitID int,
@Rate decimal,
@RateFC decimal,
@Value decimal,
@ValueFC decimal,
@Taxprec decimal,
@Tax decimal,
@NetValue decimal,
@AcHeadID int,
@JobID int,
@Description nvarchar(max)

)
AS
Begin

 INSERT INTO [PurchaseInvoiceDetails] 
           ([PurchaseInvoiceID],
		   [ProductID],
		    [Quantity],
	[ItemUnitID],
	[Rate],
	[RateFC],
	[Value],
	[ValueFC],
	[Taxprec],
	[Tax],
	[NetValue],
	[AcHeadID],
	[JobID],
	[Description]			
         )
     VALUES
           (@PurchaseInvoiceID,
       @ProductID, 
@Quantity, 
@ItemUnitID,
@Rate,
@RateFC,
@Value,
@ValueFC,
@Taxprec,
@Tax,
@NetValue,
@AcHeadID,
@JobID,
@Description
         )
		 
END
-------------------------




/****** Object:  Table [dbo].[SalesInvoiceDetails]    Script Date: 18-10-2019 07:27:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SalesInvoiceDetails](
	[SalesInvoiceDetailID] [int] IDENTITY(1,1) NOT NULL,
	[SalesInvoiceID] [int] NULL,
	[ProductID] [int] NULL,
	[Quantity] [int] NULL,
	[ItemUnitID] [int] NULL,
	[Rate] [decimal](18, 2) NULL,
	[RateLC] [decimal](18, 2) NULL,
	[RateFC] [decimal](18, 2) NULL,
	[Value] [decimal](18, 2) NULL,
	[ValueLC] [decimal](18, 2) NULL,
	[ValueFC] [decimal](18, 2) NULL,
	[Tax] [decimal](18, 2) NULL,
	[NetValue] [decimal](18, 2) NULL,
	[JobID] [int] NULL,
	[Description] [varchar](max) NULL,
 )


------------------------




/****** Object:  Table [dbo].[PurchaseInvoiceDetails]    Script Date: 18-10-2019 07:19:34 ******/


SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PurchaseInvoiceDetails](
	[PurchaseInvoiceDetailID] [int] IDENTITY(1,1) NOT NULL,
	[PurchaseInvoiceID] [int] NULL,
	[ProductID] [int] NULL,
	[Quantity] [int] NULL,
	[ItemUnitID] [int] NULL,
	[Rate] [decimal](18, 2) NULL,
	[RateFC] [decimal](18, 2) NULL,
	[Value] [decimal](18, 2) NULL,
	[ValueFC] [decimal](18, 2) NULL,
	[Taxprec] [decimal](5, 2) NULL,
	[Tax] [decimal](18, 2) NULL,
	[NetValue] [decimal](18, 2) NULL,
	[AcHeadID] [int] NULL,
	[JobID] [int] NULL,
	[Description] [varchar](max) NULL,
 )
GO

-----------------------------------------

/****** Object:  StoredProcedure [dbo].[SP_InsertPurchaseInvoice]    Script Date: 18-10-2019 07:34:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[SP_InsertSalesInvoiceDetails]
(
@SalesInvoiceDetailID int=0 output,
@SalesInvoiceID int,
@ProductID int,
@Quantity int,
@ItemUnitID int,
@Rate decimal,
@RateLC decimal,
@RateFC decimal,
@Value decimal,
@ValueLC decimal,
@ValueFC decimal,
@Tax decimal,
@NetValue decimal,
@JobID int,
@Description nvarchar(max)

)
AS
Begin

 INSERT INTO [SalesInvoiceDetails] 
           ([SalesInvoiceID],
		   [ProductID],
		    [Quantity],
	[ItemUnitID],
	[Rate],[RateLC],
	[RateFC],
	[Value],[ValueLC],
	[ValueFC],
	[Tax],
	[NetValue],
	
	[JobID],
	[Description]			
         )
     VALUES
           (@SalesInvoiceID,
       @ProductID, 
@Quantity, 
@ItemUnitID,
@Rate,@RateLC,
@RateFC,
@Value,
@ValueLC,
@ValueFC,
@Tax,
@NetValue,
@JobID,
@Description
         )
		 
END

----------------------
/****** Object:  StoredProcedure [dbo].[SP_UpdatePurchaseInvoice]    Script Date: 18-10-2019 18:42:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[SP_UpdatePurchaseInvoiceDetails]
(
@PurchaseInvoiceDetailID int=0 output,
@PurchaseInvoiceID int,
@ProductID int,
@Quantity int,
@ItemUnitID int,
@Rate decimal,
@RateFC decimal,
@Value decimal,
@ValueFC decimal,
@Taxprec decimal,
@Tax decimal,
@NetValue decimal,
@AcHeadID int,
@JobID int,
@Description nvarchar(max)
)
AS
Begin

UPDATE [PurchaseInvoiceDetails]  SET 
    [PurchaseInvoiceID]=@PurchaseInvoiceID,
		   [ProductID]=@ProductID,
		    [Quantity]=@Quantity,
	[ItemUnitID]=@ItemUnitID,
	[Rate]=@Rate,
	[RateFC]=@RateFC,
	[Value]=@Value,
	[ValueFC]=@ValueFC,
	[Taxprec]=@Taxprec,
	[Tax]=@Tax,
	[NetValue]=@NetValue,
	[AcHeadID]=@AcHeadID,
	[JobID]=@JobID,
	[Description]=@Description	
		WHERE PurchaseInvoiceDetailID=@PurchaseInvoiceDetailID
  
END
----------------------------
/****** Object:  StoredProcedure [dbo].[SP_UpdatePurchaseInvoice]    Script Date: 18-10-2019 18:42:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[SP_UpdateSalesInvoiceDetails]
(
@SalesInvoiceDetailID int=0 output,
@SalesInvoiceID int,
@ProductID int,
@Quantity int,
@ItemUnitID int,
@Rate decimal,
@RateLC decimal,
@RateFC decimal,
@Value decimal,
@ValueLC decimal,
@ValueFC decimal,
@Tax decimal,
@NetValue decimal,
@JobID int,
@Description nvarchar(max)
)
AS
Begin

UPDATE [SalesInvoiceDetails]  SET 
    [SalesInvoiceID]=@SalesInvoiceID,
		   [ProductID]=@ProductID,
		    [Quantity]=@Quantity,
	[ItemUnitID]=@ItemUnitID,
	[Rate]=@Rate,
	[RateLC]=@RateLC,
	[RateFC]=@RateFC,
	[Value]=@Value,
	[ValueLC]=@ValueLC,
	[ValueFC]=@ValueFC,
	[Tax]=@Tax,
	[NetValue]=@NetValue,
	[JobID]=@JobID,
	[Description]=@Description	
		WHERE SalesInvoiceDetailID=@SalesInvoiceDetailID
  
END


-- sethu oct 22
/****** Object:  StoredProcedure [dbo].[SP_GetAllJobsDetailsByDate]    Script Date: 10/22/2019 7:07:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[SP_GetAllSalesInvoiceByDate]--'2016-12-1','2016-12-21'
(
@fdate datetime,
@tdate datetime 
)
AS
BEGIN
select 
i.SalesInvoiceID,
i.SalesInvoiceNo,
i.SalesInvoiceDate,
i.DueDate,
C.Customer as Client
from SalesInvoice i 
left outer join CUSTOMER C on i.CustomerID=c.CustomerID 
where (i.SalesInvoiceDate>=@fdate and i.SalesInvoiceDate<=@tdate) order by SalesInvoiceDate desc 
END


/***** Object:  Table [dbo].[ProductServices]    Script Date: 22-10-2019 08:43:35 *****/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ProductServices](
	[ProductID] [int] NOT NULL,
	[ProductName] [nvarchar](100) NULL,
	[Status] [nvarchar](50) NULL
) ON [PRIMARY]
GO


/***** Object:  StoredProcedure [dbo].[SP_GetAllProductServices]    Script Date: 22-10-2019 08:44:00 *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[SP_GetAllProductServices]
AS
Begin 
Select * from dbo.ProductServices where status = 1
END


EXEC sp_rename 'SalesInvoiceDetails.Rate', 'RateType', 'COLUMN';

ALTER TABLE SalesInvoiceDetails
ALTER COLUMN RateType nvarchar(50);


---

USE [DB_9F57C4_ShippingTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertSalesInvoiceDetails]    Script Date: 24-10-2019 16:58:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER proc [dbo].[SP_InsertSalesInvoiceDetails]
(
@SalesInvoiceDetailID int=0 output,
@SalesInvoiceID int,
@ProductID int,
@Quantity int,
@ItemUnitID int,
@RateType nvarchar(50),
@RateLC decimal,
@RateFC decimal,
@Value decimal,
@ValueLC decimal,
@ValueFC decimal,
@Tax decimal,
@NetValue decimal,
@JobID int,
@Description nvarchar(max)

)
AS
Begin

 INSERT INTO [SalesInvoiceDetails] 
           ([SalesInvoiceID],
		   [ProductID],
		    [Quantity],
	[ItemUnitID],
	[RateType],[RateLC],
	[RateFC],
	[Value],[ValueLC],
	[ValueFC],
	[Tax],
	[NetValue],
	
	[JobID],
	[Description]			
         )
     VALUES
           (@SalesInvoiceID,
       @ProductID, 
@Quantity, 
@ItemUnitID,
@RateType,@RateLC,
@RateFC,
@Value,
@ValueLC,
@ValueFC,
@Tax,
@NetValue,
@JobID,
@Description
         )
		 
END


--
ALTER TABLE SalesInvoice Add DiscountType int;
ALTER TABLE SalesInvoice ADD DiscountValueLC decimal(18,2);
ALTER TABLE SalesInvoice ADD DiscountValueFC decimal(18,2);

-----
USE [DB_9F57C4_ShippingTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateSalesInvoice]    Script Date: 10/25/2019 6:36:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc[dbo].[SP_UpdateSalesInvoice]
    (
@SalesInvoiceId int = 0,
@SalesInvoiceNo nvarchar(50),
@SalesInvoiceDate datetime,
@Reference nvarchar(50),
@LPOReference nvarchar(50),
@CustomerID int,
@EmployeeeID int,
@QuotationNumber nvarchar(100) = '',
@CurrencyID int,
@EXchangeRate decimal,
@CreditDays int,
@DueDate datetime,
@AcJouranalID int,
@BranchID int,
@Discount decimal,
@StatusDiscountAmt bit,
@OtherCharges decimal,
@PaymentTerm nvarchar(max),
@Remarks nvarchar(max),
@FYearID int,
@DeliveryId int,
@DiscountType int,
@DiscountValueLC decimal(18,2),
@DiscountValueFC decimal(18,2)
)
AS
Begin

UPDATE[SalesInvoice]  SET
[SalesInvoiceNo] =@SalesInvoiceNo,
[SalesInvoiceDate] =@SalesInvoiceDate,
[Reference] =@Reference,
[LPOReference] =@LPOReference,
[CustomerID] =@CustomerID,
[EmployeeID] =@EmployeeeID,
[QuotationNumber] =@QuotationNumber,
[CurrencyID] =@CurrencyID,
[ExchangeRate] =@ExchangeRate,
[CreditDays] =@CreditDays,
[DueDate] =@DueDate,
[AcJournalID] =@AcJouranalID,
[BranchID] =@BranchID,
[Discount] =@Discount,
[StatusDiscountAmt] =@StatusDiscountAmt,
[OtherCharges] =@OtherCharges,
[PaymentTerm] =@PaymentTerm,
[Remarks] =@Remarks,
[FYearID] =@FYearID,
[DeliveryId] =@DeliveryId,
[DiscountType] = @DiscountType,
	[DiscountValueLC] = @DiscountValueLC,
	[DiscountValueFC] = @DiscountValueFC
WHERE SalesInvoiceID =@SalesInvoiceId

END


----
USE [DB_9F57C4_ShippingTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertSalesInvoice]    Script Date: 10/25/2019 6:34:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER proc[dbo].[SP_InsertSalesInvoice]
    (
@SalesInvoiceId int = 0 output,
@SalesInvoiceNo nvarchar(50),
@SalesInvoiceDate datetime = null,
@Reference nvarchar(50),
@LPOReference nvarchar(50),
@CustomerID int,
@EmployeeeID int,
@QuotationNumber nvarchar(100) = '',
@CurrencyID int,
@EXchangeRate decimal,
@CreditDays int,
@DueDate datetime = null,
@AcJouranalID int,
@BranchID int,
@Discount decimal,
@StatusDiscountAmt bit,
@OtherCharges decimal,
@PaymentTerm nvarchar(max),
@Remarks nvarchar(max),
@FYearID int,
@DeliveryId int,
@DiscountType int,
@DiscountValueLC decimal(18,2),
@DiscountValueFC decimal(18,2)
)
AS
Begin
Declare @MaxId int;
SELECT @MaxId = MAX(@SalesInvoiceId) FROM SalesInvoice;

SET @SalesInvoiceNo = RIGHT('0000' + CAST(@MaxId AS VARCHAR(5)), 5);
SET @SalesInvoiceNo = 'SI-' + @SalesInvoiceNo;

INSERT INTO[SalesInvoice]
    ([SalesInvoiceNo],
    [SalesInvoiceDate],
    [Reference],
    [LPOReference],
    [CustomerID],
    [EmployeeID],
    [QuotationNumber],
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
    [FYearID],
    [DeliveryId],
	[DiscountType],
	[DiscountValueLC],
	[DiscountValueFC]
    )
VALUES
    (
          @SalesInvoiceNo,
@SalesInvoiceDate,
@Reference,
@LPOReference,
@CustomerID,
@EmployeeeID,
@QuotationNumber,
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
@FYearID,
@DeliveryId,
@DiscountType,
@DiscountValueLC,
@DiscountValueFC
 )
SET @SalesInvoiceId = SCOPE_IDENTITY();
return @SalesInvoiceId;
END

-------------------------------------------------------------------------------------------------------------------------
ALTER TABLE PurchaseInvoice Add DiscountType int;
ALTER TABLE PurchaseInvoice ADD DiscountValueLC decimal(18,2);
ALTER TABLE PurchaseInvoice ADD DiscountValueFC decimal(18,2);

-------------------------------------------------------------------------------------------------------------------------


USE [DB_9F57C4_ShippingTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertPurchaseInvoice]    Script Date: 25-10-2019 15:00:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER proc [dbo].[SP_InsertPurchaseInvoice]
(
@PurchaseInvoiceId int=0 output,
@PurchaseInvoiceNo nvarchar(50),
@PurchaseInvoiceDate datetime,
@Reference nvarchar(50),
@LPOReference nvarchar(50),
@SupplierID int,
@EmployeeeID int,
@QuotationNumber nvarchar(100)= '',
@CurrencyID int,
@ExchangeRate decimal,
@CreditDays int,
@DueDate datetime,
@AcJouranalID int,
@BranchID int,
@Discount decimal,
@StatusDiscountAmt bit,
@OtherCharges decimal,
@PaymentTerm nvarchar(max),
@Remarks nvarchar(max),
@FYearID int,
@DiscountType int,
@DiscountValueLC decimal(18,2),
@DiscountValueFC decimal(18,2)
)
AS
Begin
Declare @MaxId int;
SELECT @MaxId = MAX(@PurchaseInvoiceId) FROM PurchaseInvoice;

SET @PurchaseInvoiceNo = RIGHT('0000' + CAST(@MaxId AS VARCHAR(5)),5);
SET @PurchaseInvoiceNo = 'PI-' + @PurchaseInvoiceNo;

 INSERT INTO [PurchaseInvoice] 
           ([PurchaseInvoiceNo],
		   [PurchaseInvoiceDate],
		     [Reference],
			   [LPOReference],
			   [SupplierID],
			   [EmployeeID],
			   [QuotationNumber],
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
			   [FYearID],
			 [DiscountType],
	[DiscountValueLC],
	[DiscountValueFC]
         )
     VALUES
           (
          @PurchaseInvoiceNo, 
@PurchaseInvoiceDate,
@Reference,
@LPOReference,
@SupplierID,
@EmployeeeID,
@QuotationNumber,
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
@FYearID,
@DiscountType,
@DiscountValueLC,
@DiscountValueFC
         )
		  SET @PurchaseInvoiceId = SCOPE_IDENTITY();
		 return @PurchaseInvoiceId;
END

------------------------------------------------------------------------------------------------------------------------
USE [DB_9F57C4_ShippingTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdatePurchaseInvoice]    Script Date: 25-10-2019 15:04:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[SP_UpdatePurchaseInvoice]
(
@PurchaseInvoiceId int=0,
@PurchaseInvoiceNo nvarchar(50),
@PurchaseInvoiceDate datetime,
@Reference nvarchar(50),
@LPOReference nvarchar(50),
@SupplierID int,
@EmployeeeID int,
@QuotationNumber nvarchar(100)='',
@CurrencyID int,
@ExchangeRate decimal,
@CreditDays int,
@DueDate datetime,
@AcJouranalID int,
@BranchID int,
@Discount decimal,
@StatusDiscountAmt bit,
@OtherCharges decimal,
@PaymentTerm nvarchar(max),
@Remarks nvarchar(max),
@FYearID int,
@DiscountType int,
@DiscountValueLC decimal(18,2),
@DiscountValueFC decimal(18,2)
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
			   [QuotationNumber]=@QuotationNumber,
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
			   [FYearID]=@FYearID,
			   [DiscountType] = @DiscountType,
	[DiscountValueLC] = @DiscountValueLC,
	[DiscountValueFC] = @DiscountValueFC
		WHERE PurchaseInvoiceID=@PurchaseInvoiceId
  
END
-----------------------------------------------------------------------------oct31--------------------------

ALTER TABLE CountryMaster ADD CountryPrefix nvarchar(50);
ALTER TABLE Location ADD Country nvarchar(50);
ALTER TABLE CurrencyMaster ADD CurrencyCode nvarchar(50);
ALTER TABLE BranchMaster ADD City nvarchar(50);
ALTER TABLE BranchMaster ADD ContactPerson nvarchar(50);
ALTER TABLE ShippingAgent ADD City nvarchar(50);
ALTER TABLE ShippingAgent ADD Discount Decimal(18,5);
ALTER TABLE ShippingAgent ADD ExportCode int;
ALTER TABLE ShippingAgent ADD CreditLimit int;
ALTER TABLE ShippingAgent ADD CreditDays int;
ALTER TABLE Transporter ADD CountryID int;
ALTER TABLE Transporter ADD City nvarchar(100);
ALTER TABLE CUSTOMER ADD CountryID int;
ALTER TABLE CUSTOMER ADD City nvarchar(100);
ALTER TABLE Supplier ADD CountryID int;
ALTER TABLE Supplier ADD City nvarchar(100);
ALTER TABLE Supplier ADD ExportCode int;
ALTER TABLE AcCompany ADD ContactPerson nvarchar(100);
ALTER TABLE AcCompany ADD logo nvarchar(max);