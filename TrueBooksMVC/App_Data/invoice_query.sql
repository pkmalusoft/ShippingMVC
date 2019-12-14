SE[DB_9F57C4_ShippingTest]
GO

/****** Object:  Table [dbo].[PurchaseInvoice]    Script Date: 22-10-2019 05:51:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE[dbo].[PurchaseInvoice](
    [PurchaseInvoiceID][int] IDENTITY(1, 1) NOT NULL,
    [PurchaseInvoiceNo][nvarchar](50) NULL,
    [PurchaseInvoiceDate][datetime] NULL,
    [Reference][nvarchar](100) NULL,
    [LPOReference][nvarchar](100) NULL,
    [SupplierID][int] NULL,
    [EmployeeID][int] NULL,
    [QuotationNumber][nvarchar](100) NULL,
    [CurrencyID][int] NULL,
    [ExchangeRate][decimal](18, 2) NULL,
    [CreditDays][int] NULL,
    [DueDate][datetime] NULL,
    [AcJournalID][int] NULL,
    [BranchID][int] NULL,
    [Discount][decimal](18, 2) NULL,
    [StatusDiscountAmt][bit] NULL,
    [OtherCharges][decimal](18, 2) NULL,
    [PaymentTerm][nvarchar](max) NULL,
    [Remarks][nvarchar](max) NULL,
    [FYearID][int] NULL,
    CONSTRAINT[PK_PurchaseInvoiceNew] PRIMARY KEY CLUSTERED
        (
        [PurchaseInvoiceID] ASC
        )WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]
GO

ALTER TABLE[dbo].[PurchaseInvoice] ADD  CONSTRAINT[DF_PurchaseInvoice_ExchangeRate]  DEFAULT((0)) FOR[ExchangeRate]
GO

ALTER TABLE[dbo].[PurchaseInvoice] ADD  CONSTRAINT[DF_PurchaseInvoice_CreditDays]  DEFAULT((0)) FOR[CreditDays]
GO

ALTER TABLE[dbo].[PurchaseInvoice] ADD  CONSTRAINT[DF_PurchaseInvoice_Discount]  DEFAULT((0)) FOR[Discount]
GO

ALTER TABLE[dbo].[PurchaseInvoice] ADD  CONSTRAINT[DF_PurchaseInvoice_StatusDiscountAmt]  DEFAULT((1)) FOR[StatusDiscountAmt]
GO

ALTER TABLE[dbo].[PurchaseInvoice] ADD  CONSTRAINT[DF_PurchaseInvoice_OtherCharges]  DEFAULT((0)) FOR[OtherCharges]
GO


------------------------------------------------------------------------------



    USE[DB_9F57C4_ShippingTest]
GO

/****** Object:  Table [dbo].[PurchaseInvoiceDetails]    Script Date: 22-10-2019 05:52:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE[dbo].[PurchaseInvoiceDetails](
    [PurchaseInvoiceDetailID][int] IDENTITY(1, 1) NOT NULL,
    [PurchaseInvoiceID][int] NULL,
    [ProductID][int] NULL,
    [Quantity][int] NULL,
    [ItemUnitID][int] NULL,
    [Rate][decimal](18, 2) NULL,
    [RateFC][decimal](18, 2) NULL,
    [Value][decimal](18, 2) NULL,
    [ValueFC][decimal](18, 2) NULL,
    [Taxprec][decimal](5, 2) NULL,
    [Tax][decimal](18, 2) NULL,
    [NetValue][decimal](18, 2) NULL,
    [AcHeadID][int] NULL,
    [JobID][int] NULL,
    [Description][varchar](max) NULL
) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]
GO


--------------------------------------------------------------------------------------------


    USE[DB_9F57C4_ShippingTest]
GO

/****** Object:  Table [dbo].[SalesInvoice]    Script Date: 22-10-2019 05:52:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE[dbo].[SalesInvoice](
    [SalesInvoiceID][int] IDENTITY(1, 1) NOT NULL,
    [SalesInvoiceNo][nvarchar](50) NULL,
    [SalesInvoiceDate][datetime] NULL,
    [QuotationNumber][nvarchar](100) NULL,
    [Reference][nvarchar](100) NULL,
    [LPOReference][nvarchar](100) NULL,
    [CustomerID][int] NULL,
    [EmployeeID][int] NULL,
    [CurrencyID][int] NULL,
    [ExchangeRate][decimal](18, 2) NULL,
    [CreditDays][int] NULL,
    [DueDate][datetime] NULL,
    [AcJournalID][int] NULL,
    [BranchID][int] NULL,
    [Discount][decimal](18, 2) NULL,
    [StatusDiscountAmt][bit] NULL,
    [OtherCharges][decimal](18, 2) NULL,
    [PaymentTerm][nvarchar](max) NULL,
    [Remarks][nvarchar](max) NULL,
    [FYearID][int] NULL,
    [DeliveryId][int] NULL,
    CONSTRAINT[PK_SalesInvoiceNew] PRIMARY KEY CLUSTERED
        (
        [SalesInvoiceID] ASC
        )WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]
GO

ALTER TABLE[dbo].[SalesInvoice] ADD  CONSTRAINT[DF_SalesInvoice_ExchangeRate]  DEFAULT((0)) FOR[ExchangeRate]
GO

ALTER TABLE[dbo].[SalesInvoice] ADD  CONSTRAINT[DF_SalesInvoice_CreditDays]  DEFAULT((0)) FOR[CreditDays]
GO

ALTER TABLE[dbo].[SalesInvoice] ADD  CONSTRAINT[DF__SalesInvo__AcCom__2665ABE1]  DEFAULT((1)) FOR[BranchID]
GO

ALTER TABLE[dbo].[SalesInvoice] ADD  CONSTRAINT[sALESiNVOICE_dISCOUNT]  DEFAULT((0)) FOR[Discount]
GO

ALTER TABLE[dbo].[SalesInvoice] ADD  CONSTRAINT[sALESiNVOICE_STATUSDISCOUNTAMT]  DEFAULT((1)) FOR[StatusDiscountAmt]
GO

ALTER TABLE[dbo].[SalesInvoice] ADD  CONSTRAINT[DF_SalesInvoice_OtherCharges]  DEFAULT((0)) FOR[OtherCharges]
GO


-------------------------------------------------------------------------------------------------------------------------------------

    USE[DB_9F57C4_ShippingTest]
GO

/****** Object:  Table [dbo].[SalesInvoiceDetails]    Script Date: 22-10-2019 05:53:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE[dbo].[SalesInvoiceDetails](
    [SalesInvoiceDetailID][int] IDENTITY(1, 1) NOT NULL,
    [SalesInvoiceID][int] NULL,
    [ProductID][int] NULL,
    [Quantity][int] NULL,
    [ItemUnitID][int] NULL,
    [Rate][decimal](18, 2) NULL,
    [RateLC][decimal](18, 2) NULL,
    [RateFC][decimal](18, 2) NULL,
    [Value][decimal](18, 2) NULL,
    [ValueLC][decimal](18, 2) NULL,
    [ValueFC][decimal](18, 2) NULL,
    [Tax][decimal](18, 2) NULL,
    [NetValue][decimal](18, 2) NULL,
    [JobID][int] NULL,
    [Description][varchar](max) NULL
) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]
GO


--------------------------------------------------------------------------------------------------

    USE[DB_9F57C4_ShippingTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_DeletePurchaseInvoice]    Script Date: 22-10-2019 05:54:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Proc[dbo].[SP_DeletePurchaseInvoice]
    (
@PurchaseInvoiceId int
)
AS
BEGIN
Delete from PurchaseInvoice where PurchaseInvoiceID =@PurchaseInvoiceId

END
---------------------------------

    USE[DB_9F57C4_ShippingTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_DeletePurchaseInvoiceDetails]    Script Date: 22-10-2019 05:54:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Proc[dbo].[SP_DeletePurchaseInvoiceDetails]
    (
@PurchaseInvoiceId int
)
AS
BEGIN
Delete from PurchaseInvoiceDetails where PurchaseInvoiceID =@PurchaseInvoiceId

END
-----------------------------------------

    USE[DB_9F57C4_ShippingTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_DeleteSalesInvoice]    Script Date: 22-10-2019 05:55:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER
Proc[dbo].[SP_DeleteSalesInvoice]
    (
@SalesInvoiceId int
)
AS
BEGIN
Delete from SalesInvoice where SalesInvoiceID =@SalesInvoiceId

END
-----------------------------------------------------------

    USE[DB_9F57C4_ShippingTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_DeleteSalesInvoiceDetails]    Script Date: 22-10-2019 05:55:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Proc[dbo].[SP_DeleteSalesInvoiceDetails]
    (
@SalesInvoiceID int
)
AS
BEGIN
Delete from SalesInvoiceDetails where SalesInvoiceID =@SalesInvoiceID

END
------------------------------------------------
    USE[DB_9F57C4_ShippingTest]
GO

/****** Object:  Table [dbo].[ProductServices]    Script Date: 22-10-2019 05:57:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE[dbo].[ProductServices](
    [ProductID][int] NOT NULL,
    [ProductName][nvarchar](100) NULL,
    [Status][nvarchar](50) NULL
) ON[PRIMARY]
GO

--------------------------------------------------

    USE[DB_9F57C4_ShippingTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetPurchaseInvoiceByID]    Script Date: 22-10-2019 05:58:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC[dbo].[SP_GetPurchaseInvoiceByID]
    (
@PurchaseInvoiceID int
)
AS
BEGIN
SELECT * FROM PurchaseInvoice where PurchaseInvoiceID = @PurchaseInvoiceID
END
-------------------------------------------------


    USE[DB_9F57C4_ShippingTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetPurchaseInvoiceDetailsByID]    Script Date: 22-10-2019 05:58:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC[dbo].[SP_GetPurchaseInvoiceDetailsByID]
    (
@PurchaseInvoiceID int
)
AS
BEGIN
SELECT * FROM PurchaseInvoiceDetails where PurchaseInvoiceID = @PurchaseInvoiceID
END
--------------------------------------------

    USE[DB_9F57C4_ShippingTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetSalesInvoiceByID]    Script Date: 22-10-2019 05:58:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC[dbo].[SP_GetSalesInvoiceByID]
    (
@SalesInvoiceID int
)
AS
BEGIN
SELECT * FROM SalesInvoice where SalesInvoiceID = @SalesInvoiceID
END

--------------------------------------

    USE[DB_9F57C4_ShippingTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetSalesInvoiceDetailsByID]    Script Date: 22-10-2019 05:59:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC[dbo].[SP_GetSalesInvoiceDetailsByID]
    (
@SalesInvoiceID int
)
AS
BEGIN
SELECT * FROM SalesInvoiceDetails where SalesInvoiceID = @SalesInvoiceID
END

-----------------------------



    USE[DB_9F57C4_ShippingTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertSalesInvoice]    Script Date: 22-10-2019 05:59:43 ******/
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
@DeliveryId int
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
    [DeliveryId]


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
@DeliveryId        )
SET @SalesInvoiceId = SCOPE_IDENTITY();
return @SalesInvoiceId;
END

