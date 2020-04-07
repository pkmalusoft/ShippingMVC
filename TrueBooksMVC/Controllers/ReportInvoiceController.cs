using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueBooksMVC.Models;
using DAL;
using System.IO;

namespace TrueBooksMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class ReportInvoiceController : Controller
    {
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
        //
        // GET: /ReportInvoice/
        ReportModel RM = new ReportModel();

        public ActionResult ReportInvoice(int id)
        {
            InvoiceReport1(id);
            DeliveryReport1(id);
            TruckConsignmentReport1(id);
            ArrivalReport1(id);
            ViewBag.id = id;

            ViewBag.CompanyName = "";
            ViewBag.Address = "";

            return View();
        }

        //Invoice Reports
        public ActionResult InvoiceReportPlain(int JobID)
        {
            try
            {
                var query = RM.GetInvoiceReport(JobID);
              

                ViewData.Model = query.AsEnumerable();

                var data = query.GroupBy(o => new { o.Address1, o.Address2, o.ArrivalDate, o.Customer, o.CustRef, o.Expr1, o.Freight, o.InvoiceNo, o.JobCode, o.Expr2, o.Expr3, o.InvoiceDate, o.MBL, o.Phone, o.Port, o.Vessel, o.VoyageNo, o.Symbol, o.CurrencyName, o.Description }).Select(o => o.FirstOrDefault());

                foreach (var item in data)
                {
                    ViewBag.description = item.Description;
                    ViewBag.ArrvlDate = item.ArrivalDate;
                    ViewBag.Adress1 = item.Address1;
                    ViewBag.Adress2 = item.Address2;
                    ViewBag.InvceNo = item.InvoiceNo;
                    ViewBag.Custmr = item.Customer;
                    ViewBag.Exp1 = item.Expr1;
                    ViewBag.Exp2 = item.Expr2;
                    ViewBag.Exp3 = item.Expr3;
                    ViewBag.JobCd = item.JobCode;
                    ViewBag.InvcDate = item.InvoiceDate;
                    ViewBag.Phne = item.Phone;
                    ViewBag.Frgt = item.Freight;
                    ViewBag.Prt = item.Port;
                    ViewBag.BL = item.MBL;
                    ViewBag.CuRef = item.CustRef;
                    ViewBag.Vesel = item.Vessel;
                    ViewBag.VyageNo = item.VoyageNo;
                    ViewBag.CurrencyName = item.CurrencyName;
                    ViewBag.symbol = item.Symbol;
                    ViewBag.BaseCurrency = Session["BaseCurrency"];

                 

                    AcCompany c = entity.AcCompanies.FirstOrDefault();
                    ViewBag.companyname = c.AcCompany1;
                    ViewBag.caddress1 = c.Address1;
                    ViewBag.caddress2 = c.Address2;
                    ViewBag.caddress3 = c.Address3;
                    ViewBag.cphone = c.Phone;

                }
            }
            catch (Exception)
            {

                throw;
            }
            return View();

        }

        public ActionResult InvoiceReportFormatted(int JobID)
        {
            try
            {
                var query = RM.GetInvoiceReport(JobID);

                //int provcid = (from x in entity.JInvoices where x.InvoiceID == query.FirstOrDefault().JobID select x.ProvisionCurrencyID).FirstOrDefault().Value;

                //ViewBag.CurrencyName = (from c in entity.CurrencyMasters where c.CurrencyID == provcid select c.CurrencyName).FirstOrDefault();

                ViewData.Model = query.AsEnumerable();

                var data = query.GroupBy(o => new { o.Address1, o.Address2, o.ArrivalDate, o.Customer, o.CustRef, o.Expr1, o.Freight, o.InvoiceNo, o.JobCode, o.Expr2, o.Expr3, o.InvoiceDate, o.MBL, o.Phone, o.Port, o.Vessel, o.VoyageNo, o.CurrencyName, o.Symbol, o.Description }).Select(o => o.FirstOrDefault());

                foreach (var item in data)
                {
                    ViewBag.description = item.Description;
                    ViewBag.ArrvlDate = item.ArrivalDate;
                    ViewBag.Adress1 = item.Address1;
                    ViewBag.Adress2 = item.Address2;
                    ViewBag.InvceNo = item.InvoiceNo;
                    ViewBag.Custmr = item.Customer;
                    ViewBag.Exp1 = item.Expr1;
                    ViewBag.Exp2 = item.Expr2;
                    ViewBag.Exp3 = item.Expr3;
                    ViewBag.JobCd = item.JobCode;
                    ViewBag.InvcDate = item.InvoiceDate;
                    ViewBag.Phne = item.Phone;
                    ViewBag.Frgt = item.Freight;
                    ViewBag.Prt = item.Port;
                    ViewBag.BL = item.MBL;
                    ViewBag.CuRef = item.CustRef;
                    ViewBag.Vesel = item.Vessel;
                    ViewBag.VyageNo = item.VoyageNo;
                    ViewBag.CurrencyName = item.CurrencyName;
                    ViewBag.symbol = item.Symbol;
                    ViewBag.BaseCurrency = Session["BaseCurrency"];



                    AcCompany c = entity.AcCompanies.FirstOrDefault();
                    if (c != null)
                    {
                        ViewBag.companyname = c.AcCompany1;
                        ViewBag.caddress1 = c.Address1;
                        ViewBag.caddress2 = c.Address2;
                        ViewBag.caddress3 = c.Address3;
                        ViewBag.cphone = c.Phone;
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
            return View();

        }

        public void InvoiceReport1(int JobID)
        {
            try
            {
                var query = RM.GetInvoiceReport(JobID);

                ViewData.Model = query.AsEnumerable();

                var data = query.GroupBy(o => new { o.Address1, o.Address2, o.ArrivalDate, o.Customer, o.CustRef, o.Expr1, o.Freight, o.InvoiceNo, o.JobCode, o.Expr2, o.Expr3, o.InvoiceDate, o.MBL, o.Phone, o.Port, o.Vessel, o.VoyageNo,o.Symbol,o.CurrencyName }).Select(o => o.FirstOrDefault());

                foreach (var item in data)
                {
                    ViewBag.ArrvlDate = item.ArrivalDate;
                    ViewBag.Adress1 = item.Address1;
                    ViewBag.Adress2 = item.Address2;
                    ViewBag.InvceNo = item.InvoiceNo;
                    ViewBag.Custmr = item.Customer;
                    ViewBag.Exp1 = item.Expr1;
                    ViewBag.Exp2 = item.Expr2;
                    ViewBag.Exp3 = item.Expr3;
                    ViewBag.JobCd = item.JobCode;
                    ViewBag.InvcDate = item.InvoiceDate;
                    ViewBag.Phne = item.Phone;
                    ViewBag.Frgt = item.Freight;
                    ViewBag.Prt = item.Port;
                    ViewBag.BL = item.MBL;
                    ViewBag.CuRef = item.CustRef;
                    ViewBag.Vesel = item.Vessel;
                    ViewBag.VyageNo = item.VoyageNo;
                    ViewBag.CurrencyName = item.CurrencyName;
                    ViewBag.symbol = item.Symbol;


                }
            }
            catch (Exception)
            {

                throw;
            }

        }



        //Delivery Reports
        public ActionResult DeliveryReportPlain(int JobID)
        {
            try
            {
                var query = RM.GetDeliveryNote(JobID);

                foreach (var item in query)
                {
                    ViewBag.DCustomer = item.Customer;
                    ViewBag.DMBL = item.MBL;
                    ViewBag.DBIllOfEntry = item.BIllOfEntry;
                    ViewBag.DCollectionPoint = item.CollectionPoint;
                    ViewBag.DAddress1 = item.Address1;
                    ViewBag.DPhone = item.Phone;
                    ViewBag.DPortRP = item.PortRP;
                    ViewBag.DPort = item.Port;
                    ViewBag.DPortDL = item.PortDL;
                    ViewBag.DDestPort = item.DestPort;
                    ViewBag.DCollectionDate = item.CollectionDate;
                    ViewBag.DArrivalDate = item.ArrivalDate;
                    ViewBag.DDeliveryDate = item.DeliveryDate;
                    ViewBag.DVessel = item.Vessel;
                    ViewBag.DVoyageNo = item.VoyageNo;
                    ViewBag.Dvolume = item.volume;
                    ViewBag.DMark = item.Mark;
                    ViewBag.Dweight = item.weight;
                    ViewBag.DDescription = item.Description;
                    ViewBag.DPackages = item.Packages;
                    ViewBag.DJobCode = item.JobCode;

                    AcCompany c = entity.AcCompanies.FirstOrDefault();
                    ViewBag.companyname = c.AcCompany1;
                    ViewBag.caddress1 = c.Address1;
                    ViewBag.caddress2 = c.Address2;
                    ViewBag.caddress3 = c.Address3;
                    ViewBag.cphone = c.Phone;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }
        public ActionResult DeliveryReportFormatted(int JobID)
        {
            try
            {
                var query = RM.GetDeliveryNote(JobID);

                foreach (var item in query)
                {
                    ViewBag.DCustomer = item.Customer;
                    ViewBag.DMBL = item.MBL;
                    ViewBag.DBIllOfEntry = item.BIllOfEntry;
                    ViewBag.DCollectionPoint = item.CollectionPoint;
                    ViewBag.DAddress1 = item.Address1;
                    ViewBag.DPhone = item.Phone;
                    ViewBag.DPortRP = item.PortRP;
                    ViewBag.DPort = item.Port;
                    ViewBag.DPortDL = item.PortDL;
                    ViewBag.DDestPort = item.DestPort;
                    ViewBag.DCollectionDate = item.CollectionDate;
                    ViewBag.DArrivalDate = item.ArrivalDate;
                    ViewBag.DDeliveryDate = item.DeliveryDate;
                    ViewBag.DVessel = item.Vessel;
                    ViewBag.DVoyageNo = item.VoyageNo;
                    ViewBag.Dvolume = item.volume;
                    ViewBag.DMark = item.Mark;
                    ViewBag.Dweight = item.weight;
                    ViewBag.DDescription = item.Description;
                    ViewBag.DPackages = item.Packages;
                    ViewBag.DJobCode = item.JobCode;

                    AcCompany c = entity.AcCompanies.FirstOrDefault();
                    ViewBag.companyname = c.AcCompany1;
                    ViewBag.caddress1 = c.Address1;
                    ViewBag.caddress2 = c.Address2;
                    ViewBag.caddress3 = c.Address3;
                    ViewBag.cphone = c.Phone;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }
        public void DeliveryReport1(int JobID)
        {
            try
            {
                var query = RM.GetDeliveryNote(JobID);

                foreach (var item in query)
                {
                    ViewBag.DCustomer = item.Customer;
                    ViewBag.DMBL = item.MBL;
                    ViewBag.DBIllOfEntry = item.BIllOfEntry;
                    ViewBag.DCollectionPoint = item.CollectionPoint;
                    ViewBag.DAddress1 = item.Address1;
                    ViewBag.DPhone = item.Phone;
                    ViewBag.DPortRP = item.PortRP;
                    ViewBag.DPort = item.Port;
                    ViewBag.DPortDL = item.PortDL;
                    ViewBag.DDestPort = item.DestPort;
                    ViewBag.DCollectionDate = item.CollectionDate;
                    ViewBag.DArrivalDate = item.ArrivalDate;
                    ViewBag.DDeliveryDate = item.DeliveryDate;
                    ViewBag.DVessel = item.Vessel;
                    ViewBag.DVoyageNo = item.VoyageNo;
                    ViewBag.Dvolume = item.volume;
                    ViewBag.DMark = item.Mark;
                    ViewBag.Dweight = item.weight;
                    ViewBag.DDescription = item.Description;
                    ViewBag.DPackages = item.Packages;
                    ViewBag.DJobCode = item.JobCode;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }






        //Truck Consignment Reports
        public ActionResult TruckConsignmentReportPlain(int JobID)
        {
            try
            {
                var query = RM.GetTruckConsignmentReport(JobID);

                foreach (var item in query)
                {
                    ViewBag.TJobCode = item.JobCode;
                    ViewBag.TTDN = item.TDN;
                    ViewBag.TCUSTOMERCust = item.CUSTOMERCust;
                    ViewBag.TCUSTOMERAddrs1 = item.CUSTOMERAddrs1;
                    ViewBag.TCUSTOMERAddrs2 = item.CUSTOMERAddrs2;
                    ViewBag.TCUSTOMERAddrs3 = item.CUSTOMERAddrs3;
                    ViewBag.TCUSTOMERPhn = item.CUSTOMERPhn;
                    ViewBag.TConsigneeCust = item.ConsigneeCust;
                    ViewBag.TConsigneeAddrs1 = item.ConsigneeAddrs1;
                    ViewBag.TConsigneeAddrs2 = item.ConsigneeAddrs2;
                    ViewBag.TConsigneeAddrs3 = item.ConsigneeAddrs3;
                    ViewBag.TConsigneePhn = item.ConsigneePhn;
                    ViewBag.TNotifyCust = item.NotifyCust;
                    ViewBag.TNotifyAddrs = item.NotifyAddrs;
                    ViewBag.TNotifyAddrs2 = item.NotifyAddrs2;
                    ViewBag.TNotifyAddrs3 = item.NotifyAddrs3;
                    ViewBag.TNotifyPhn = item.NotifyPhn;
                    ViewBag.TDeliveryOrderDate = item.DeliveryOrderDate;
                    ViewBag.TDescription = item.Description;
                    ViewBag.TMark = item.Mark;
                    ViewBag.Tvolume = item.volume;
                    ViewBag.Tweight = item.weight;
                    ViewBag.TPort = item.Port;
                    ViewBag.TDestiPort = item.DestiPort;
                    ViewBag.TJobDate = item.JobDate;
                    ViewBag.TTruckRegNo = item.TruckRegNo;
                    ViewBag.TDriverDetails = item.DriverDetails;
                    ViewBag.TCLFValue = item.CLFValue;
                    ViewBag.TDeliveryInstructions = item.DeliveryInstructions;


                    AcCompany c = entity.AcCompanies.FirstOrDefault();
                    ViewBag.companyname = c.AcCompany1;
                    ViewBag.caddress1 = c.Address1;
                    ViewBag.caddress2 = c.Address2;
                    ViewBag.caddress3 = c.Address3;
                    ViewBag.cphone = c.Phone;

                }
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }

        public ActionResult TruckConsignmentReportFormatted(int JobID)
        {
            try
            {
                var query = RM.GetTruckConsignmentReport(JobID);

                foreach (var item in query)
                {
                    ViewBag.TJobCode = item.JobCode;
                    ViewBag.TTDN = item.TDN;
                    ViewBag.TCUSTOMERCust = item.CUSTOMERCust;
                    ViewBag.TCUSTOMERAddrs1 = item.CUSTOMERAddrs1;
                    ViewBag.TCUSTOMERAddrs2 = item.CUSTOMERAddrs2;
                    ViewBag.TCUSTOMERAddrs3 = item.CUSTOMERAddrs3;
                    ViewBag.TCUSTOMERPhn = item.CUSTOMERPhn;
                    ViewBag.TConsigneeCust = item.ConsigneeCust;
                    ViewBag.TConsigneeAddrs1 = item.ConsigneeAddrs1;
                    ViewBag.TConsigneeAddrs2 = item.ConsigneeAddrs2;
                    ViewBag.TConsigneeAddrs3 = item.ConsigneeAddrs3;
                    ViewBag.TConsigneePhn = item.ConsigneePhn;
                    ViewBag.TNotifyCust = item.NotifyCust;
                    ViewBag.TNotifyAddrs = item.NotifyAddrs;
                    ViewBag.TNotifyAddrs2 = item.NotifyAddrs2;
                    ViewBag.TNotifyAddrs3 = item.NotifyAddrs3;
                    ViewBag.TNotifyPhn = item.NotifyPhn;
                    ViewBag.TDeliveryOrderDate = item.DeliveryOrderDate;
                    ViewBag.TDescription = item.Description;
                    ViewBag.TMark = item.Mark;
                    ViewBag.Tvolume = item.volume;
                    ViewBag.Tweight = item.weight;
                    ViewBag.TPort = item.Port;
                    ViewBag.TDestiPort = item.DestiPort;
                    ViewBag.TJobDate = item.JobDate;
                    ViewBag.TTruckRegNo = item.TruckRegNo;
                    ViewBag.TDriverDetails = item.DriverDetails;
                    ViewBag.TCLFValue = item.CLFValue;
                    ViewBag.TDeliveryInstructions = item.DeliveryInstructions;


                    AcCompany c = entity.AcCompanies.FirstOrDefault();
                    ViewBag.companyname = c.AcCompany1;
                    ViewBag.caddress1 = c.Address1;
                    ViewBag.caddress2 = c.Address2;
                    ViewBag.caddress3 = c.Address3;
                    ViewBag.cphone = c.Phone;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }

        public void TruckConsignmentReport1(int JobID)
        {
            try
            {
                var query = RM.GetTruckConsignmentReport(JobID);

                foreach (var item in query)
                {
                    ViewBag.TJobCode = item.JobCode;
                    ViewBag.TTDN = item.TDN;
                    ViewBag.TCUSTOMERCust = item.CUSTOMERCust;
                    ViewBag.TCUSTOMERAddrs1 = item.CUSTOMERAddrs1;
                    ViewBag.TCUSTOMERAddrs2 = item.CUSTOMERAddrs2;
                    ViewBag.TCUSTOMERAddrs3 = item.CUSTOMERAddrs3;
                    ViewBag.TCUSTOMERPhn = item.CUSTOMERPhn;
                    ViewBag.TConsigneeCust = item.ConsigneeCust;
                    ViewBag.TConsigneeAddrs1 = item.ConsigneeAddrs1;
                    ViewBag.TConsigneeAddrs2 = item.ConsigneeAddrs2;
                    ViewBag.TConsigneeAddrs3 = item.ConsigneeAddrs3;
                    ViewBag.TConsigneePhn = item.ConsigneePhn;
                    ViewBag.TNotifyCust = item.NotifyCust;
                    ViewBag.TNotifyAddrs = item.NotifyAddrs;
                    ViewBag.TNotifyAddrs2 = item.NotifyAddrs2;
                    ViewBag.TNotifyAddrs3 = item.NotifyAddrs3;
                    ViewBag.TNotifyPhn = item.NotifyPhn;
                    ViewBag.TDeliveryOrderDate = item.DeliveryOrderDate;
                    ViewBag.TDescription = item.Description;
                    ViewBag.TMark = item.Mark;
                    ViewBag.Tvolume = item.volume;
                    ViewBag.Tweight = item.weight;
                    ViewBag.TPort = item.Port;
                    ViewBag.TDestiPort = item.DestiPort;
                    ViewBag.TJobDate = item.JobDate;
                    ViewBag.TTruckRegNo = item.TruckRegNo;
                    ViewBag.TDriverDetails = item.DriverDetails;
                    ViewBag.TCLFValue = item.CLFValue;
                    ViewBag.TDeliveryInstructions = item.DeliveryInstructions;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }



        //Arrival Reports
        public ActionResult ArrivalReportPlain(int JobID)
        {
            try
            {
                var item = RM.GetArrivalReport(JobID).FirstOrDefault();

                // foreach (var item in query)
                // {
                if (item != null)
                {
                    ViewBag.AConsigneeCust = item.ConsigneeCust;
                    ViewBag.AcongPhn = item.congPhn;
                    ViewBag.AAddress1 = item.Address1;
                    ViewBag.AAddress2 = item.Address2;
                    ViewBag.AAmtInWords = item.AmtInWords;
                    ViewBag.AArrivalDate = item.ArrivalDate;
                    ViewBag.AContainerNo = item.ContainerNo;
                    ViewBag.ACurrencyName = item.CurrencyName;
                    ViewBag.ACustomerCust = item.CustomerCust;
                    ViewBag.ACustPhn = item.CustPhn;
                    ViewBag.AFlight = item.Flight;
                    ViewBag.AJobDescription = item.JobDescription;
                    ViewBag.AMAWB = item.MAWB;
                    ViewBag.AMBL = item.MBL;
                    ViewBag.AnotifyCust = item.notifyCust;
                    ViewBag.APackages = item.Packages;
                    ViewBag.APort = item.Port;
                    ViewBag.ARefNo = item.RefNo;
                    ViewBag.ASealNo = item.SealNo;
                    ViewBag.AStatusSea = item.StatusSea;
                    ViewBag.AVessel = item.Vessel;
                    ViewBag.AVoyageNo = item.VoyageNo;
                    ViewBag.Avolume = item.volume;
                    ViewBag.Aweight = item.weight;


                    AcCompany c = entity.AcCompanies.FirstOrDefault();
                    ViewBag.companyname = c.AcCompany1;
                    ViewBag.caddress1 = c.Address1;
                    ViewBag.caddress2 = c.Address2;
                    ViewBag.caddress3 = c.Address3;
                    ViewBag.cphone = c.Phone;

                }
                // }
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }
        public ActionResult ArrivalReportFormatted(int JobID)
        {
            try
            {
                var item = RM.GetArrivalReport(JobID).FirstOrDefault();

                // foreach (var item in query)
                // {
                if (item != null)
                {
                    ViewBag.AConsigneeCust = item.ConsigneeCust;
                    ViewBag.AcongPhn = item.congPhn;
                    ViewBag.AAddress1 = item.Address1;
                    ViewBag.AAddress2 = item.Address2;
                    ViewBag.AAmtInWords = item.AmtInWords;
                    ViewBag.AArrivalDate = item.ArrivalDate;
                    ViewBag.AContainerNo = item.ContainerNo;
                    ViewBag.ACurrencyName = item.CurrencyName;
                    ViewBag.ACustomerCust = item.CustomerCust;
                    ViewBag.ACustPhn = item.CustPhn;
                    ViewBag.AFlight = item.Flight;
                    ViewBag.AJobDescription = item.JobDescription;
                    ViewBag.AMAWB = item.MAWB;
                    ViewBag.AMBL = item.MBL;
                    ViewBag.AnotifyCust = item.notifyCust;
                    ViewBag.APackages = item.Packages;
                    ViewBag.APort = item.Port;
                    ViewBag.ARefNo = item.RefNo;
                    ViewBag.ASealNo = item.SealNo;
                    ViewBag.AStatusSea = item.StatusSea;
                    ViewBag.AVessel = item.Vessel;
                    ViewBag.AVoyageNo = item.VoyageNo;
                    ViewBag.Avolume = item.volume;
                    ViewBag.Aweight = item.weight;


                    AcCompany c = entity.AcCompanies.FirstOrDefault();
                    ViewBag.companyname = c.AcCompany1;
                    ViewBag.caddress1 = c.Address1;
                    ViewBag.caddress2 = c.Address2;
                    ViewBag.caddress3 = c.Address3;
                    ViewBag.cphone = c.Phone;


                }
                // }
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }
        public void ArrivalReport1(int JobID)
        {
            try
            {
                var item = RM.GetArrivalReport(JobID).FirstOrDefault();

                // foreach (var item in query)
                // {
                if (item != null)
                {
                    ViewBag.AConsigneeCust = item.ConsigneeCust;
                    ViewBag.AcongPhn = item.congPhn;
                    ViewBag.AAddress1 = item.Address1;
                    ViewBag.AAddress2 = item.Address2;
                    ViewBag.AAmtInWords = item.AmtInWords;
                    ViewBag.AArrivalDate = item.ArrivalDate;
                    ViewBag.AContainerNo = item.ContainerNo;
                    ViewBag.ACurrencyName = item.CurrencyName;
                    ViewBag.ACustomerCust = item.CustomerCust;
                    ViewBag.ACustPhn = item.CustPhn;
                    ViewBag.AFlight = item.Flight;
                    ViewBag.AJobDescription = item.JobDescription;
                    ViewBag.AMAWB = item.MAWB;
                    ViewBag.AMBL = item.MBL;
                    ViewBag.AnotifyCust = item.notifyCust;
                    ViewBag.APackages = item.Packages;
                    ViewBag.APort = item.Port;
                    ViewBag.ARefNo = item.RefNo;
                    ViewBag.ASealNo = item.SealNo;
                    ViewBag.AStatusSea = item.StatusSea;
                    ViewBag.AVessel = item.Vessel;
                    ViewBag.AVoyageNo = item.VoyageNo;
                    ViewBag.Avolume = item.volume;
                    ViewBag.Aweight = item.weight;
                }
                // }
            }
            catch (Exception)
            {

                throw;
            }
        }
        //public ActionResult CustomerInvoiceReportPlain(int Invoiceno)
        //{
        //    try
        //    {
        //        var query = entity.SP_GetCustomerInvoiceReport(Invoiceno).ToList();

        //        ViewData.Model = query.FirstOrDefault();

        //        var data = query.GroupBy(o => new { o.Address1, o.Address2, o.ArrivalDate, o.Customer, o.CustRef, o.Expr1, o.Freight, o.InvoiceNo, o.JobCode, o.Expr2, o.Expr3, o.InvoiceDate, o.MBL, o.Phone, o.Port, o.Vessel, o.VoyageNo,o.CurrencyName,o.Symbol,o.description}).Select(o => o.FirstOrDefault());

        //        foreach (var item in query)
        //        {
        //            ViewBag.description = item.description;
        //            ViewBag.ArrvlDate = item.ArrivalDate;
        //            ViewBag.Adress1 = item.Address1;
        //            ViewBag.Adress2 = item.Address2;
        //            ViewBag.InvceNo = item.InvoiceNo;
        //            ViewBag.Custmr = item.Customer;
        //            ViewBag.Exp1 = item.Expr1;
        //            ViewBag.Exp2 = item.Expr2;
        //            ViewBag.Exp3 = item.Expr3;
        //            ViewBag.JobCd = item.JobCode;
        //            ViewBag.InvcDate = item.InvoiceDate;
        //            ViewBag.Phne = item.Phone;
        //            ViewBag.Frgt = item.Freight;
        //            ViewBag.Prt = item.Port;
        //            ViewBag.BL = item.MBL;
        //            ViewBag.CuRef = item.CustRef;
        //            ViewBag.Vesel = item.Vessel;
        //            ViewBag.VyageNo = item.VoyageNo;
        //            ViewBag.CurrencyName = item.CurrencyName;
        //            ViewBag.symbol = item.Symbol;
        //            ViewBag.BaseCurrency = Session["BaseCurrency"];

        //            AcCompany c = entity.AcCompanies.FirstOrDefault();
        //            ViewBag.companyname = c.AcCompany1;
        //            ViewBag.caddress1 = c.Address1;
        //            ViewBag.caddress2 = c.Address2;
        //            ViewBag.caddress3 = c.Address3;
        //            ViewBag.cphone = c.Phone;

        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    return View();


        //}

        //public ActionResult CustomerInvoiceReportFormatted(int Invoiceno)
        //{
        //    try
        //    {
        //        var query = entity.SP_GetCustomerInvoiceReport(Invoiceno).ToList();

        //        ViewData.Model = query.FirstOrDefault();

        //        var data = query.GroupBy(o => new { o.Address1, o.Address2, o.ArrivalDate, o.Customer, o.CustRef, o.Expr1, o.Freight, o.InvoiceNo, o.JobCode, o.Expr2, o.Expr3, o.InvoiceDate, o.MBL, o.Phone, o.Port, o.Vessel, o.VoyageNo ,o.CurrencyName,o.Symbol,o.description}).Select(o => o.FirstOrDefault());

        //        foreach (var item in query)
        //        {
        //            ViewBag.description = item.description;
        //            ViewBag.ArrvlDate = item.ArrivalDate;
        //            ViewBag.Adress1 = item.Address1;
        //            ViewBag.Adress2 = item.Address2;
        //            ViewBag.InvceNo = item.InvoiceNo;
        //            ViewBag.Custmr = item.Customer;
        //            ViewBag.Exp1 = item.Expr1;
        //            ViewBag.Exp2 = item.Expr2;
        //            ViewBag.Exp3 = item.Expr3;
        //            ViewBag.JobCd = item.JobCode;
        //            ViewBag.InvcDate = item.InvoiceDate;
        //            ViewBag.Phne = item.Phone;
        //            ViewBag.Frgt = item.Freight;
        //            ViewBag.Prt = item.Port;
        //            ViewBag.BL = item.MBL;
        //            ViewBag.CuRef = item.CustRef;
        //            ViewBag.Vesel = item.Vessel;
        //            ViewBag.VyageNo = item.VoyageNo;
        //            ViewBag.CurrencyName = item.;
        //            ViewBag.symbol = item.Symbol;
        //            ViewBag.BaseCurrency = Session["BaseCurrency"];



        //            AcCompany c = entity.AcCompanies.FirstOrDefault();
        //            ViewBag.companyname = c.AcCompany1;
        //            ViewBag.caddress1 = c.Address1;
        //            ViewBag.caddress2 = c.Address2;
        //            ViewBag.caddress3 = c.Address3;
        //            ViewBag.cphone = c.Phone;

        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    return View();


        //}
        public ActionResult JobRegister()
        {


            ViewBag.customers = new SelectList(entity.CUSTOMERs.OrderBy(x => x.Customer1).ToList(), "CustomerID", "Customer1");
            ViewBag.jobs = new SelectList(entity.JobGenerations.OrderBy(a=>a.JobCode).ToList(), "JobID", "JobCode");
            ViewBag.Currency = new SelectList(entity.CurrencyMasters.OrderBy(x => x.CurrencyName).ToList(), "CurrencyID", "CurrencyName");
            var invoice = new SelectList(new[] 
                                        {
                                            new { ID = "1", trans = "Invoiced" },
                                            new { ID = "2", trans = "Not Invoiced" },
                                           
                                        },
                                     "ID", "trans", 1);
            ViewBag.inv = invoice;
            //var query = (from t in entity.JobGenerations
            //             select new JobRegisterVM
            //             {
            //                 JobCode = t.JobCode,
            //                 JobTypeID = t.JobTypeID.Value

            //             }).ToList();

            //ViewBag.jobRegister = query;

            //return View(query);

            return View();
        }


        public ActionResult GetAllJobs(string custid, string jobid, string currencyid,string statusinv, string fdate, string todate)
        {
            ViewBag.customers = new SelectList(entity.CUSTOMERs, "CustomerID", "Customer1");
            ViewBag.jobs = new SelectList(entity.JobGenerations, "JobID", "JobCode");
            //DateTime datefrom = Convert.ToDateTime(frm);
            //ViewBag.customers = entity.CUSTOMERs.ToList();
            //ViewBag.jobs = entity.JobGenerations.ToList();

            DateTime frmdate = Convert.ToDateTime(fdate);
            DateTime tdate = Convert.ToDateTime(todate);

            string view = "";

            decimal exrate = 0;
            int currid;

            int vcustid;
            int vjobid;
            int vstatus;
            int jobId;

            if (currencyid == "")
            {
                currid = 0;
            }
            else
            {
                currid = Convert.ToInt32(currencyid);
                exrate = (from x in entity.CurrencyMasters where x.CurrencyID == currid select x.ExchangeRate).FirstOrDefault().Value;
            }

            if (custid == "")
            {
                vcustid = 0;
            }
            else
            {
                vcustid = Convert.ToInt32(custid);
            }

            if (statusinv == "")
            {
                vstatus = 0;
            }
            else
            {
                vstatus = Convert.ToInt32(statusinv);
            }

            if (jobid == "")
            {
                vjobid = 0;
            }
            else
            {
                vjobid = Convert.ToInt32(jobid);
            }


            if (jobid == "")
            {
                jobId = 0;
            }
            else
            {
                jobId = Convert.ToInt32(jobid);
            }





            decimal provisionCurrency = (from t in entity.JInvoices where t.JobID == jobId select t.ProvisionExchangeRate.Value).FirstOrDefault();
            decimal SalesCurrency = (from t in entity.JInvoices where t.JobID == jobId select t.SalesExchangeRate.Value).FirstOrDefault();
            var vJobRegisterVMlist = new List<JobRegisterVM>();
            var tempJobRegisterVMlist = new List<JobRegisterVM>();


            //if (currid == 0)
            //{
            //todo:fix to run by sethu
            // var data = entity.JobRegisterReport(Convert.ToDateTime(fdate), Convert.ToDateTime(todate), vcustid, vjobid, vstatus).ToList();
            var data = entity.JobRegisterReport(Convert.ToDateTime(fdate), Convert.ToDateTime(todate), vcustid, vjobid).ToList();
            //    view = this.RenderPartialView("ucJobRegister", data);



            //    return PartialView("ucJobRegister", data);
            //}

            return PartialView("ucJobRegister", data);

            //return new LargeJsonResult
            //{
            //    MaxJsonLength = Int32.MaxValue,
            //    Data = new
            //    {
            //        success = true,
            //        view = view
            //    },
            //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //};

            //if (vcustid == 0 && vjobid == 0)
            //{
            //    if (currid == 0)
            //    {
            //        vJobRegisterVMlist = (from j in entity.JobGenerations
            //                              join k in entity.CUSTOMERs on j.ShipperID equals k.CustomerID
            //                              join l in entity.JInvoices on j.JobID equals l.JobID
            //                              where
            //                             (
            //                           //(frmdate == DateTime.MinValue) ? true : j.JobDate >= frmdate
            //                           //&&
            //                           //(tdate == DateTime.MinValue) ? true : j.JobDate <= tdate
            //                           j.JobDate>=frmdate && j.JobDate<=tdate
            //                             )
            //                              select new JobRegisterVM
            //                              {
            //                                  JobID=j.JobID,
            //                                  JobCode = j.JobCode,
            //                                  JobDate = j.JobDate.Value,
            //                                  InvoiceNo = j.InvoiceNo,
            //                                  Customer = k.Customer1,
            //                                  Provision = l.ProvisionHome,
            //                                  Sales = l.SalesHome,
            //                                  Profit = (l.SalesHome-l.ProvisionHome)

            //                              }).ToList();

            //        foreach (var item in vJobRegisterVMlist)
            //        {
            //            JobRegisterVM a = new JobRegisterVM();
            //            a.JobCode = item.JobCode;
            //            a.JobID = item.JobID;
            //            a.JobDate = item.JobDate;
            //            a.InvoiceNo = item.InvoiceNo;
            //            a.Customer = item.Customer;
            //            a.Provision = item.Provision;
            //            a.Sales = item.Sales;
            //            a.Profit = (a.Sales - a.Provision);
            //            tempJobRegisterVMlist.Add(a);
            //        }



            //        view = this.RenderPartialView("ucJobRegister", tempJobRegisterVMlist);
            //    }
            //    else if(currid!=0)
            //    {
            //        vJobRegisterVMlist = (from j in entity.JobGenerations
            //                              join k in entity.CUSTOMERs on j.ShipperID equals k.CustomerID
            //                              join l in entity.JInvoices on j.JobID equals l.JobID
            //                              where
            //                        (
            //                      (frmdate == DateTime.MinValue) ? true : j.JobDate >= frmdate
            //                      &&
            //                      (tdate == DateTime.MinValue) ? true : j.JobDate <= tdate
            //                        )
            //                              select new JobRegisterVM
            //                              {
            //                                  JobCode = j.JobCode,
            //                                   JobID=j.JobID,
            //                                  JobDate = j.JobDate.Value,
            //                                  InvoiceNo = j.InvoiceNo,
            //                                  Customer = k.Customer1,
            //                                  ProvExRate = l.ProvisionExchangeRate,
            //                                  SalesExRate = l.SalesExchangeRate,
            //                                  Provision = l.ProvisionHome,
            //                                  Sales = l.SalesHome,
            //                                  Profit = (l.SalesHome - l.ProvisionHome)

            //                              }).ToList();

            //        foreach (var item in vJobRegisterVMlist)
            //        {
            //            JobRegisterVM a = new JobRegisterVM();
            //            a.JobCode = item.JobCode;
            //            a.JobID = item.JobID;
            //            a.JobDate = item.JobDate;
            //            a.InvoiceNo = item.InvoiceNo;
            //            a.Customer = item.Customer;
            //            a.Provision = (item.Provision / item.ProvExRate) * exrate;
            //            a.Sales = (item.Sales / item.SalesExRate) * exrate;
            //            a.Profit = (a.Sales - a.Provision);
            //            tempJobRegisterVMlist.Add(a);
            //        }

            //        view = this.RenderPartialView("ucJobRegister", tempJobRegisterVMlist);
            //    }


            //}
            //else if (vcustid !=0 && vjobid == 0)
            //{
            //    if (currid == 0)
            //    {
            //        vJobRegisterVMlist = (from j in entity.JobGenerations
            //                              join k in entity.CUSTOMERs on j.ShipperID equals k.CustomerID
            //                              join l in entity.JInvoices on j.JobID equals l.JobID
            //                   //           where j.JobDate != null && j.ShipperID.HasValue
            //                   //              && j.ShipperID.HasValue ? j.ShipperID.Value == (vcustid) : false
            //                   //      &&
            //                   //  (
            //                   //(frmdate == DateTime.MinValue) ? true : j.JobDate >= frmdate
            //                   //&&
            //                   //(tdate == DateTime.MinValue) ? true : j.JobDate <= tdate
            //                   //  )
            //                   where (j.JobDate>=frmdate && j.JobDate<=tdate) && j.ShipperID==vcustid
            //                              select new JobRegisterVM
            //                              {
            //                                  JobCode = j.JobCode,
            //                                  JobID=j.JobID,
            //                                  JobDate = j.JobDate.Value,
            //                                  InvoiceNo = j.InvoiceNo,
            //                                  Customer = k.Customer1,
            //                                  ProvExRate = l.ProvisionExchangeRate,
            //                                  SalesExRate = l.SalesExchangeRate,
            //                                  Provision = l.ProvisionHome,
            //                                  Sales = l.SalesHome,
            //                                  Profit = (l.SalesHome - l.ProvisionHome)

            //                              }).ToList();

            //        foreach (var item in vJobRegisterVMlist)
            //        {
            //            JobRegisterVM a = new JobRegisterVM();
            //            a.JobCode = item.JobCode;
            //            a.JobDate = item.JobDate;
            //            a.JobID = item.JobID;
            //            a.InvoiceNo = item.InvoiceNo;
            //            a.Customer = item.Customer;
            //            a.Provision = item.Provision;
            //            a.Sales = item.Sales;
            //            a.Profit = (a.Sales - a.Provision);
            //            tempJobRegisterVMlist.Add(a);
            //        }

            //        view = this.RenderPartialView("ucJobRegister", tempJobRegisterVMlist);
            //    } 
            //    else if (currid != 0)
            //    {
            //        vJobRegisterVMlist = (from j in entity.JobGenerations
            //                              join k in entity.CUSTOMERs on j.ShipperID equals k.CustomerID
            //                              join l in entity.JInvoices on j.JobID equals l.JobID
            //                              where j.JobDate != null && j.ShipperID.HasValue
            //                                 && j.ShipperID.HasValue ? j.ShipperID.Value == (vcustid) : false
            //                                     &&
            //                     (
            //                   (frmdate == DateTime.MinValue) ? true : j.JobDate >= frmdate
            //                   &&
            //                   (tdate == DateTime.MinValue) ? true : j.JobDate <= tdate
            //                     )
            //                              select new JobRegisterVM
            //                              {
            //                                  JobCode = j.JobCode,
            //                                  JobID=j.JobID,
            //                                  JobDate = j.JobDate.Value,
            //                                  InvoiceNo = j.InvoiceNo,
            //                                  Customer = k.Customer1,
            //                                  ProvExRate = l.ProvisionExchangeRate,
            //                                  SalesExRate = l.SalesExchangeRate,
            //                                  Provision = l.ProvisionHome,
            //                                  Sales = l.SalesHome,
            //                                  Profit = (l.SalesHome - l.ProvisionHome)


            //                              }).ToList();

            //        foreach (var item in vJobRegisterVMlist)
            //        {
            //            JobRegisterVM a = new JobRegisterVM();
            //            a.JobCode = item.JobCode;
            //            a.JobID = item.JobID;
            //            a.JobDate = item.JobDate;
            //            a.InvoiceNo = item.InvoiceNo;
            //            a.Customer = item.Customer;
            //            a.Provision = (item.Provision / item.ProvExRate) * exrate;
            //            a.Sales = (item.Sales / item.SalesExRate) * exrate;
            //            a.Profit = (a.Sales - a.Provision);
            //            tempJobRegisterVMlist.Add(a);
            //        }

            //        view = this.RenderPartialView("ucJobRegister", tempJobRegisterVMlist);
            //    }

            //}

            //else if (vcustid == 0 && vjobid != 0)
            //{
            //    if (currid == 0)
            //    {
            //        vJobRegisterVMlist = (from j in entity.JobGenerations
            //                              join k in entity.CUSTOMERs on j.ShipperID equals k.CustomerID
            //                              join l in entity.JInvoices on j.JobID equals l.JobID
            //                              where j.JobDate != null && j.ShipperID.HasValue
            //                                    && vjobid > 0 ? j.JobID == (vjobid) : false
            //                                        &&
            //                     (
            //                   (frmdate == DateTime.MinValue) ? true : j.JobDate >= frmdate
            //                   &&
            //                   (tdate == DateTime.MinValue) ? true : j.JobDate <= tdate
            //                     )
            //                              select new JobRegisterVM
            //                              {
            //                                  JobCode = j.JobCode,
            //                                  JobDate = j.JobDate.Value,
            //                                  JobID=j.JobID,
            //                                  InvoiceNo = j.InvoiceNo,
            //                                  Customer = k.Customer1,
            //                                  ProvExRate = l.ProvisionExchangeRate,
            //                                  SalesExRate = l.SalesExchangeRate,
            //                                  Provision = l.ProvisionHome,
            //                                  Sales = l.SalesHome,
            //                                  Profit = (l.SalesHome - l.ProvisionHome)

            //                              }).ToList();

            //        foreach (var item in vJobRegisterVMlist)
            //        {
            //            JobRegisterVM a = new JobRegisterVM();
            //            a.JobCode = item.JobCode;
            //            a.JobDate = item.JobDate;
            //            a.InvoiceNo = item.InvoiceNo;
            //            a.JobID = item.JobID;
            //            a.Customer = item.Customer;
            //            a.Provision = item.Provision;
            //            a.Sales = item.Sales;
            //            a.Profit = (a.Sales - a.Provision);
            //            tempJobRegisterVMlist.Add(a);
            //        }

            //        view = this.RenderPartialView("ucJobRegister", tempJobRegisterVMlist);
            //    }
            //    else if (currid != 0)
            //    {
            //        vJobRegisterVMlist = (from j in entity.JobGenerations
            //                              join k in entity.CUSTOMERs on j.ShipperID equals k.CustomerID
            //                              join l in entity.JInvoices on j.JobID equals l.JobID
            //                              where j.JobDate != null && j.ShipperID.HasValue
            //                                   && vjobid > 0 ? j.JobID == (vjobid) : false
            //                                       &&
            //                     (
            //                   (frmdate == DateTime.MinValue) ? true : j.JobDate >= frmdate
            //                   &&
            //                   (tdate == DateTime.MinValue) ? true : j.JobDate <= tdate
            //                     )
            //                              select new JobRegisterVM
            //                              {
            //                                  JobCode = j.JobCode,
            //                                  JobDate = j.JobDate.Value,
            //                                  JobID=j.JobID,
            //                                  InvoiceNo = j.InvoiceNo,
            //                                  Customer = k.Customer1,
            //                                  ProvExRate = l.ProvisionExchangeRate,
            //                                  SalesExRate = l.SalesExchangeRate,
            //                                  Provision = l.ProvisionHome,
            //                                  Sales = l.SalesHome,
            //                                  Profit = (l.SalesHome - l.ProvisionHome)


            //                              }).ToList();

            //        foreach (var item in vJobRegisterVMlist)
            //        {
            //            JobRegisterVM a = new JobRegisterVM();
            //            a.JobCode = item.JobCode;
            //            a.JobID = item.JobID; 
            //            a.JobDate = item.JobDate;
            //            a.InvoiceNo = item.InvoiceNo;
            //            a.Customer = item.Customer;
            //            a.Provision = (item.Provision / item.ProvExRate) * exrate;
            //            a.Sales = (item.Sales / item.SalesExRate) * exrate;
            //            a.Profit = (a.Sales - a.Provision);
            //            tempJobRegisterVMlist.Add(a);
            //        }

            //        view = this.RenderPartialView("ucJobRegister", tempJobRegisterVMlist);

            //    }
            //}
            //else if (vcustid != 0 && vjobid != 0)
            //{
            //    if (currid == 0)
            //    {
            //        vJobRegisterVMlist = (from j in entity.JobGenerations
            //                              join k in entity.CUSTOMERs on j.ShipperID equals k.CustomerID
            //                              join l in entity.JInvoices on j.JobID equals l.JobID
            //                              where j.JobDate != null && j.ShipperID.HasValue
            //                               && vjobid > 0 ? j.JobID == (vjobid) : false
            //                                  && j.ShipperID.HasValue ? j.ShipperID.Value == (vcustid) : false
            //                                      &&
            //                     (
            //                   (frmdate == DateTime.MinValue) ? true : j.JobDate >= frmdate
            //                   &&
            //                   (tdate == DateTime.MinValue) ? true : j.JobDate <= tdate
            //                     )
            //                              select new JobRegisterVM
            //                              {
            //                                  JobCode = j.JobCode,
            //                                  JobID=j.JobID,
            //                                  JobDate = j.JobDate.Value,
            //                                  InvoiceNo = j.InvoiceNo,
            //                                  Customer = k.Customer1,
            //                                  ProvExRate = l.ProvisionExchangeRate,
            //                                  SalesExRate = l.SalesExchangeRate,
            //                                  Provision = l.ProvisionHome,
            //                                  Sales = l.SalesHome,
            //                                  Profit = (l.SalesHome - l.ProvisionHome)

            //                              }).ToList();

            //        foreach (var item in vJobRegisterVMlist)
            //        {
            //            JobRegisterVM a = new JobRegisterVM();
            //            a.JobCode = item.JobCode;
            //            a.JobDate = item.JobDate;
            //            a.InvoiceNo = item.InvoiceNo;
            //            a.JobID = item.JobID;
            //            a.Customer = item.Customer;
            //            a.Provision = item.Provision;
            //            a.Sales = item.Sales;
            //            a.Profit = (a.Sales - a.Provision);
            //            tempJobRegisterVMlist.Add(a);
            //        }

            //        view = this.RenderPartialView("ucJobRegister", tempJobRegisterVMlist);

            //    }
            //    else if (currid != 0)
            //    {
            //        vJobRegisterVMlist = (from j in entity.JobGenerations
            //                              join k in entity.CUSTOMERs on j.ShipperID equals k.CustomerID
            //                              join l in entity.JInvoices on j.JobID equals l.JobID
            //                              where j.JobDate != null && j.ShipperID.HasValue
            //                               && vjobid > 0 ? j.JobID == (vjobid) : false
            //                                  && j.ShipperID.HasValue ? j.ShipperID.Value == (vcustid) : false
            //                                      &&
            //                     (
            //                   (frmdate == DateTime.MinValue) ? true : j.JobDate >= frmdate
            //                   &&
            //                   (tdate == DateTime.MinValue) ? true : j.JobDate <= tdate
            //                     )
            //                              select new JobRegisterVM
            //                              {
            //                                  JobCode = j.JobCode,
            //                                  JobDate = j.JobDate.Value,
            //                                  InvoiceNo = j.InvoiceNo,
            //                                  Customer = k.Customer1,
            //                                  ProvExRate = l.ProvisionExchangeRate,
            //                                  SalesExRate = l.SalesExchangeRate,
            //                                  Provision = l.ProvisionHome,
            //                                  JobID=j.JobID,
            //                                  Sales = l.SalesHome,
            //                                  Profit = (l.SalesHome - l.ProvisionHome)


            //                              }).ToList();

            //        foreach (var item in vJobRegisterVMlist)
            //        {
            //            JobRegisterVM a = new JobRegisterVM();
            //            a.JobCode = item.JobCode;
            //            a.JobDate = item.JobDate;
            //            a.JobID = item.JobID;
            //            a.InvoiceNo = item.InvoiceNo;
            //            a.Customer = item.Customer;
            //            a.Provision = (item.Provision / item.ProvExRate) * exrate;
            //            a.Sales = (item.Sales / item.SalesExRate) * exrate;
            //            a.Profit = (a.Sales - a.Provision) ;
            //            tempJobRegisterVMlist.Add(a);
            //        }

            //        view = this.RenderPartialView("ucJobRegister", tempJobRegisterVMlist);
            //    }
            //}


            //return new JsonResult
            // {
            //     Data = new
            //                {
            //                    success = true,
            //                    view = view
            //                },
            //     JsonRequestBehavior = JsonRequestBehavior.AllowGet
            // };

        }


        public JsonResult GetAllJobsForPrint(string custid, string jobid, string currencyid, DateTime frmdate, DateTime tdate)
        {
            ViewBag.customers = new SelectList(entity.CUSTOMERs.OrderBy(x => x.Customer1).ToList(), "CustomerID", "Customer1");
            ViewBag.jobs = new SelectList(entity.JobGenerations, "JobID", "JobCode");
            //DateTime datefrom = Convert.ToDateTime(frm);
            //ViewBag.customers = entity.CUSTOMERs.ToList();
            //ViewBag.jobs = entity.JobGenerations.ToList();

            string view = "";

            decimal exrate = 0;
            int currid;

            int vcustid;
            int vjobid;

            int jobId;
            //DateTime dateto = Convert.ToDateTime(dto);
            if (currencyid == "")
            {
                currid = 0;
            }
            else
            {
                currid = Convert.ToInt32(currencyid);
                exrate = (from x in entity.CurrencyMasters where x.CurrencyID == currid select x.ExchangeRate).FirstOrDefault().Value;
            }

            if (custid == "")
            {
                vcustid = 0;
            }
            else
            {
                vcustid = Convert.ToInt32(custid);
            }

            if (jobid == "")
            {
                vjobid = 0;
            }
            else
            {
                vjobid = Convert.ToInt32(jobid);
            }


            if (jobid == "")
            {
                jobId = 0;
            }
            else
            {
                jobId = Convert.ToInt32(jobid);
            }





            decimal provisionCurrency = (from t in entity.JInvoices where t.JobID == jobId select t.ProvisionExchangeRate.Value).FirstOrDefault();
            decimal SalesCurrency = (from t in entity.JInvoices where t.JobID == jobId select t.SalesExchangeRate.Value).FirstOrDefault();
            var vJobRegisterVMlist = new List<JobRegisterVM>();
            var tempJobRegisterVMlist = new List<JobRegisterVM>();



            if (vcustid == 0 && vjobid == 0)
            {
                if (currid == 0)
                {
                    vJobRegisterVMlist = (from j in entity.JobGenerations
                                          join k in entity.CUSTOMERs on j.ShipperID equals k.CustomerID
                                          join l in entity.JInvoices on j.JobID equals l.JobID
                                          where
                                         (
                                       (frmdate == DateTime.MinValue) ? true : j.JobDate >= frmdate
                                       &&
                                       (tdate == DateTime.MinValue) ? true : j.JobDate <= tdate
                                         )
                                          select new JobRegisterVM
                                          {
                                              JobCode = j.JobCode,
                                              JobDate = j.JobDate.Value,
                                              InvoiceNo = j.InvoiceNo,
                                              Customer = k.Customer1,
                                              Provision = l.ProvisionHome,
                                              Sales = l.SalesHome,
                                              Profit = (l.SalesHome - l.ProvisionHome)

                                          }).ToList();

                    foreach (var item in vJobRegisterVMlist)
                    {
                        JobRegisterVM a = new JobRegisterVM();
                        a.JobCode = item.JobCode;
                        a.JobDate = item.JobDate;
                        a.InvoiceNo = item.InvoiceNo;
                        a.Customer = item.Customer;
                        a.Provision = item.Provision;
                        a.Sales = item.Sales;
                        a.Profit = (a.Sales - a.Provision);
                        tempJobRegisterVMlist.Add(a);
                    }



                    view = this.RenderPartialView("ucJobPrint", tempJobRegisterVMlist);
                }
                else if (currid != 0)
                {
                    vJobRegisterVMlist = (from j in entity.JobGenerations
                                          join k in entity.CUSTOMERs on j.ShipperID equals k.CustomerID
                                          join l in entity.JInvoices on j.JobID equals l.JobID
                                          where
                                    (
                                  (frmdate == DateTime.MinValue) ? true : j.JobDate >= frmdate
                                  &&
                                  (tdate == DateTime.MinValue) ? true : j.JobDate <= tdate
                                    )
                                          select new JobRegisterVM
                                          {
                                              JobCode = j.JobCode,
                                              JobDate = j.JobDate.Value,
                                              InvoiceNo = j.InvoiceNo,
                                              Customer = k.Customer1,
                                              ProvExRate = l.ProvisionExchangeRate,
                                              SalesExRate = l.SalesExchangeRate,
                                              Provision = l.ProvisionHome,
                                              Sales = l.SalesHome,
                                              Profit = (l.SalesHome - l.ProvisionHome)

                                          }).ToList();

                    foreach (var item in vJobRegisterVMlist)
                    {
                        JobRegisterVM a = new JobRegisterVM();
                        a.JobCode = item.JobCode;
                        a.JobDate = item.JobDate;
                        a.InvoiceNo = item.InvoiceNo;
                        a.Customer = item.Customer;
                        a.Provision = (item.Provision / item.ProvExRate) * exrate;
                        a.Sales = (item.Sales / item.SalesExRate) * exrate;
                        a.Profit = (a.Sales - a.Provision);
                        tempJobRegisterVMlist.Add(a);
                    }

                    view = this.RenderPartialView("ucJobPrint", tempJobRegisterVMlist);
                }


            }
            else if (vcustid != 0 && vjobid == 0)
            {
                if (currid == 0)
                {
                    vJobRegisterVMlist = (from j in entity.JobGenerations
                                          join k in entity.CUSTOMERs on j.ShipperID equals k.CustomerID
                                          join l in entity.JInvoices on j.JobID equals l.JobID
                                          where j.JobDate != null && j.ShipperID.HasValue
                                             && j.ShipperID.HasValue ? j.ShipperID.Value == (vcustid) : false
                                     &&
                                 (
                               (frmdate == DateTime.MinValue) ? true : j.JobDate >= frmdate
                               &&
                               (tdate == DateTime.MinValue) ? true : j.JobDate <= tdate
                                 )
                                          select new JobRegisterVM
                                          {
                                              JobCode = j.JobCode,
                                              JobDate = j.JobDate.Value,
                                              InvoiceNo = j.InvoiceNo,
                                              Customer = k.Customer1,
                                              ProvExRate = l.ProvisionExchangeRate,
                                              SalesExRate = l.SalesExchangeRate,
                                              Provision = l.ProvisionHome,
                                              Sales = l.SalesHome,
                                              Profit = (l.SalesHome - l.ProvisionHome)

                                          }).ToList();

                    foreach (var item in vJobRegisterVMlist)
                    {
                        JobRegisterVM a = new JobRegisterVM();
                        a.JobCode = item.JobCode;
                        a.JobDate = item.JobDate;
                        a.InvoiceNo = item.InvoiceNo;
                        a.Customer = item.Customer;
                        a.Provision = item.Provision;
                        a.Sales = item.Sales;
                        a.Profit = (a.Sales - a.Provision);
                        tempJobRegisterVMlist.Add(a);
                    }

                    view = this.RenderPartialView("ucJobPrint", tempJobRegisterVMlist);
                }
                else if (currid != 0)
                {
                    vJobRegisterVMlist = (from j in entity.JobGenerations
                                          join k in entity.CUSTOMERs on j.ShipperID equals k.CustomerID
                                          join l in entity.JInvoices on j.JobID equals l.JobID
                                          where j.JobDate != null && j.ShipperID.HasValue
                                             && j.ShipperID.HasValue ? j.ShipperID.Value == (vcustid) : false
                                                 &&
                                 (
                               (frmdate == DateTime.MinValue) ? true : j.JobDate >= frmdate
                               &&
                               (tdate == DateTime.MinValue) ? true : j.JobDate <= tdate
                                 )
                                          select new JobRegisterVM
                                          {
                                              JobCode = j.JobCode,
                                              JobDate = j.JobDate.Value,
                                              InvoiceNo = j.InvoiceNo,
                                              Customer = k.Customer1,
                                              ProvExRate = l.ProvisionExchangeRate,
                                              SalesExRate = l.SalesExchangeRate,
                                              Provision = l.ProvisionHome,
                                              Sales = l.SalesHome,
                                              Profit = (l.SalesHome - l.ProvisionHome)


                                          }).ToList();

                    foreach (var item in vJobRegisterVMlist)
                    {
                        JobRegisterVM a = new JobRegisterVM();
                        a.JobCode = item.JobCode;
                        a.JobDate = item.JobDate;
                        a.InvoiceNo = item.InvoiceNo;
                        a.Customer = item.Customer;
                        a.Provision = (item.Provision / item.ProvExRate) * exrate;
                        a.Sales = (item.Sales / item.SalesExRate) * exrate;
                        a.Profit = (a.Sales - a.Provision);
                        tempJobRegisterVMlist.Add(a);
                    }

                    view = this.RenderPartialView("ucJobPrint", tempJobRegisterVMlist);
                }

            }

            else if (vcustid == 0 && vjobid != 0)
            {
                if (currid == 0)
                {
                    vJobRegisterVMlist = (from j in entity.JobGenerations
                                          join k in entity.CUSTOMERs on j.ShipperID equals k.CustomerID
                                          join l in entity.JInvoices on j.JobID equals l.JobID
                                          where j.JobDate != null && j.ShipperID.HasValue
                                                && vjobid > 0 ? j.JobID == (vjobid) : false
                                                    &&
                                 (
                               (frmdate == DateTime.MinValue) ? true : j.JobDate >= frmdate
                               &&
                               (tdate == DateTime.MinValue) ? true : j.JobDate <= tdate
                                 )
                                          select new JobRegisterVM
                                          {
                                              JobCode = j.JobCode,
                                              JobDate = j.JobDate.Value,
                                              InvoiceNo = j.InvoiceNo,
                                              Customer = k.Customer1,
                                              ProvExRate = l.ProvisionExchangeRate,
                                              SalesExRate = l.SalesExchangeRate,
                                              Provision = l.ProvisionHome,
                                              Sales = l.SalesHome,
                                              Profit = (l.SalesHome - l.ProvisionHome)

                                          }).ToList();

                    foreach (var item in vJobRegisterVMlist)
                    {
                        JobRegisterVM a = new JobRegisterVM();
                        a.JobCode = item.JobCode;
                        a.JobDate = item.JobDate;
                        a.InvoiceNo = item.InvoiceNo;
                        a.Customer = item.Customer;
                        a.Provision = item.Provision;
                        a.Sales = item.Sales;
                        a.Profit = (a.Sales - a.Provision);
                        tempJobRegisterVMlist.Add(a);
                    }

                    view = this.RenderPartialView("ucJobPrint", tempJobRegisterVMlist);
                }
                else if (currid != 0)
                {
                    vJobRegisterVMlist = (from j in entity.JobGenerations
                                          join k in entity.CUSTOMERs on j.ShipperID equals k.CustomerID
                                          join l in entity.JInvoices on j.JobID equals l.JobID
                                          where j.JobDate != null && j.ShipperID.HasValue
                                               && vjobid > 0 ? j.JobID == (vjobid) : false
                                                   &&
                                 (
                               (frmdate == DateTime.MinValue) ? true : j.JobDate >= frmdate
                               &&
                               (tdate == DateTime.MinValue) ? true : j.JobDate <= tdate
                                 )
                                          select new JobRegisterVM
                                          {
                                              JobCode = j.JobCode,
                                              JobDate = j.JobDate.Value,
                                              InvoiceNo = j.InvoiceNo,
                                              Customer = k.Customer1,
                                              ProvExRate = l.ProvisionExchangeRate,
                                              SalesExRate = l.SalesExchangeRate,
                                              Provision = l.ProvisionHome,
                                              Sales = l.SalesHome,
                                              Profit = (l.SalesHome - l.ProvisionHome)


                                          }).ToList();

                    foreach (var item in vJobRegisterVMlist)
                    {
                        JobRegisterVM a = new JobRegisterVM();
                        a.JobCode = item.JobCode;
                        a.JobDate = item.JobDate;
                        a.InvoiceNo = item.InvoiceNo;
                        a.Customer = item.Customer;
                        a.Provision = (item.Provision / item.ProvExRate) * exrate;
                        a.Sales = (item.Sales / item.SalesExRate) * exrate;
                        a.Profit = (a.Sales - a.Provision);
                        tempJobRegisterVMlist.Add(a);
                    }

                    view = this.RenderPartialView("ucJobPrint", tempJobRegisterVMlist);

                }
            }
            else if (vcustid != 0 && vjobid != 0)
            {
                if (currid == 0)
                {
                    vJobRegisterVMlist = (from j in entity.JobGenerations
                                          join k in entity.CUSTOMERs on j.ShipperID equals k.CustomerID
                                          join l in entity.JInvoices on j.JobID equals l.JobID
                                          where j.JobDate != null && j.ShipperID.HasValue
                                           && vjobid > 0 ? j.JobID == (vjobid) : false
                                              && j.ShipperID.HasValue ? j.ShipperID.Value == (vcustid) : false
                                                  &&
                                 (
                               (frmdate == DateTime.MinValue) ? true : j.JobDate >= frmdate
                               &&
                               (tdate == DateTime.MinValue) ? true : j.JobDate <= tdate
                                 )
                                          select new JobRegisterVM
                                          {
                                              JobCode = j.JobCode,
                                              JobDate = j.JobDate.Value,
                                              InvoiceNo = j.InvoiceNo,
                                              Customer = k.Customer1,
                                              ProvExRate = l.ProvisionExchangeRate,
                                              SalesExRate = l.SalesExchangeRate,
                                              Provision = l.ProvisionHome,
                                              Sales = l.SalesHome,
                                              Profit = (l.SalesHome - l.ProvisionHome)

                                          }).ToList();

                    foreach (var item in vJobRegisterVMlist)
                    {
                        JobRegisterVM a = new JobRegisterVM();
                        a.JobCode = item.JobCode;
                        a.JobDate = item.JobDate;
                        a.InvoiceNo = item.InvoiceNo;
                        a.Customer = item.Customer;
                        a.Provision = item.Provision;
                        a.Sales = item.Sales;
                        a.Profit = (a.Sales - a.Provision);
                        tempJobRegisterVMlist.Add(a);
                    }

                    view = this.RenderPartialView("ucJobPrint", tempJobRegisterVMlist);

                }
                else if (currid != 0)
                {
                    vJobRegisterVMlist = (from j in entity.JobGenerations
                                          join k in entity.CUSTOMERs on j.ShipperID equals k.CustomerID
                                          join l in entity.JInvoices on j.JobID equals l.JobID
                                          where j.JobDate != null && j.ShipperID.HasValue
                                           && vjobid > 0 ? j.JobID == (vjobid) : false
                                              && j.ShipperID.HasValue ? j.ShipperID.Value == (vcustid) : false
                                                  &&
                                 (
                               (frmdate == DateTime.MinValue) ? true : j.JobDate >= frmdate
                               &&
                               (tdate == DateTime.MinValue) ? true : j.JobDate <= tdate
                                 )
                                          select new JobRegisterVM
                                          {
                                              JobCode = j.JobCode,
                                              JobDate = j.JobDate.Value,
                                              InvoiceNo = j.InvoiceNo,
                                              Customer = k.Customer1,
                                              ProvExRate = l.ProvisionExchangeRate,
                                              SalesExRate = l.SalesExchangeRate,
                                              Provision = l.ProvisionHome,
                                              Sales = l.SalesHome,
                                              Profit = (l.SalesHome - l.ProvisionHome)


                                          }).ToList();

                    foreach (var item in vJobRegisterVMlist)
                    {
                        JobRegisterVM a = new JobRegisterVM();
                        a.JobCode = item.JobCode;
                        a.JobDate = item.JobDate;
                        a.InvoiceNo = item.InvoiceNo;
                        a.Customer = item.Customer;
                        a.Provision = (item.Provision / item.ProvExRate) * exrate;
                        a.Sales = (item.Sales / item.SalesExRate) * exrate;
                        a.Profit = (a.Sales - a.Provision);
                        tempJobRegisterVMlist.Add(a);
                    }

                    view = this.RenderPartialView("ucJobPrint", tempJobRegisterVMlist);
                }
            }


            return new JsonResult
            {
                Data = new
                {
                    success = true,
                    view = view
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }


        public JsonResult GetJobs()
        {


            DateTime frmdate = Convert.ToDateTime(Session["FyearFrom"].ToString());
            DateTime tdate = Convert.ToDateTime(Session["FyearTo"].ToString());


            var vJobRegisterVMlist = new List<JobRegisterVM>();


            vJobRegisterVMlist = (from j in entity.JobGenerations
                                  join k in entity.CUSTOMERs on j.ShipperID equals k.CustomerID
                                  join l in entity.JInvoices on j.JobID equals l.JobID
                                     where
                                 (
                               ( j.JobDate >= frmdate
                               &&
                               ( j.JobDate <= tdate
                                 )))
                                  select new JobRegisterVM
                                  {
                                      JobCode = j.JobCode,
                                      JobID=j.JobID,
                                      JobDate = j.JobDate.Value,
                                      InvoiceNo = j.InvoiceNo,
                                      Customer = k.Customer1,
                                      Provision = l.ProvisionHome,
                                      Sales = l.SalesHome,
                                      Profit = (l.SalesHome - l.ProvisionHome)

                                  }).Take(100).ToList();


            string view = this.RenderPartialView("ucJobRegister", vJobRegisterVMlist);

            return new JsonResult
            {
                Data = new
                {
                    success = true,
                    view = view
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }


        public JsonResult GetJobsByCustomerID(string customerid,string fdate,string todate)
        {

            DateTime from = Convert.ToDateTime(fdate);
            DateTime tdate = Convert.ToDateTime(todate);
            if (!string.IsNullOrEmpty(customerid))
            {

                int Id = Convert.ToInt32(customerid);
                var states = (from a in entity.JobGenerations where a.ShipperID == Id select a).Where(z=>z.JobDate>=from && z.JobDate<=tdate).OrderBy(x=>x.JobCode).ToList();
                return Json(states);
            }
            else
            {
                var states = (from a in entity.JobGenerations select a).Where(z => z.JobDate >= from && z.JobDate <= tdate).OrderBy(x => x.JobCode).ToList();
                return Json(states, JsonRequestBehavior.AllowGet);
            }

        }

        public LargeJsonResult getDataForPrintByJobid(int JobID)
        {
            AcCompany c = entity.AcCompanies.FirstOrDefault();
            ViewBag.companyname = c.AcCompany1;
            ViewBag.caddress1 = c.Address1;
            ViewBag.caddress2 = c.Address2;
            ViewBag.caddress3 = c.Address3;
            ViewBag.cphone = c.Phone;
            string view = "";

           // var data = entity.SP_JobRegisterReportPrint(JobID).FirstOrDefault();
          //  view = this.RenderPartialView("ucJobPrint", data);
            return new LargeJsonResult
            {
                Data = new
                {
                    success = true,
                    view = view
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        public ActionResult GetCustomerInvoiceReport()
        {
            return View();
        }
        //public ActionResult JobRegisterFormat()
        //{

        //    var query = (from t in entity.JobGenerations
        //                 select new JobRegisterVM
        //                 {
        //                     JobCode = t.JobCode,
        //                     JobTypeID = t.JobTypeID.Value

        //                 }).ToList();

        //    ViewBag.jobRegister = query;
        //    ViewBag.CmpnyName = "TriangleSoft";
        //    return View(query);
        //}
    }
}
public static class MvcHelpers1
{
    public static string RenderPartialView(this Controller controller, string viewName, object model)
    {
        if (string.IsNullOrEmpty(viewName))
            viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

        controller.ViewData.Model = model;
        using (var sw = new StringWriter())
        {
            ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
            var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
            viewResult.View.Render(viewContext, sw);

            return sw.GetStringBuilder().ToString();
        }
    }
}