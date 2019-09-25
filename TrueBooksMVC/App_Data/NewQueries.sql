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