----
--backup
/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [SalesInvoiceID]
      ,[DocumentNo]
      ,[TransDate]
      ,[CustomerID]
      ,[Remarks]
      ,[Amt]
      ,[AcCompanyID]
      ,[AcjournalID]
  FROM [malusoft_shipping].[dbo].[SalesInvoice]
  ---------
----------------------------------

    USE[DB_9F57C4_ShippingTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertSalesInvoiceDetails]    Script Date: 22-10-2019 06:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER proc[dbo].[SP_InsertSalesInvoiceDetails]
    (
@SalesInvoiceDetailID int = 0 output,
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

INSERT INTO[SalesInvoiceDetails]
    ([SalesInvoiceID],
    [ProductID],
    [Quantity],
    [ItemUnitID],
    [Rate], [RateLC],
    [RateFC],
    [Value], [ValueLC],
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
@Rate, @RateLC,
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
-------------------------------------------

    USE[DB_9F57C4_ShippingTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertPurchaseInvoice]    Script Date: 22-10-2019 06:01:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER proc[dbo].[SP_InsertPurchaseInvoice]
    (
@PurchaseInvoiceId int = 0 output,
@PurchaseInvoiceNo nvarchar(50),
@PurchaseInvoiceDate datetime,
@Reference nvarchar(50),
@LPOReference nvarchar(50),
@SupplierID int,
@EmployeeeID int,
@QuotationNumber nvarchar(100) = '',
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
@FYearID int
)
AS
Begin
Declare @MaxId int;
SELECT @MaxId = MAX(@PurchaseInvoiceId) FROM PurchaseInvoice;

SET @PurchaseInvoiceNo = RIGHT('0000' + CAST(@MaxId AS VARCHAR(5)), 5);
SET @PurchaseInvoiceNo = 'PI-' + @PurchaseInvoiceNo;

INSERT INTO[PurchaseInvoice]
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
@FYearID
         )
SET @PurchaseInvoiceId = SCOPE_IDENTITY();
return @PurchaseInvoiceId;
END
---------------------------------

    USE[DB_9F57C4_ShippingTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertPurchaseInvoiceDetails]    Script Date: 22-10-2019 06:01:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER proc[dbo].[SP_InsertPurchaseInvoiceDetails]
    (
@PurchaseInvoiceDetailID int = 0 output,
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

INSERT INTO[PurchaseInvoiceDetails]
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
@AcHeadID ,
@JobID,
@Description
         )

END
--------------------------------------------------------
    USE[DB_9F57C4_ShippingTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdatePurchaseInvoice]    Script Date: 22-10-2019 06:01:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc[dbo].[SP_UpdatePurchaseInvoice]
    (
@PurchaseInvoiceId int = 0,
@PurchaseInvoiceNo nvarchar(50),
@PurchaseInvoiceDate datetime,
@Reference nvarchar(50),
@LPOReference nvarchar(50),
@SupplierID int,
@EmployeeeID int,
@QuotationNumber nvarchar(100) = '',
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
@FYearID int
)
AS
Begin

UPDATE[PurchaseInvoice]  SET
[PurchaseInvoiceNo] =@PurchaseInvoiceNo,
[PurchaseInvoiceDate] =@PurchaseInvoiceDate,
[Reference] =@Reference,
[LPOReference] =@LPOReference,
[SupplierID] =@SupplierID,
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
[FYearID] =@FYearID
WHERE PurchaseInvoiceID =@PurchaseInvoiceId

END
------------------------------------------------------

    USE[DB_9F57C4_ShippingTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdatePurchaseInvoiceDetails]    Script Date: 22-10-2019 06:02:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc[dbo].[SP_UpdatePurchaseInvoiceDetails]
    (
@PurchaseInvoiceDetailID int = 0 output,
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

UPDATE[PurchaseInvoiceDetails]  SET
[PurchaseInvoiceID] =@PurchaseInvoiceID,
[ProductID] =@ProductID,
[Quantity] =@Quantity,
[ItemUnitID] =@ItemUnitID,
[Rate] =@Rate,
[RateFC] =@RateFC,
[Value] =@Value,
[ValueFC] =@ValueFC,
[Taxprec] =@Taxprec,
[Tax] =@Tax,
[NetValue] =@NetValue,
[AcHeadID] =@AcHeadID,
[JobID] =@JobID,
[Description] =@Description
WHERE PurchaseInvoiceDetailID =@PurchaseInvoiceDetailID

END
----------------------------------------------------------


    USE[DB_9F57C4_ShippingTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateSalesInvoice]    Script Date: 22-10-2019 06:02:24 ******/
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
@DeliveryId int
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
[DeliveryId] =@DeliveryId
WHERE SalesInvoiceID =@SalesInvoiceId

END
---------------------------------------------------------
    USE[DB_9F57C4_ShippingTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateSalesInvoiceDetails]    Script Date: 22-10-2019 06:02:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc[dbo].[SP_UpdateSalesInvoiceDetails]
    (
@SalesInvoiceDetailID int = 0 output,
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

UPDATE[SalesInvoiceDetails]  SET
[SalesInvoiceID] =@SalesInvoiceID,
[ProductID] =@ProductID,
[Quantity] =@Quantity,
[ItemUnitID] =@ItemUnitID,
[Rate] =@Rate,
[RateLC] =@RateLC,
[RateFC] =@RateFC,
[Value] =@Value,
[ValueLC] =@ValueLC,
[ValueFC] =@ValueFC,
[Tax] =@Tax,
[NetValue] =@NetValue,
[JobID] =@JobID,
[Description] =@Description
WHERE SalesInvoiceDetailID =@SalesInvoiceDetailID

END
------------------------------------------------------------------
-- oct 28 -- 

/****** Object:  StoredProcedure [dbo].[SP_GetSalesInvoiceDetailsByID]    Script Date: 10/28/2019 1:01:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC[dbo].[SP_GetSalesInvoiceDetailsByID]
    (
@SalesInvoiceID int
)
AS
BEGIN
SELECT sd.*,j.JobCode,p.ProductName FROM SalesInvoiceDetails as sd
left join JobGeneration as j on sd.JobID = j.JobID
left join ProductServices as p on sd.ProductID = p.ProductID
 where sd.SalesInvoiceID = @SalesInvoiceID
END

GO
/****** Object:  StoredProcedure [dbo].[SP_GetPurchaseInvoiceDetailsByID]    Script Date: 10/28/2019 7:37:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC[dbo].[SP_GetPurchaseInvoiceDetailsByID]
    (
@PurchaseInvoiceID int
)
AS
BEGIN
SELECT pid.*,h.AcHead,j.JobCode,p.ProductName FROM PurchaseInvoiceDetails as pid
left join AcHead as h on pid.AcHeadID = h.AcHeadID
left join JobGeneration as j on pid.JobID = j.JobID
left join ProductServices as p on pid.ProductID = p.ProductID
 where pid.PurchaseInvoiceID = @PurchaseInvoiceID
END

-- oct 31 --

GO
/****** Object:  StoredProcedure [dbo].[SP_GetAllJobsDetailsForDropdown]    Script Date: 10/31/2019 2:44:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Proc [dbo].[SP_GetAllJobsDetailsForDropdown]
@JobCode nvarchar(50) = ''
AS
/*************
EXEC SP_GetAllJobsDetailsForDropdown @JobCode = 'AC1'
*****************/
BEGIN
select top 200
j.JobID as [JobID],
jt.JobDescription,
j.JobCode
from JobGeneration j 
left outer join JobType jt on j.JobTypeID=jt.JobTypeID 
left outer join Employees E on j.EmployeeID=E.EmployeeID   
WHERE (@JobCode = '' OR j.JobCode LIKE CONCAT('%', @JobCode ,'%')) ORDER by JobID desc
END

---

/****** Object:  StoredProcedure [dbo].[SP_GetInvoiceReport]    Script Date: 10/31/2019 2:12:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[SP_GetSalesInvoiceReport]
(
@SalesInvoiceID int
)
AS
BEGIN
SELECT si.SalesInvoiceNo,REPLACE(CONVERT(NVARCHAR(20),si.SalesInvoiceDate,106),' ','-') as SalesInvoiceDate
,si.QuotationNumber,si.Reference,si.LPOReference,c1.Customer, e.EmployeeName,cr.CurrencyName,si.ExchangeRate
,si.CreditDays,REPLACE(CONVERT(NVARCHAR(20),si.DueDate,106),' ','-') as DueDate,si.Remarks
,case when si.DiscountType = 1 THEN 'Percentage' WHEN si.DiscountType = 2 THEN 'Amount' ELSE '' END as DiscountType
,si.DiscountValueLC,si.DiscountValueFC
 FROM SalesInvoice as si LEFT JOIN CUSTOMER as c1 on si.CustomerID = c1.CustomerID
 LEFT JOIn Employees as e on si.EmployeeID = e.EmployeeID
 LEFT JOIN CurrencyMaster as cr on si.CurrencyID = cr.CurrencyID
 WHERE SalesInvoiceID = @SalesInvoiceID;

 SELECT p.ProductName,sd.Quantity,iu.ItemUnit,sd.RateType,sd.RateLC,sd.RateFC,sd.ValueLC,sd.ValueFC,sd.Tax,sd.NetValue
 ,j.JobCode,sd.Description FROM SalesInvoiceDetails as sd
 LEFT JOIN ProductServices as p on sd.ProductID = p.ProductID
 LEFT JOIN ItemUnit as iu on sd.ItemUnitID = iu.ItemUnitID
 LEFT JOIN JobGeneration as j on sd.JobID = j.JobID
 WHERE SalesInvoiceID = @SalesInvoiceID;


END

-----

/****** Object:  StoredProcedure [dbo].[SP_GetInvoiceReport]    Script Date: 10/31/2019 2:12:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[SP_GetPurchaseInvoiceReport]
(
@PurchaseInvoiceID int
)
AS
BEGIN
SELECT pit.PurchaseInvoiceNo,REPLACE(CONVERT(NVARCHAR(20),pit.PurchaseInvoiceDate,106),' ','-') as PurchaseInvoiceDate
,pit.QuotationNumber,pit.Reference,pit.LPOReference,s1.SupplierName, e.EmployeeName,cr.CurrencyName,pit.ExchangeRate
,pit.CreditDays,REPLACE(CONVERT(NVARCHAR(20),pit.DueDate,106),' ','-') as DueDate,pit.Remarks
,case when pit.DiscountType = 1 THEN 'Percentage' WHEN pit.DiscountType = 2 THEN 'Amount' ELSE '' END as DiscountType
,pit.DiscountValueLC,pit.DiscountValueFC
 FROM PurchaseInvoice as pit LEFT JOIN Supplier as s1 on s1.SupplierID = pit.SupplierID
 LEFT JOIn Employees as e on pit.EmployeeID = e.EmployeeID
 LEFT JOIN CurrencyMaster as cr on pit.CurrencyID = cr.CurrencyID
 WHERE pit.PurchaseInvoiceID = @PurchaseInvoiceID;

 SELECT p.ProductName,pd.Quantity,iu.ItemUnit,pd.Rate as RateLC,pd.RateFC,pd.Value as ValueLC,pd.ValueFC,pd.Tax,pd.NetValue
 ,j.JobCode,pd.Description FROM PurchaseInvoiceDetails as pd
 LEFT JOIN ProductServices as p on pd.ProductID = p.ProductID
 LEFT JOIN ItemUnit as iu on pd.ItemUnitID = iu.ItemUnitID
 LEFT JOIN JobGeneration as j on pd.JobID = j.JobID
 WHERE pd.PurchaseInvoiceID = @PurchaseInvoiceID;


END


-- sethu -- oct 4

ALTER TABLE CostUpdation Add TransactionDate datetime;
ALTER TABLE CostUpdation Add InvoiceAmount decimal(18,2);

alter table ShippingAgent alter column [ExportCode] [nvarchar](200) NULL

--- nov 13
ALTER TABLE AcJournalMaster ADD TransactionNo nvarchar(100)

-- nov 19
alter table AcAnalysisHeadAllocation add primary key (AcAnalysisHeadAllocationID);

--dec 13
alter table RecPayDetails ADD JobID int
