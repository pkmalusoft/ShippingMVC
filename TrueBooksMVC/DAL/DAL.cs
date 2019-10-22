using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using DAL;
using System.Collections;
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




        public static int AddSalesInvoiceDetails(SalesInvoiceDetail SID)
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

            cmd.Parameters.Add("@Rate", SqlDbType.Int);
            cmd.Parameters["@Rate"].Value = SID.Rate;

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

        public static List<SalesInvoice> SP_GetAllSalesInvoiceByDate(Nullable<System.DateTime> fdate, Nullable<System.DateTime> tdate)
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

        public static List<PurchaseInvoice> SP_GetAllPurchaseInvoiceByDate(Nullable<System.DateTime> fdate, Nullable<System.DateTime> tdate)
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
    }
}