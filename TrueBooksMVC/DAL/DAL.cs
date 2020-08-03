using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using DAL;
using System.Collections;
using TrueBooksMVC.Models;
//using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace TrueBooksMVC
{
    public class DAL
    {
        public static int SP_InsertSalesInvoice(SalesInvoice SI)
        {
            int i = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "SP_InsertSalesInvoice";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@SalesInvoiceId", SqlDbType.Int);
            cmd.Parameters["@SalesInvoiceId"].Value = 0;
            cmd.Parameters["@SalesInvoiceId"].Direction = ParameterDirection.InputOutput;

            cmd.Parameters.Add("@SalesInvoiceNo", SqlDbType.NVarChar);
            if (SI.SalesInvoiceNo != null)
            {
                cmd.Parameters["@SalesInvoiceNo"].Value = SI.SalesInvoiceNo;
            }
            else
            {
                cmd.Parameters["@SalesInvoiceNo"].Value = "";
            }

            cmd.Parameters.Add("@SalesInvoiceDate", SqlDbType.DateTime);
            if(SI.SalesInvoiceDate == DateTime.MinValue)
            {
                cmd.Parameters["@SalesInvoiceDate"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@SalesInvoiceDate"].Value = SI.SalesInvoiceDate;
            }
            

            cmd.Parameters.Add("@Reference", SqlDbType.NVarChar);
            cmd.Parameters["@Reference"].Value = SI.Reference;

            cmd.Parameters.Add("@LPOReference", SqlDbType.NVarChar);
            cmd.Parameters["@LPOReference"].Value = SI.LPOReference;

            cmd.Parameters.Add("@CustomerID", SqlDbType.Int);
            cmd.Parameters["@CustomerID"].Value = SI.CustomerID;

            cmd.Parameters.Add("@EmployeeeID", SqlDbType.Int);
            cmd.Parameters["@EmployeeeID"].Value = SI.EmployeeID;

            cmd.Parameters.Add("@QuotationNumber", SqlDbType.NVarChar);
            cmd.Parameters["@QuotationNumber"].Value = SI.QuotationNumber;

            cmd.Parameters.Add("@CurrencyID", SqlDbType.Int);
            cmd.Parameters["@CurrencyID"].Value = SI.CurrencyID;

            cmd.Parameters.Add("@ExchangeRate", SqlDbType.Decimal);
            cmd.Parameters["@ExchangeRate"].Value = SI.ExchangeRate;

            cmd.Parameters.Add("@CreditDays", SqlDbType.Int);
            if (SI.SalesInvoiceNo != null)
            {
                cmd.Parameters["@CreditDays"].Value = SI.CreditDays;
            }
            else
            {
                cmd.Parameters["@CreditDays"].Value = "";
            }
            cmd.Parameters.Add("@DueDate", SqlDbType.DateTime);
            if(SI.DueDate == DateTime.MinValue)
            {
                cmd.Parameters["@DueDate"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@DueDate"].Value = SI.DueDate;
            }
            

            cmd.Parameters.Add("@AcJouranalID", SqlDbType.Int);
            if (SI.SalesInvoiceNo != null)
            {
                cmd.Parameters["@AcJouranalID"].Value = SI.AcJournalID;
            }
            else
            {
                cmd.Parameters["@AcJouranalID"].Value = "";
            }

             cmd.Parameters.Add("@BranchID", SqlDbType.Int);
            cmd.Parameters["@BranchID"].Value = SI.BranchID;

            cmd.Parameters.Add("@Discount", SqlDbType.Decimal);
            cmd.Parameters["@Discount"].Value = SI.Discount;

            cmd.Parameters.Add("@StatusDiscountAmt", SqlDbType.Bit);
            cmd.Parameters["@StatusDiscountAmt"].Value = SI.StatusDiscountAmt;
         
            cmd.Parameters.Add("@OtherCharges", SqlDbType.Decimal);
            cmd.Parameters["@OtherCharges"].Value = SI.OtherCharges;

            cmd.Parameters.Add("@PaymentTerm", SqlDbType.NVarChar);
            cmd.Parameters["@PaymentTerm"].Value = SI.PaymentTerm;

            cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar);
            cmd.Parameters["@Remarks"].Value = SI.Remarks;

            cmd.Parameters.Add("@FYearID", SqlDbType.Int);
            cmd.Parameters["@FYearID"].Value = SI.FYearID;

            cmd.Parameters.Add("@DeliveryId", SqlDbType.Int);
            cmd.Parameters["@DeliveryId"].Value = SI.DeliveryId;

            cmd.Parameters.Add("@DiscountType", SqlDbType.Int);
            cmd.Parameters["@DiscountType"].Value = SI.DiscountType;

            cmd.Parameters.Add("@DiscountValueLC", SqlDbType.Decimal);
            cmd.Parameters["@DiscountValueLC"].Value = SI.DiscountValueLC;

            cmd.Parameters.Add("@DiscountValueFC", SqlDbType.Decimal);
            cmd.Parameters["@DiscountValueFC"].Value = SI.DiscountValueFC;

            cmd.Parameters.Add("@IsShipping", SqlDbType.Bit);
            cmd.Parameters["@IsShipping"].Value = SI.IsShipping;

            try
            {
                cmd.Connection.Open();
                i = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return Convert.ToInt32(cmd.Parameters["@SalesInvoiceId"].Value);
        }
        public static int SP_UpdateSalesInvoice(SalesInvoice SI)
        {
            int i = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "SP_UpdateSalesInvoice";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@SalesInvoiceId", SqlDbType.Int);
            cmd.Parameters["@SalesInvoiceId"].Value = SI.SalesInvoiceID;
            // cmd.Parameters["@SalesInvoiceId"].Direction = ParameterDirection.InputOutput;
            cmd.Parameters.Add("@SalesInvoiceNo", SqlDbType.NVarChar);
            if (SI.SalesInvoiceNo != null)
            {
                cmd.Parameters["@SalesInvoiceNo"].Value = SI.SalesInvoiceNo;
            }
            else
            {
                cmd.Parameters["@SalesInvoiceNo"].Value = "";
            }

            cmd.Parameters.Add("@SalesInvoiceDate", SqlDbType.DateTime);
            if (SI.SalesInvoiceDate == DateTime.MinValue)
            {
                cmd.Parameters["@SalesInvoiceDate"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@SalesInvoiceDate"].Value = SI.SalesInvoiceDate;
            }


            cmd.Parameters.Add("@Reference", SqlDbType.NVarChar);
            cmd.Parameters["@Reference"].Value = SI.Reference;

            cmd.Parameters.Add("@LPOReference", SqlDbType.NVarChar);
            cmd.Parameters["@LPOReference"].Value = SI.LPOReference;

            cmd.Parameters.Add("@CustomerID", SqlDbType.Int);
            cmd.Parameters["@CustomerID"].Value = SI.CustomerID;

            cmd.Parameters.Add("@EmployeeeID", SqlDbType.Int);
            cmd.Parameters["@EmployeeeID"].Value = SI.EmployeeID;

            cmd.Parameters.Add("@QuotationNumber", SqlDbType.NVarChar);
            cmd.Parameters["@QuotationNumber"].Value = SI.QuotationNumber;

            cmd.Parameters.Add("@CurrencyID", SqlDbType.Int);
            cmd.Parameters["@CurrencyID"].Value = SI.CurrencyID;

            cmd.Parameters.Add("@ExchangeRate", SqlDbType.Decimal);
            cmd.Parameters["@ExchangeRate"].Value = SI.ExchangeRate;

            cmd.Parameters.Add("@CreditDays", SqlDbType.Int);
            if (SI.SalesInvoiceNo != null)
            {
                cmd.Parameters["@CreditDays"].Value = SI.CreditDays;
            }
            else
            {
                cmd.Parameters["@CreditDays"].Value = "";
            }
            cmd.Parameters.Add("@DueDate", SqlDbType.DateTime);
            if (SI.DueDate == DateTime.MinValue)
            {
                cmd.Parameters["@DueDate"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@DueDate"].Value = SI.DueDate;
            }


            cmd.Parameters.Add("@AcJouranalID", SqlDbType.Int);
            if (SI.SalesInvoiceNo != null)
            {
                cmd.Parameters["@AcJouranalID"].Value = SI.AcJournalID;
            }
            else
            {
                cmd.Parameters["@AcJouranalID"].Value = "";
            }

            cmd.Parameters.Add("@BranchID", SqlDbType.Int);
            cmd.Parameters["@BranchID"].Value = SI.BranchID;

            cmd.Parameters.Add("@Discount", SqlDbType.Decimal);
            cmd.Parameters["@Discount"].Value = SI.Discount;

            cmd.Parameters.Add("@StatusDiscountAmt", SqlDbType.Bit);
            cmd.Parameters["@StatusDiscountAmt"].Value = SI.StatusDiscountAmt;

            cmd.Parameters.Add("@OtherCharges", SqlDbType.Decimal);
            cmd.Parameters["@OtherCharges"].Value = SI.OtherCharges;

            cmd.Parameters.Add("@PaymentTerm", SqlDbType.NVarChar);
            cmd.Parameters["@PaymentTerm"].Value = SI.PaymentTerm;

            cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar);
            cmd.Parameters["@Remarks"].Value = SI.Remarks;

            cmd.Parameters.Add("@FYearID", SqlDbType.Int);
            cmd.Parameters["@FYearID"].Value = SI.FYearID;

            cmd.Parameters.Add("@DeliveryId", SqlDbType.Int);
            cmd.Parameters["@DeliveryId"].Value = SI.DeliveryId;

            cmd.Parameters.Add("@DiscountType", SqlDbType.Int);
            cmd.Parameters["@DiscountType"].Value = SI.DiscountType;

            cmd.Parameters.Add("@DiscountValueLC", SqlDbType.Decimal);
            cmd.Parameters["@DiscountValueLC"].Value = SI.DiscountValueLC;

            cmd.Parameters.Add("@DiscountValueFC", SqlDbType.Decimal);
            cmd.Parameters["@DiscountValueFC"].Value = SI.DiscountValueFC;

            try
            {
                cmd.Connection.Open();
                i = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return Convert.ToInt32(cmd.Parameters["@SalesInvoiceId"].Value);
        }

        public static int SP_DeleteSalesInvoice(int Id)
        {
            int i = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "SP_DeleteSalesInvoice";
            cmd.CommandType = CommandType.StoredProcedure;
          
            cmd.Parameters.Add("@SalesInvoiceId", SqlDbType.Int);
            cmd.Parameters["@SalesInvoiceId"].Value = Id;

            try
            {
                cmd.Connection.Open();
                i = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return Convert.ToInt32(cmd.Parameters["@SalesInvoiceId"].Value);
        }




        public static int SP_InsertPurchaseInvoice(PurchaseInvoice PI)
        {
            int i = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "SP_InsertPurchaseInvoice";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@PurchaseInvoiceId", SqlDbType.Int);
            cmd.Parameters["@PurchaseInvoiceId"].Value = 0;
            cmd.Parameters["@PurchaseInvoiceId"].Direction = ParameterDirection.InputOutput;

            cmd.Parameters.Add("@PurchaseInvoiceNo", SqlDbType.NVarChar);
            if (PI.PurchaseInvoiceNo != null)
            {
                cmd.Parameters["@PurchaseInvoiceNo"].Value = PI.PurchaseInvoiceNo;
            }
            else
            {
                cmd.Parameters["@PurchaseInvoiceNo"].Value = "";
            }
            

            cmd.Parameters.Add("@PurchaseInvoiceDate", SqlDbType.DateTime);
            if (PI.PurchaseInvoiceDate == DateTime.MinValue)
            {
                cmd.Parameters["@PurchaseInvoiceDate"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@PurchaseInvoiceDate"].Value = PI.PurchaseInvoiceDate;
            }

            cmd.Parameters.Add("@Reference", SqlDbType.NVarChar);
            cmd.Parameters["@Reference"].Value = PI.Reference;

            cmd.Parameters.Add("@LPOReference", SqlDbType.NVarChar);
            cmd.Parameters["@LPOReference"].Value = PI.LPOReference;

            cmd.Parameters.Add("@SupplierID", SqlDbType.Int);
            cmd.Parameters["@SupplierID"].Value = PI.SupplierID;

            cmd.Parameters.Add("@EmployeeeID", SqlDbType.Int);
            cmd.Parameters["@EmployeeeID"].Value = PI.EmployeeID;

            cmd.Parameters.Add("@QuotationNumber", SqlDbType.NVarChar);
            cmd.Parameters["@QuotationNumber"].Value = PI.QuotationNumber;

            cmd.Parameters.Add("@CurrencyID", SqlDbType.Int);
            cmd.Parameters["@CurrencyID"].Value = PI.CurrencyID;

            cmd.Parameters.Add("@ExchangeRate", SqlDbType.Decimal);
            cmd.Parameters["@ExchangeRate"].Value = PI.ExchangeRate;

            cmd.Parameters.Add("@CreditDays", SqlDbType.Int);
            if (PI.CreditDays != null)
            {
                cmd.Parameters["@CreditDays"].Value = PI.CreditDays;
            }
            else
            {
                cmd.Parameters["@CreditDays"].Value = "";
            }
            cmd.Parameters.Add("@DueDate", SqlDbType.DateTime);
            if (PI.DueDate == DateTime.MinValue)
            {
                cmd.Parameters["@DueDate"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@DueDate"].Value = PI.DueDate;
            }


            cmd.Parameters.Add("@AcJouranalID", SqlDbType.Int);
            if (PI.AcJournalID != null)
            {
                cmd.Parameters["@AcJouranalID"].Value = PI.AcJournalID;
            }
            else
            {
                cmd.Parameters["@AcJouranalID"].Value = "";
            }

            cmd.Parameters.Add("@BranchID", SqlDbType.Int);
            cmd.Parameters["@BranchID"].Value = PI.BranchID;

            cmd.Parameters.Add("@Discount", SqlDbType.Decimal);
            cmd.Parameters["@Discount"].Value = PI.Discount;

            cmd.Parameters.Add("@StatusDiscountAmt", SqlDbType.Bit);
            cmd.Parameters["@StatusDiscountAmt"].Value = PI.StatusDiscountAmt;

            cmd.Parameters.Add("@OtherCharges", SqlDbType.Decimal);
            cmd.Parameters["@OtherCharges"].Value = PI.OtherCharges;

            cmd.Parameters.Add("@PaymentTerm", SqlDbType.NVarChar);
            cmd.Parameters["@PaymentTerm"].Value = PI.PaymentTerm;

            cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar);
            cmd.Parameters["@Remarks"].Value = PI.Remarks;

            cmd.Parameters.Add("@FYearID", SqlDbType.Int);
            cmd.Parameters["@FYearID"].Value = PI.FYearID;                     

            cmd.Parameters.Add("@DiscountType", SqlDbType.Int);
            cmd.Parameters["@DiscountType"].Value = PI.DiscountType;

            cmd.Parameters.Add("@DiscountValueLC", SqlDbType.Decimal);
            cmd.Parameters["@DiscountValueLC"].Value = PI.DiscountValueLC;

            cmd.Parameters.Add("@DiscountValueFC", SqlDbType.Decimal);
            cmd.Parameters["@DiscountValueFC"].Value = PI.DiscountValueFC;

            cmd.Parameters.Add("@IsShipping", SqlDbType.Bit);
            cmd.Parameters["@IsShipping"].Value = PI.IsShipping;



            try
            {
                cmd.Connection.Open();
                i = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return Convert.ToInt32(cmd.Parameters["@PurchaseInvoiceId"].Value);
        }

        public static int SP_UpdatePurchaseInvoice(PurchaseInvoice PI)
        {
            int i = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "SP_UpdatePurchaseInvoice";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@PurchaseInvoiceId", SqlDbType.Int);
            cmd.Parameters["@PurchaseInvoiceId"].Value = PI.PurchaseInvoiceID;
            //cmd.Parameters["@PurchaseInvoiceId"].Direction = ParameterDirection.InputOutput;
            
            cmd.Parameters.Add("@PurchaseInvoiceNo", SqlDbType.NVarChar);
            if (PI.PurchaseInvoiceNo != null)
            {
                cmd.Parameters["@PurchaseInvoiceNo"].Value = PI.PurchaseInvoiceNo;
            }
            else
            {
                cmd.Parameters["@PurchaseInvoiceNo"].Value = "";
            }

            cmd.Parameters.Add("@PurchaseInvoiceDate", SqlDbType.DateTime);
            if (PI.PurchaseInvoiceDate == DateTime.MinValue)
            {
                cmd.Parameters["@PurchaseInvoiceDate"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@PurchaseInvoiceDate"].Value = PI.PurchaseInvoiceDate;
            }

            cmd.Parameters.Add("@Reference", SqlDbType.NVarChar);
            cmd.Parameters["@Reference"].Value = PI.Reference;

            cmd.Parameters.Add("@LPOReference", SqlDbType.NVarChar);
            cmd.Parameters["@LPOReference"].Value = PI.LPOReference;

            cmd.Parameters.Add("@SupplierID", SqlDbType.Int);
            cmd.Parameters["@SupplierID"].Value = PI.SupplierID;

            cmd.Parameters.Add("@EmployeeeID", SqlDbType.Int);
            cmd.Parameters["@EmployeeeID"].Value = PI.EmployeeID;

            cmd.Parameters.Add("@QuotationNumber", SqlDbType.NVarChar);
            cmd.Parameters["@QuotationNumber"].Value = PI.QuotationNumber;

            cmd.Parameters.Add("@CurrencyID", SqlDbType.Int);
            cmd.Parameters["@CurrencyID"].Value = PI.CurrencyID;

            cmd.Parameters.Add("@ExchangeRate", SqlDbType.Decimal);
            cmd.Parameters["@ExchangeRate"].Value = PI.ExchangeRate;

            cmd.Parameters.Add("@CreditDays", SqlDbType.Int);
            if (PI.CreditDays != null)
            {
                cmd.Parameters["@CreditDays"].Value = PI.CreditDays;
            }
            else
            {
                cmd.Parameters["@CreditDays"].Value = "";
            }
            cmd.Parameters.Add("@DueDate", SqlDbType.DateTime);
            if (PI.DueDate == DateTime.MinValue)
            {
                cmd.Parameters["@DueDate"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@DueDate"].Value = PI.DueDate;
            }


            cmd.Parameters.Add("@AcJouranalID", SqlDbType.Int);
            if (PI.AcJournalID != null)
            {
                cmd.Parameters["@AcJouranalID"].Value = PI.AcJournalID;
            }
            else
            {
                cmd.Parameters["@AcJouranalID"].Value = "";
            }

            cmd.Parameters.Add("@BranchID", SqlDbType.Int);
            cmd.Parameters["@BranchID"].Value = PI.BranchID;

            cmd.Parameters.Add("@Discount", SqlDbType.Decimal);
            cmd.Parameters["@Discount"].Value = PI.Discount;

            cmd.Parameters.Add("@StatusDiscountAmt", SqlDbType.Bit);
            cmd.Parameters["@StatusDiscountAmt"].Value = PI.StatusDiscountAmt;

            cmd.Parameters.Add("@OtherCharges", SqlDbType.Decimal);
            cmd.Parameters["@OtherCharges"].Value = PI.OtherCharges;

            cmd.Parameters.Add("@PaymentTerm", SqlDbType.NVarChar);
            cmd.Parameters["@PaymentTerm"].Value = PI.PaymentTerm;

            cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar);
            cmd.Parameters["@Remarks"].Value = PI.Remarks;

            cmd.Parameters.Add("@FYearID", SqlDbType.Int);
            cmd.Parameters["@FYearID"].Value = PI.FYearID;

            cmd.Parameters.Add("@DiscountType", SqlDbType.Int);
            cmd.Parameters["@DiscountType"].Value = PI.DiscountType;

            cmd.Parameters.Add("@DiscountValueLC", SqlDbType.Decimal);
            cmd.Parameters["@DiscountValueLC"].Value = PI.DiscountValueLC;

            cmd.Parameters.Add("@DiscountValueFC", SqlDbType.Decimal);
            cmd.Parameters["@DiscountValueFC"].Value = PI.DiscountValueFC;


            try
            {
                cmd.Connection.Open();
                i = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return Convert.ToInt32(cmd.Parameters["@PurchaseInvoiceId"].Value);
        }

        public static int SP_DeletePurchaseInvoice(int Id)
        {
            int i = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "SP_DeletePurchaseInvoice";
            cmd.CommandType = CommandType.StoredProcedure;
           
            cmd.Parameters.Add("@PurchaseInvoiceId", SqlDbType.Int);
            cmd.Parameters["@PurchaseInvoiceId"].Value = Id;

            try
            {
                cmd.Connection.Open();
                i = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return i;
        }

        public static int SP_DeletePurchaseInvoiceDetails(int Id)
        {
            int i = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "SP_DeletePurchaseInvoiceDetails";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@PurchaseInvoiceId", SqlDbType.Int);
            cmd.Parameters["@PurchaseInvoiceId"].Value = Id;

            try
            {
                cmd.Connection.Open();
                i = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return i;
        }

        public static int AddPurchaseInvoiceDetail(PurchaseInvoiceDetail PID)
        {
            int i = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "SP_InsertPurchaseInvoiceDetails";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@PurchaseInvoiceDetailID", SqlDbType.Int);
            cmd.Parameters["@PurchaseInvoiceDetailID"].Value = 0;
            cmd.Parameters["@PurchaseInvoiceDetailID"].Direction = ParameterDirection.InputOutput;

            cmd.Parameters.Add("@PurchaseInvoiceID", SqlDbType.Int);
            if (PID.PurchaseInvoiceID != null)
            {
                cmd.Parameters["@PurchaseInvoiceID"].Value = PID.PurchaseInvoiceID;
            }
            else
            {
                cmd.Parameters["@PurchaseInvoiceID"].Value = 0;
            }            

         
            cmd.Parameters.Add("@ProductID", SqlDbType.Int);

            if (PID.ProductID != null)
            {
                cmd.Parameters["@ProductID"].Value = PID.ProductID;
            }
            else
            {
                cmd.Parameters["@ProductID"].Value = 0;
            }
            cmd.Parameters.Add("@Quantity", SqlDbType.Int);
            cmd.Parameters["@Quantity"].Value = PID.Quantity;

            cmd.Parameters.Add("@ItemUnitID", SqlDbType.Int);
            cmd.Parameters["@ItemUnitID"].Value = PID.ItemUnitID;

            cmd.Parameters.Add("@Rate", SqlDbType.Int);
            cmd.Parameters["@Rate"].Value = PID.Rate;

            cmd.Parameters.Add("@RateFC", SqlDbType.Decimal);
            cmd.Parameters["@RateFC"].Value = PID.RateFC;

            cmd.Parameters.Add("@Value", SqlDbType.Int);
            cmd.Parameters["@Value"].Value = PID.Value;

            cmd.Parameters.Add("@ValueFC", SqlDbType.Decimal);
            cmd.Parameters["@ValueFC"].Value = PID.ValueFC;

                cmd.Parameters.Add("@Taxprec", SqlDbType.Decimal);
                cmd.Parameters["@Taxprec"].Value = PID.Taxprec;

                cmd.Parameters.Add("@Tax", SqlDbType.Decimal);
                cmd.Parameters["@Tax"].Value = PID.Tax;

                cmd.Parameters.Add("@NetValue", SqlDbType.Decimal);
                cmd.Parameters["@NetValue"].Value = PID.NetValue;

                cmd.Parameters.Add("@AcHeadID", SqlDbType.Int);
                cmd.Parameters["@AcHeadID"].Value = PID.AcHeadID;

                cmd.Parameters.Add("@JobID", SqlDbType.Int);
                cmd.Parameters["@JobID"].Value = PID.JobID;
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar);
                cmd.Parameters["@Description"].Value = PID.Description;





                try
            {
                cmd.Connection.Open();
                i = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return Convert.ToInt32(cmd.Parameters["@PurchaseInvoiceDetailID"].Value);
        }

        public static int UpdatePurchaseInvoiceDetail(PurchaseInvoiceDetail PID)
        {
            int i = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "SP_UpdatePurchaseInvoiceDetails";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@PurchaseInvoiceDetailID", SqlDbType.Int);
            cmd.Parameters["@PurchaseInvoiceDetailID"].Value = PID.PurchaseInvoiceDetailID;
           // cmd.Parameters["@PurchaseInvoiceDetailID"].Direction = ParameterDirection.InputOutput;

            cmd.Parameters.Add("@PurchaseInvoiceID", SqlDbType.Int);
            if (PID.PurchaseInvoiceID != null)
            {
                cmd.Parameters["@PurchaseInvoiceID"].Value = PID.PurchaseInvoiceID;
            }
            else
            {
                cmd.Parameters["@PurchaseInvoiceID"].Value = 0;
            }


            cmd.Parameters.Add("@ProductID", SqlDbType.Int);

            if (PID.ProductID != null)
            {
                cmd.Parameters["@ProductID"].Value = PID.ProductID;
            }
            else
            {
                cmd.Parameters["@ProductID"].Value = 0;
            }
            cmd.Parameters.Add("@Quantity", SqlDbType.Int);
            cmd.Parameters["@Quantity"].Value = PID.Quantity;

            cmd.Parameters.Add("@ItemUnitID", SqlDbType.Int);
            cmd.Parameters["@ItemUnitID"].Value = PID.ItemUnitID;

            cmd.Parameters.Add("@Rate", SqlDbType.Int);
            cmd.Parameters["@Rate"].Value = PID.Rate;

            cmd.Parameters.Add("@RateFC", SqlDbType.Decimal);
            cmd.Parameters["@RateFC"].Value = PID.RateFC;

            cmd.Parameters.Add("@Value", SqlDbType.Int);
            cmd.Parameters["@Value"].Value = PID.Value;

            cmd.Parameters.Add("@ValueFC", SqlDbType.Decimal);
            cmd.Parameters["@ValueFC"].Value = PID.ValueFC;

            cmd.Parameters.Add("@Taxprec", SqlDbType.Decimal);
            cmd.Parameters["@Taxprec"].Value = PID.Taxprec;

            cmd.Parameters.Add("@Tax", SqlDbType.Decimal);
            cmd.Parameters["@Tax"].Value = PID.Tax;

            cmd.Parameters.Add("@NetValue", SqlDbType.Decimal);
            cmd.Parameters["@NetValue"].Value = PID.NetValue;

            cmd.Parameters.Add("@AcHeadID", SqlDbType.Int);
            cmd.Parameters["@AcHeadID"].Value = PID.AcHeadID;

            cmd.Parameters.Add("@JobID", SqlDbType.Int);
            cmd.Parameters["@JobID"].Value = PID.JobID;
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar);
            cmd.Parameters["@Description"].Value = PID.Description;





            try
            {
                cmd.Connection.Open();
                i = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return Convert.ToInt32(cmd.Parameters["@PurchaseInvoiceDetailID"].Value);
        }

        public static int SP_DeleteSalesInvoiceDetails(int Id)
        {
            int i = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "SP_DeleteSalesInvoiceDetails";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@SalesInvoiceId", SqlDbType.Int);
            cmd.Parameters["@SalesInvoiceId"].Value = Id;

            try
            {
                cmd.Connection.Open();
                i = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return i;
        }




        public static int AddSalesInvoiceDetails(Models.SalesInvoiceDetail SID)
        {
            int i = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "SP_InsertSalesInvoiceDetails";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@SalesInvoiceDetailID", SqlDbType.Int);
            cmd.Parameters["@SalesInvoiceDetailID"].Value = 0;
            cmd.Parameters["@SalesInvoiceDetailID"].Direction = ParameterDirection.InputOutput;

            cmd.Parameters.Add("@SalesInvoiceID", SqlDbType.Int);
            if (SID.SalesInvoiceID != null)
            {
                cmd.Parameters["@SalesInvoiceID"].Value = SID.SalesInvoiceID;
            }
            else
            {
                cmd.Parameters["@SalesInvoiceID"].Value = 0;
            }


            cmd.Parameters.Add("@ProductID", SqlDbType.Int);

            if (SID.ProductID != null)
            {
                cmd.Parameters["@ProductID"].Value = SID.ProductID;
            }
            else
            {
                cmd.Parameters["@ProductID"].Value = 0;
            }

            cmd.Parameters.Add("@Quantity", SqlDbType.Int);
            cmd.Parameters["@Quantity"].Value = SID.Quantity;

            cmd.Parameters.Add("@ItemUnitID", SqlDbType.Int);
            cmd.Parameters["@ItemUnitID"].Value = SID.ItemUnitID;

            cmd.Parameters.Add("@RateType", SqlDbType.NVarChar);
            cmd.Parameters["@RateType"].Value = SID.RateType;

            cmd.Parameters.Add("@RateLC", SqlDbType.Decimal);
            cmd.Parameters["@RateLC"].Value = SID.RateLC;
            cmd.Parameters.Add("@RateFC", SqlDbType.Decimal);
            cmd.Parameters["@RateFC"].Value = SID.RateFC;

            cmd.Parameters.Add("@Value", SqlDbType.Int);
            cmd.Parameters["@Value"].Value = SID.Value;

            cmd.Parameters.Add("@ValueLC", SqlDbType.Decimal);
            cmd.Parameters["@ValueLC"].Value = SID.ValueLC;

            cmd.Parameters.Add("@ValueFC", SqlDbType.Decimal);
            cmd.Parameters["@ValueFC"].Value = SID.ValueFC;                    

            cmd.Parameters.Add("@Tax", SqlDbType.Decimal);
            cmd.Parameters["@Tax"].Value = SID.Tax;

            cmd.Parameters.Add("@NetValue", SqlDbType.Decimal);
            cmd.Parameters["@NetValue"].Value = SID.NetValue;                    

            cmd.Parameters.Add("@JobID", SqlDbType.Int);
            cmd.Parameters["@JobID"].Value = SID.JobID;
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar);
            cmd.Parameters["@Description"].Value = SID.Description;

            try
            {
                cmd.Connection.Open();
                i = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return Convert.ToInt32(cmd.Parameters["@SalesInvoiceDetailID"].Value);
        }

        public static int UpdateSalesInvoiceDetails(Models.SalesInvoiceDetail SID)
        {
            int i = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "SP_UpdateSalesInvoiceDetails";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@SalesInvoiceDetailID", SqlDbType.Int);
            cmd.Parameters["@SalesInvoiceDetailID"].Value = SID.SalesInvoiceDetailID;
            //cmd.Parameters["@SalesInvoiceDetailID"].Direction = ParameterDirection.InputOutput;

            cmd.Parameters.Add("@SalesInvoiceID", SqlDbType.Int);
            if (SID.SalesInvoiceID != null)
            {
                cmd.Parameters["@SalesInvoiceID"].Value = SID.SalesInvoiceID;
            }
            else
            {
                cmd.Parameters["@SalesInvoiceID"].Value = 0;
            }


            cmd.Parameters.Add("@ProductID", SqlDbType.Int);

            if (SID.ProductID != null)
            {
                cmd.Parameters["@ProductID"].Value = SID.ProductID;
            }
            else
            {
                cmd.Parameters["@ProductID"].Value = 0;
            }

            cmd.Parameters.Add("@Quantity", SqlDbType.Int);
            cmd.Parameters["@Quantity"].Value = SID.Quantity;

            cmd.Parameters.Add("@ItemUnitID", SqlDbType.Int);
            cmd.Parameters["@ItemUnitID"].Value = SID.ItemUnitID;

            cmd.Parameters.Add("@RateType", SqlDbType.NVarChar);
            cmd.Parameters["@RateType"].Value = SID.RateType;

            cmd.Parameters.Add("@RateLC", SqlDbType.Decimal);
            cmd.Parameters["@RateLC"].Value = SID.RateLC;
            cmd.Parameters.Add("@RateFC", SqlDbType.Decimal);
            cmd.Parameters["@RateFC"].Value = SID.RateFC;

            cmd.Parameters.Add("@Value", SqlDbType.Int);
            cmd.Parameters["@Value"].Value = SID.Value;

            cmd.Parameters.Add("@ValueLC", SqlDbType.Decimal);
            cmd.Parameters["@ValueLC"].Value = SID.ValueLC;

            cmd.Parameters.Add("@ValueFC", SqlDbType.Decimal);
            cmd.Parameters["@ValueFC"].Value = SID.ValueFC;

            cmd.Parameters.Add("@Tax", SqlDbType.Decimal);
            cmd.Parameters["@Tax"].Value = SID.Tax;

            cmd.Parameters.Add("@NetValue", SqlDbType.Decimal);
            cmd.Parameters["@NetValue"].Value = SID.NetValue;

            cmd.Parameters.Add("@JobID", SqlDbType.Int);
            cmd.Parameters["@JobID"].Value = SID.JobID;
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar);
            cmd.Parameters["@Description"].Value = SID.Description;

            try
            {
                cmd.Connection.Open();
                i = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return Convert.ToInt32(cmd.Parameters["@SalesInvoiceDetailID"].Value);
        }
        public static DataSet GetSalesInvoiceDetailsById( int SalesInvoiceID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "SP_GetSalesInvoiceDetailsByID";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@SalesInvoiceId", SqlDbType.Int);
            cmd.Parameters["@SalesInvoiceId"].Value = SalesInvoiceID;
                       
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }


        public static DataSet GetPurchaseInvoiceDetailsById(int PurchaseInvoiceId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "SP_GetPurchaseInvoiceDetailsByID";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@PurchaseInvoiceId", SqlDbType.Int);
            cmd.Parameters["@PurchaseInvoiceId"].Value = PurchaseInvoiceId;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public static List<SalesInvoice> SP_GetAllSalesInvoiceByDate(Nullable<System.DateTime> fdate, Nullable<System.DateTime> tdate,bool? isshipping)
        {
            List<SalesInvoice> dtList = new List<SalesInvoice>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "SP_GetAllSalesInvoiceByDate";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@fdate", SqlDbType.DateTime);
            cmd.Parameters["@fdate"].Value = fdate;

            cmd.Parameters.Add("@tdate", SqlDbType.DateTime);
            cmd.Parameters["@tdate"].Value = tdate;
              cmd.Parameters.Add("@IsShipping", SqlDbType.Bit);
            cmd.Parameters["@IsShipping"].Value = isshipping;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds != null && ds.Tables.Count > 0)
            {
                dtList = ds.Tables[0].AsEnumerable()
                .Select(row => new SalesInvoice
                {
                    SalesInvoiceID = int.Parse(row["SalesInvoiceID"].ToString()),
                    SalesInvoiceNo = row["SalesInvoiceNo"].ToString(),
                    SalesInvoiceDate = Convert.ToDateTime(row["SalesInvoiceDate"]),
                    DueDate = Convert.ToDateTime(row["DueDate"].ToString()),
                    Remarks = row["Client"].ToString(),
                }).ToList();
            }
            return dtList;
        }

        public static List<PurchaseInvoice> SP_GetAllPurchaseInvoiceByDate(Nullable<System.DateTime> fdate, Nullable<System.DateTime> tdate,bool? isshipping)
        {
            List<PurchaseInvoice> dtList = new List<PurchaseInvoice>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "SP_GetAllPurchaseInvoiceByDate";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@fdate", SqlDbType.DateTime);
            cmd.Parameters["@fdate"].Value = fdate;

            cmd.Parameters.Add("@tdate", SqlDbType.DateTime);
            cmd.Parameters["@tdate"].Value = tdate;
            cmd.Parameters.Add("@IsShipping", SqlDbType.Bit);
            cmd.Parameters["@IsShipping"].Value = isshipping;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds != null && ds.Tables.Count > 0)
            {
                dtList = ds.Tables[0].AsEnumerable()
                .Select(row => new PurchaseInvoice
                {
                    PurchaseInvoiceID = int.Parse(row["PurchaseInvoiceID"].ToString()),
                    PurchaseInvoiceNo = row["PurchaseInvoiceNo"].ToString(),
                    PurchaseInvoiceDate = row["PurchaseInvoiceDate"] != DBNull.Value ? Convert.ToDateTime(row["PurchaseInvoiceDate"]) : DateTime.Now,
                    DueDate = row["DueDate"]  != DBNull.Value ?  Convert.ToDateTime(row["DueDate"].ToString()) : DateTime.Now,
                    Remarks = row["SupplierName"].ToString(),
                }).ToList();
            }
            return dtList;
        }

        public static List<SP_GetAllJobsDetails_Result> GetAllJobsDetails(string Term)
        {
            List<SP_GetAllJobsDetails_Result> dtList = new List<SP_GetAllJobsDetails_Result>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "SP_GetAllJobsDetailsForDropdown";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@JobCode", SqlDbType.NVarChar);
            cmd.Parameters["@JobCode"].Value = Term;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds != null && ds.Tables.Count > 0)
            {
                dtList = ds.Tables[0].AsEnumerable()
                .Select(row => new SP_GetAllJobsDetails_Result
                {
                    JobID = int.Parse(row["JobID"].ToString()),
                   // InvoiceNo = int.Parse(row["InvoiceNo"].ToString()),
                  //  InvoiceDate = row["InvoiceDate"] != DBNull.Value ? Convert.ToDateTime(row["InvoiceDate"]) : DateTime.Now,
                  //  JobDate = row["JobDate"] != DBNull.Value ? Convert.ToDateTime(row["JobDate"].ToString()) : DateTime.Now,
                    JobDescription = row["JobDescription"].ToString(),
                    JobCode = row["JobCode"].ToString(),
                 //   CostUpdatedOrNot = row["CostUpdatedOrNot"].ToString(),
                 //   Shipper = row["Shipper"].ToString(),
                  //  Consignee = row["Consignee"].ToString(),
                  //  Customer = row["Customer"].ToString()
    }).ToList();
            }
            return dtList;
        }


       public static DataSet SP_JobRegisterReportPrintByJObID(int JobId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "SP_JobRegisterReportPrintByJObID";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@jobId", SqlDbType.Int);
            cmd.Parameters["@jobId"].Value = JobId;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public static DataSet SP_GetSalesInvoiceReport(int SalesInvoiceID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "SP_GetSalesInvoiceReport";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@SalesInvoiceID", SqlDbType.Int);
            cmd.Parameters["@SalesInvoiceID"].Value = SalesInvoiceID;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public static DataSet SP_GetPurchaseInvoiceReport(int PurchaseInvoiceID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "SP_GetPurchaseInvoiceReport";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@PurchaseInvoiceID", SqlDbType.Int);
            cmd.Parameters["@PurchaseInvoiceID"].Value = PurchaseInvoiceID;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public static List<GetAllCostUpdation_Result> SP_GetAllCostUpdation(DateTime FromDate,DateTime ToDate)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "SP_GetAllCostUpdation";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@FromDate", SqlDbType.DateTime);
            cmd.Parameters["@FromDate"].Value = FromDate;

            cmd.Parameters.Add("@ToDate", SqlDbType.DateTime);
            cmd.Parameters["@ToDate"].Value = ToDate;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            List<GetAllCostUpdation_Result> objList = new List<GetAllCostUpdation_Result>();
            GetAllCostUpdation_Result obj;
           if (ds != null && ds.Tables.Count > 0)
            {
                for(int i=0; i < ds.Tables[0].Rows.Count; i++)
                {
                    obj = new GetAllCostUpdation_Result();
                    obj.SINo = Common.ParseInt(ds.Tables[0].Rows[i]["SINo"].ToString());
                    obj.SupplierInvoiceNumber = ds.Tables[0].Rows[i]["SupplierInvoiceNumber"].ToString();
                    if(ds.Tables[0].Rows[i]["InvoiceDate"] != DBNull.Value)
                    {
                        obj.InvoiceDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["InvoiceDate"].ToString());
                    }

                    obj.SupplierName = ds.Tables[0].Rows[i]["SupplierName"].ToString();
                    obj.CurrencyName = ds.Tables[0].Rows[i]["CurrencyName"].ToString();
                    obj.InvoiceAmount = Common.ParseDecimal(ds.Tables[0].Rows[i]["InvoiceAmount"].ToString());
                    obj.JobNumbers = ds.Tables[0].Rows[i]["JobNumbers"].ToString();
                    obj.JobDates = ds.Tables[0].Rows[i]["JobDates"].ToString();
                    obj.JobValues = ds.Tables[0].Rows[i]["JobValues"].ToString();
                    obj.YearToDateInvoiced = ds.Tables[0].Rows[i]["YearToDateInvoiced"].ToString();
                    obj.CostUpdationID = Common.ParseInt(ds.Tables[0].Rows[i]["CostUpdationID"].ToString());
                    objList.Add(obj);
                }
            }
            return objList;
        }

        public static List<costUpdationDetailVM> GetCostUpdationDetailsbyCostUpdationID(int CostUpdationID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "SP_GetCostUpdationDetailByCostID";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@CostUpdationID", SqlDbType.Int);
            cmd.Parameters["@CostUpdationID"].Value = CostUpdationID;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            List<costUpdationDetailVM> objList = new List<costUpdationDetailVM>();
            costUpdationDetailVM obj;
            if (ds != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    obj = new costUpdationDetailVM();
                    obj.CostUpdationDetailID = Common.ParseInt(ds.Tables[0].Rows[i]["CostUpdationDetailID"].ToString());
                    obj.CostUpdationID = Common.ParseInt(ds.Tables[0].Rows[i]["CostUpdationID"].ToString());
                    obj.RevenueTypeID = Common.ParseInt(ds.Tables[0].Rows[i]["RevenueTypeID"].ToString());
                    obj.ProvisionCurrencyID = Common.ParseInt(ds.Tables[0].Rows[i]["ProvisionCurrencyID"].ToString());
                    obj.ProvisionExchangeRate = Common.ParseDecimal(ds.Tables[0].Rows[i]["ProvisionExchangeRate"].ToString());
                    obj.ProvisionForeign = Common.ParseDecimal(ds.Tables[0].Rows[i]["ProvisionForeign"].ToString());
                    obj.ProvisionHome = Common.ParseDecimal(ds.Tables[0].Rows[i]["ProvisionHome"].ToString());
                    obj.SalesCurrencyID = Common.ParseInt(ds.Tables[0].Rows[i]["SalesCurrencyID"].ToString());
                    obj.SalesExchangeRate=Common.ParseDecimal(ds.Tables[0].Rows[i]["SalesExchangeRate"].ToString());
                    obj.SalesForeign = Common.ParseDecimal(ds.Tables[0].Rows[i]["SalesForeign"].ToString());
                    obj.SalesHome = Common.ParseDecimal(ds.Tables[0].Rows[i]["SalesHome"].ToString());
                    obj.Variance = Common.ParseDecimal(ds.Tables[0].Rows[i]["Variance"].ToString());
                    obj.SupplierID = Common.ParseInt(ds.Tables[0].Rows[i]["SupplierID"].ToString());
                    obj.JInvoiceID = Common.ParseInt(ds.Tables[0].Rows[i]["JInvoiceID"].ToString());
                    obj.Cost = Common.ParseDecimal(ds.Tables[0].Rows[i]["Cost"].ToString());
                    obj.PrevCostDetailID = Common.ParseInt(ds.Tables[0].Rows[i]["PrevCostDetailID"].ToString());
                    obj.AmountPaidTillDate = Common.ParseDecimal(ds.Tables[0].Rows[i]["AmountPaidTillDate"].ToString());
                    obj.InvoiceAmount = Common.ParseDecimal(ds.Tables[0].Rows[i]["InvoiceAmount"].ToString());
                    obj.PaidOrNot = ds.Tables[0].Rows[i]["PaidOrNot"].ToString();
                    obj.supplierReference = ds.Tables[0].Rows[i]["SupplierReference"].ToString();
                    obj.SupplierPayStatus = ds.Tables[0].Rows[i]["SupplierPayStatus"].ToString();
                    obj.RevenueType = ds.Tables[0].Rows[i]["RevenueType"].ToString();
                    obj.CurrencyName = ds.Tables[0].Rows[i]["CurrencyName"].ToString();

                    if (ds.Tables[0].Rows[i]["Lock"] != DBNull.Value)
                    {
                        obj.Lock = Convert.ToBoolean(ds.Tables[0].Rows[i]["Lock"]);
                    }
                    obj.JobCode = ds.Tables[0].Rows[i]["JobCode"].ToString();
                    objList.Add(obj);
                }
            }
            return objList;
         }

        public static List<AcAnalysisHeadAllocationVM> GetAcJDetailsExpenseAllocation(int AcJournalDetailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "GetAcJDetailsExpenseAllocation";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@AcJournalDetailID", SqlDbType.Int);
            cmd.Parameters["@AcJournalDetailID"].Value = AcJournalDetailID;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            List<AcAnalysisHeadAllocationVM> objList = new List<AcAnalysisHeadAllocationVM>();
            AcAnalysisHeadAllocationVM obj;
            if (ds != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    obj = new AcAnalysisHeadAllocationVM();
                    obj.AcAnalysisHeadAllocationID = Common.ParseInt(ds.Tables[0].Rows[i]["AcAnalysisHeadAllocationID"].ToString());
                    obj.AcjournalDetailID = Common.ParseInt(ds.Tables[0].Rows[i]["AcjournalDetailID"].ToString());
                    obj.AnalysisHeadID = Common.ParseInt(ds.Tables[0].Rows[i]["AnalysisHeadID"].ToString());
                    if(ds.Tables[0].Rows[i]["Amount"] == DBNull.Value)
                    {
                        obj.Amount = 0;
                    }
                    else
                    {
                        obj.Amount = Common.ParseDecimal(ds.Tables[0].Rows[i]["Amount"].ToString());
                    }
                    obj.AnalysisHead = ds.Tables[0].Rows[i]["AnalysisHead"].ToString();
                    objList.Add(obj);
                }
            }
            return objList;
        }

        public static int InsertAcJournalDetail(AcJournalDetail ObjectAcJournalDetail)
        {
            int iReturn = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "INSERT INTO AcJournalDetail(AcJournalDetailID,AcJournalID,AcHeadID,Amount,Remarks,BranchID) VALUES(@AcJournalDetailID,@AcJournalID,@AcHeadID,@Amount,@Remarks,@BranchID)";
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add("@AcJournalDetailID", SqlDbType.Int);
            cmd.Parameters["@AcJournalDetailID"].Value = ObjectAcJournalDetail.AcJournalDetailID;

            cmd.Parameters.Add("@AcJournalID", SqlDbType.Int);
            cmd.Parameters["@AcJournalID"].Value = ObjectAcJournalDetail.AcJournalID;

            cmd.Parameters.Add("@AcHeadID", SqlDbType.Int);
            cmd.Parameters["@AcHeadID"].Value = ObjectAcJournalDetail.AcHeadID;

         //   cmd.Parameters.Add("@AnalysisHeadID", SqlDbType.Int);
        //    cmd.Parameters["@AnalysisHeadID"].Value = ObjectAcJournalDetail.AnalysisHeadID;

            cmd.Parameters.Add("@Amount", SqlDbType.Money);
            cmd.Parameters["@Amount"].Value = ObjectAcJournalDetail.Amount;

            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar);
            cmd.Parameters["@Remarks"].Value = ObjectAcJournalDetail.Remarks;

            cmd.Parameters.Add("@BranchID", SqlDbType.Int);
            cmd.Parameters["@BranchID"].Value = ObjectAcJournalDetail.BranchID;

            try
            {
                cmd.Connection.Open();
                iReturn = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }catch(Exception ex)
            {

            }

            return iReturn;
        }

        public static int UpdateAcJournalDetail(AcJournalDetail ObjectAcJournalDetail)
        {
            int iReturn = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "UPDATE AcJournalDetail SET AcJournalID=@AcJournalID,AcHeadID=@AcHeadID,AnalysisHeadID=@AnalysisHeadID,Amount=@Amount,Remarks=@Remarks,BranchID=@BranchID WHERE AcJournalDetailID = @AcJournalDetailID";
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add("@AcJournalDetailID", SqlDbType.Int);
            cmd.Parameters["@AcJournalDetailID"].Value = ObjectAcJournalDetail.AcJournalDetailID;

            cmd.Parameters.Add("@AcJournalID", SqlDbType.Int);
            cmd.Parameters["@AcJournalID"].Value = ObjectAcJournalDetail.AcJournalID;

            cmd.Parameters.Add("@AcHeadID", SqlDbType.Int);
            cmd.Parameters["@AcHeadID"].Value = ObjectAcJournalDetail.AcHeadID;

            cmd.Parameters.Add("@AnalysisHeadID", SqlDbType.Int);
            cmd.Parameters["@AnalysisHeadID"].Value = ObjectAcJournalDetail.AnalysisHeadID;

            cmd.Parameters.Add("@Amount", SqlDbType.Money);
            cmd.Parameters["@Amount"].Value = ObjectAcJournalDetail.Amount;

            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar);
            cmd.Parameters["@Remarks"].Value = ObjectAcJournalDetail.Remarks;

            cmd.Parameters.Add("@BranchID", SqlDbType.Int);
            cmd.Parameters["@BranchID"].Value = ObjectAcJournalDetail.BranchID;

            try
            {
                cmd.Connection.Open();
                iReturn = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {

            }

            return iReturn;
        }

        public static int DeleteAcJournalDetail(int AcJournalDetailID)
        {
            int iReturn = 0;
            SqlCommand cmd2 = new SqlCommand();
            cmd2.Connection = new SqlConnection(Common.GetConnectionString);
            cmd2.CommandText = "DELETE FROM AcAnalysisHeadAllocation WHERE AcjournalDetailID = @AcjournalDetailID";
            cmd2.CommandType = CommandType.Text;
            cmd2.Parameters.Add("@AcjournalDetailID", SqlDbType.Int);
            cmd2.Parameters["@AcjournalDetailID"].Value = AcJournalDetailID;
            try
            {
                cmd2.Connection.Open();
                iReturn = cmd2.ExecuteNonQuery();
                cmd2.Connection.Close();
            }
            catch (Exception ex)
            {

            }


            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "DELETE FROM AcJournalDetail WHERE AcJournalDetailID = @AcJournalDetailID";
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add("@AcJournalDetailID", SqlDbType.Int);
            cmd.Parameters["@AcJournalDetailID"].Value = AcJournalDetailID;

            try
            {
                cmd.Connection.Open();
                iReturn = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {

            }
            
            return iReturn;
        }

        public static int InsertAcAnalysisHeadAllocation(AcAnalysisHeadAllocation ObjectAcAnalysisHeadAllocation)
        {
            int iReturn = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "INSERT INTO AcAnalysisHeadAllocation(AcjournalDetailID,AnalysisHeadID,Amount) VALUES(@AcjournalDetailID,@AnalysisHeadID,@Amount)";
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add("@AcjournalDetailID", SqlDbType.Int);
            cmd.Parameters["@AcjournalDetailID"].Value = ObjectAcAnalysisHeadAllocation.AcjournalDetailID;

            cmd.Parameters.Add("@AnalysisHeadID", SqlDbType.Int);
            cmd.Parameters["@AnalysisHeadID"].Value = ObjectAcAnalysisHeadAllocation.AnalysisHeadID;

            cmd.Parameters.Add("@Amount", SqlDbType.Money);
            cmd.Parameters["@Amount"].Value = ObjectAcAnalysisHeadAllocation.Amount;

            try
            {
                cmd.Connection.Open();
                iReturn = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {

            }
            return iReturn;
        }

        public static int UpdateAcAnalysisHeadAllocation(AcAnalysisHeadAllocation ObjectAcAnalysisHeadAllocation)
        {
            int iReturn = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "UPDATE AcAnalysisHeadAllocation SET AcjournalDetailID=@AcjournalDetailID,AnalysisHeadID=@AnalysisHeadID,Amount=@Amount WHERE AcAnalysisHeadAllocationID=@AcAnalysisHeadAllocationID";
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add("@AcAnalysisHeadAllocationID", SqlDbType.Int);
            cmd.Parameters["@AcAnalysisHeadAllocationID"].Value = ObjectAcAnalysisHeadAllocation.AcAnalysisHeadAllocationID;

            cmd.Parameters.Add("@AcjournalDetailID", SqlDbType.Int);
            cmd.Parameters["@AcjournalDetailID"].Value = ObjectAcAnalysisHeadAllocation.AcjournalDetailID;

            cmd.Parameters.Add("@AnalysisHeadID", SqlDbType.Int);
            cmd.Parameters["@AnalysisHeadID"].Value = ObjectAcAnalysisHeadAllocation.AnalysisHeadID;

            cmd.Parameters.Add("@Amount", SqlDbType.Money);
            cmd.Parameters["@Amount"].Value = ObjectAcAnalysisHeadAllocation.Amount;

            try
            {
                cmd.Connection.Open();
                iReturn = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {

            }
            return iReturn;
        }

        public static int DeleteAcAnalysisHeadAllocation(int AcAnalysisHeadAllocationID)
        {
            int iReturn = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "DELETE FROM AcAnalysisHeadAllocation WHERE AcAnalysisHeadAllocationID=@AcAnalysisHeadAllocationID";
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add("@AcAnalysisHeadAllocationID", SqlDbType.Int);
            cmd.Parameters["@AcAnalysisHeadAllocationID"].Value = AcAnalysisHeadAllocationID;

            try
            {
                cmd.Connection.Open();
                iReturn = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {

            }
            return iReturn;
        }

        public static List<AcJournalDetailsVM> GetAcJournalDetails(int AcJournalID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "SELECT ah.AcHead,aj.Amount, aj.Remarks FROM AcJournalDetail as aj INNER JOIN AcHead as ah on aj.AcHeadID=ah.AcHeadID WHERE aj.AcJournalID = @AcJournalID";
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add("@AcJournalID", SqlDbType.Int);
            cmd.Parameters["@AcJournalID"].Value = AcJournalID;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            List<AcJournalDetailsVM> objList = new List<AcJournalDetailsVM>();
            AcJournalDetailsVM obj;
            if (ds != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    obj = new AcJournalDetailsVM();
                    obj.AcHead = ds.Tables[0].Rows[i]["AcHead"].ToString();
                    obj.Amount = Common.ParseDecimal(ds.Tables[0].Rows[i]["Amount"].ToString());
                    obj.Remarks = ds.Tables[0].Rows[i]["Remarks"].ToString();
                    objList.Add(obj);
                }
            }
            return objList;
        }

        public static int InsertOrUpdateAcBankDetails(AcBankDetail ObjectAcBankDetail,int isupdate)
        {
            int iReturn = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "SP_InsertOrUpdateAcBankDetails";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@AcBankDetailID", SqlDbType.Int);
            cmd.Parameters["@AcBankDetailID"].Value = ObjectAcBankDetail.AcBankDetailID;

            cmd.Parameters.Add("@AcJournalID", SqlDbType.Int);
            cmd.Parameters["@AcJournalID"].Value = ObjectAcBankDetail.AcJournalID;

            cmd.Parameters.Add("@BankName", SqlDbType.NVarChar);
            cmd.Parameters["@BankName"].Value = ObjectAcBankDetail.BankName;

            cmd.Parameters.Add("@ChequeNo", SqlDbType.NVarChar);
            cmd.Parameters["@ChequeNo"].Value = ObjectAcBankDetail.ChequeNo;

            if (ObjectAcBankDetail.ChequeDate != null)
            {
                cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime);
                cmd.Parameters["@ChequeDate"].Value = ObjectAcBankDetail.ChequeDate;
            }
            cmd.Parameters.Add("@PartyName", SqlDbType.NVarChar);
            cmd.Parameters["@PartyName"].Value = ObjectAcBankDetail.PartyName;

            cmd.Parameters.Add("@StatusTrans", SqlDbType.NVarChar);
            cmd.Parameters["@StatusTrans"].Value = ObjectAcBankDetail.StatusTrans;

            cmd.Parameters.Add("@IsUpdate", SqlDbType.Int);
            cmd.Parameters["@IsUpdate"].Value = isupdate;
            if (ObjectAcBankDetail.StatusReconciled != null)
            {
                cmd.Parameters.Add("@StatusReconciled", SqlDbType.Bit);
                cmd.Parameters["@StatusReconciled"].Value = ObjectAcBankDetail.StatusReconciled;
            }
            if (ObjectAcBankDetail.ValueDate != null)
            {
                cmd.Parameters.Add("@ValueDate", SqlDbType.DateTime);
                cmd.Parameters["@ValueDate"].Value = ObjectAcBankDetail.ValueDate;
            }
            try
            {
                cmd.Connection.Open();
                iReturn = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {

            }
            return iReturn;
        }

        public static List<CustomerRcieptChildVM> GetCustomerReciept(int RecPayID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "select r.RecPayID,r.RecPayDate,p.InvoiceID,s.SalesHome,p.Amount,s.SalesHome,p.RecPayDetailID,p.CurrencyID from RecPay as r inner join RecPayDetails as p on p.RecPayID = r.RecPayID left join JobGeneration as t on t.InvoiceNo = p.InvoiceID left join JInvoice as s on p.InvoiceID = s.InvoiceID where r.RecPayID = @RecPayID and p.InvoiceID <> 0 and p.InvoiceID is not null";
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add("@RecPayID", SqlDbType.Int);
            cmd.Parameters["@RecPayID"].Value = RecPayID;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            List<CustomerRcieptChildVM> objList = new List<CustomerRcieptChildVM>();
            CustomerRcieptChildVM obj;
            if (ds != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    obj = new CustomerRcieptChildVM();
                    if(ds.Tables[0].Rows[i]["RecPayDate"] != DBNull.Value)
                    {
                        obj.InvoiceDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["RecPayDate"]);
                    }
                    obj.InvoiceID = Common.ParseInt(ds.Tables[0].Rows[i]["InvoiceID"].ToString());
                    obj.AmountToBeRecieved = Common.ParseDecimal(ds.Tables[0].Rows[i]["SalesHome"].ToString());
                    obj.Amount = Common.ParseDecimal(ds.Tables[0].Rows[i]["Amount"].ToString());
                    obj.Amount = -obj.Amount;
                    obj.Balance = Common.ParseDecimal(ds.Tables[0].Rows[i]["SalesHome"].ToString());
                    obj.RecPayDetailID = Common.ParseInt(ds.Tables[0].Rows[i]["RecPayDetailID"].ToString());
                    obj.CurrencyId = Common.ParseInt(ds.Tables[0].Rows[i]["CurrencyId"].ToString());
                    objList.Add(obj);
                }
            }
            return objList;
        }
     

        public static List<CustomerRcieptChildVM> GetCustomerReceipt(int RecPayID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(Common.GetConnectionString);
            cmd.CommandText = "select distinct r.RecPayID,r.RecPayDate,p.InvoiceID,s.SalesHome,p.Amount,s.SalesHome,p.RecPayDetailID,p.CurrencyID from RecPay as r inner join RecPayDetails as p on p.RecPayID = r.RecPayID left join JobGeneration as t on t.InvoiceNo = p.InvoiceID left join JInvoice as s on p.InvoiceID = s.InvoiceID where r.RecPayID = @RecPayID and p.InvoiceID <> 0 and p.InvoiceID is not null order by r.RecPayDate desc";
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add("@RecPayID", SqlDbType.Int);
            cmd.Parameters["@RecPayID"].Value = RecPayID;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            List<CustomerRcieptChildVM> objList = new List<CustomerRcieptChildVM>();
            CustomerRcieptChildVM obj;
            if (ds != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    obj = new CustomerRcieptChildVM();
                    if (ds.Tables[0].Rows[i]["RecPayDate"] != DBNull.Value)
                    {
                        obj.InvoiceDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["RecPayDate"]);
                    }
                    obj.InvoiceID = Common.ParseInt(ds.Tables[0].Rows[i]["InvoiceID"].ToString());
                    obj.AmountToBeRecieved = Common.ParseDecimal(ds.Tables[0].Rows[i]["SalesHome"].ToString());
                    obj.AmtPaidTillDate = Common.ParseDecimal(ds.Tables[0].Rows[i]["Amount"].ToString());
                    obj.Amount = Common.ParseDecimal(ds.Tables[0].Rows[i]["Amount"].ToString());
                    //   obj.Amount = -obj.Amount;
                    obj.Balance = obj.AmountToBeRecieved - obj.Amount;
                    obj.RecPayDetailID = Common.ParseInt(ds.Tables[0].Rows[i]["RecPayDetailID"].ToString());
                    obj.CurrencyId = Common.ParseInt(ds.Tables[0].Rows[i]["CurrencyId"].ToString());
                    objList.Add(obj);
                }
            }
            return objList;
        }

    }
}