﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using AMCAAuditing.Models;
using System.Configuration;
using System.Net.Mail;
using AMCAAuditing.BusinessLogic;
using CaptchaMvc.HtmlHelpers;

namespace AMCAAuditing.Controllers
{
    public class ServicesController : Controller
    {
        [NonAction]
        public string SendMail(string ToMailID, string CC, string BCC, string subject, string body)
        {
            string servername = "", FromMailID = "", fromEmailPassword = "";
            int PortNo = 0;
            bool ssl;

            string msg = string.Empty;
            DataTable dtg = GetGeneralSender(1); // 1 is company id
            servername = "smtp.office365.com"; PortNo = 587; ssl = true; FromMailID = dtg.Rows[0]["SenderEmail"].ToString(); fromEmailPassword = dtg.Rows[0]["SenderPassword"].ToString();
            MailMessage MailMsg = new MailMessage();
            try
            {
                if (ToMailID.EndsWith(","))
                {
                    ToMailID = ToMailID.Substring(0, ToMailID.Length - 1);
                }
                if (CC.EndsWith(","))
                {
                    CC = CC.Substring(0, CC.Length - 1);

                }
                if (BCC.EndsWith(","))
                {
                    BCC = BCC.Substring(0, BCC.Length - 1);

                }

                MailMsg.To.Add(ToMailID);
                if (CC != "")
                {
                    MailMsg.CC.Add(CC);
                }
                if (BCC != "")
                {
                    MailMsg.Bcc.Add(BCC);
                }

                //=================
                MailMsg.From = new MailAddress(FromMailID);
                MailMsg.Subject = subject;
                MailMsg.IsBodyHtml = true;
                MailMsg.Body = body;

                SmtpClient tempsmtp = new SmtpClient();
                tempsmtp.Host = servername;
                tempsmtp.Port = PortNo;
                tempsmtp.Credentials = new System.Net.NetworkCredential(FromMailID, fromEmailPassword);
                tempsmtp.EnableSsl = ssl;

                tempsmtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                tempsmtp.Send(MailMsg);
                msg = "Successful";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }
        public DataTable GetGeneralSender(int companyid)
        {
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 69;
            PL.AutoId = companyid;
            ServiceModelD.returnTable(PL);
            return PL.dt;
        }
        [NonAction]
        public SelectList ToSelectList(DataTable table, string valueField, string textField)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (DataRow row in table.Rows)
            {
                list.Add(new SelectListItem()
                {
                    Text = row[textField].ToString(),
                    Value = row[valueField].ToString()
                });
            }
            return new SelectList(list, "Value", "Text");
        }
        void BindDropDown(){
            //Select Authority
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 1;
            ServiceModelD.returnTable(PL);
            ViewBag.TradeLicenseAutList = ToSelectList(PL.dt, "TradeLicenseAuthority", "TradeLicenseAuthority");

            //Select Country Code
            PL.OpCode = 2;
            ServiceModelD.returnTable(PL);
            ViewBag.CountryList = ToSelectList(PL.dt, "CountryISDCode", "CountryName");

            //Select About AMCA
            PL.OpCode = 4;
            ServiceModelD.returnTable(PL);
            ViewBag.AboutAMCA = ToSelectList(PL.dt, "AutoId", "AboutAMCA");

            //select service
            //PL.OpCode = 10;
            //ServiceModelD.returnTable(PL);
            //ViewBag.Service = ToSelectList(PL.dt, "ServiceName", "ServiceName");
            PL.OpCode = 3;
            PL.ServiceType = Session["SER"].ToString();
            ServiceModelD.returnTable(PL);
            ViewBag.Service = ToSelectList(PL.dt, "SubServiceName", "SubServiceName");
        }
       
        public List<InsightModel> BlogSearch(int? ServiceType)
        {

            Insight ins = new Insight();
            //DataSet ds = ins.GetAllBlogs();
            DataSet ds = ins.GetAllBlogsBySearch(ServiceType);
            InsightModel insMod = new InsightModel();
            DataTable dt = ds.Tables[0];
            List<InsightModel> datatable = new List<InsightModel>();
            foreach (DataRow row in dt.Rows)
            {
                datatable.Add(
                   new InsightModel
                   {
                       BlogID = (int)(row["AutoID"]),
                       BlogTitle = Convert.ToString(row["BlogTitle"]),
                       PageTitle = Convert.ToString(row["PageTitle"]),
                       Keyword = Convert.ToString(row["Keyword"]),
                       Description = Convert.ToString(row["Description"]),
                       ImageURL = Convert.ToString(row["ImageURL"]),
                       IsActive = Convert.ToString(row["IsActive"]),
                       CreatedOn = Convert.ToString(row["CreatedOn"]),
                       BlogContent = Convert.ToString(row["BlogContent"]),
                       Designation = Convert.ToString(row["Designation"]),
                       pageUrlText = Convert.ToString(row["pageUrlText"]),
                       CreatedBy = Convert.ToString(row["CreatedBy"])
                   }
                );
            }

           return datatable;
        }
       
        [HttpGet]
        public ActionResult ServicesQuery()
        {
            int BlogNo = Int32.Parse(Session["BlogSearch"].ToString());
            ViewData["IndexInsight"] = BlogSearch(BlogNo);
            BindDropDown();
            return View();
        }

        [Route("audit-services-in-dubai")]
        public ActionResult AuditAssurance()
        {
            string AuditAssurance = "8";
            Session["SER"] = "8";
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 3;
            PL.ServiceType = AuditAssurance;
            ServiceModelD.returnTable(PL);
            ViewBag.Service = ToSelectList(PL.dt, "SubServiceName", "SubServiceName");
            Session["txtPageName"] = "AuditAssurance";
            ViewData["IndexInsight"] = BlogSearch(5);


            string AuditAssuranceId = "1";
            PL.OpCode = 40;
            PL.ServiceType = AuditAssuranceId;
            ServiceModelD.returnTable(PL);
            ViewBag.AuditAsssuranceSubServices = ToSelectList(PL.dt, "Id", "SubServiceName");

            Session["BlogSearch"] = 5;
            return ServicesQuery();
        }
        [Route("accounting-bookkeeping-services")]
        public ActionResult AccountingBookkeeping()
        {
            string AccountingBookkeeping = "9";
            Session["SER"] = "9";
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 3;
            PL.ServiceType = AccountingBookkeeping;
            ServiceModelD.returnTable(PL);
            ViewBag.Service = ToSelectList(PL.dt, "SubServiceName", "SubServiceName");
            Session["txtPageName"] = "AccountingBookkeeping";
            ViewData["IndexInsight"] = BlogSearch(2);
             
            PL.OpCode = 46; 
            ServiceModelD.returnTable(PL);
            ViewBag.AuditAsssuranceSubServices = ToSelectList(PL.dt, "Id", "SubServiceName");

            Session["BlogSearch"] = 2;
            return ServicesQuery();
        }
        [Route("vat-consultant-in-dubai")]
        public ActionResult VatConsulting()
        {
            string VatConsulting = "10";
            Session["SER"] = "10";
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 3;
            PL.ServiceType = VatConsulting;
            ServiceModelD.returnTable(PL);
            ViewBag.Service = ToSelectList(PL.dt, "SubServiceName", "SubServiceName");
            Session["txtPageName"] = "VatConsulting";
            ViewData["IndexInsight"] = BlogSearch(2);
            Session["BlogSearch"] = 2;

            PL.OpCode = 47;
            ServiceModelD.returnTable(PL);
            ViewBag.AuditAsssuranceSubServices = ToSelectList(PL.dt, "Id", "SubServiceName");

            return ServicesQuery();
        }
        [Route("company-liquidation-in-dubai")]
        public ActionResult Liquidation()
        {
            string Liquidation = "11";
            Session["SER"] = "11";
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 3;
            PL.ServiceType = Liquidation;
            ServiceModelD.returnTable(PL);
            ViewBag.Service = ToSelectList(PL.dt, "SubServiceName", "SubServiceName");
            Session["txtPageName"] = "Liquidation";
            ViewData["IndexInsight"] = BlogSearch(6);
            Session["BlogSearch"] = 6;

            PL.OpCode = 49;
            ServiceModelD.returnTable(PL);
            ViewBag.AuditAsssuranceSubServices = ToSelectList(PL.dt, "Id", "SubServiceName");

            return ServicesQuery();
        }
        [Route("corporate-tax-services-in-uae")]
        public ActionResult CorporateTaxServices()
        {
            string CorporateTax = "12";
            Session["SER"] = "12";
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 3;
            PL.ServiceType = CorporateTax;
            ServiceModelD.returnTable(PL);
            ViewBag.Service = ToSelectList(PL.dt, "SubServiceName", "SubServiceName");
            Session["txtPageName"] = "CorporateTaxServices";
            ViewData["IndexInsight"] = BlogSearch(10);
            Session["BlogSearch"] = 10;

            PL.OpCode = 50;
            ServiceModelD.returnTable(PL);
            ViewBag.AuditAsssuranceSubServices = ToSelectList(PL.dt, "Id", "SubServiceName");

            return ServicesQuery();
        }
        [Route("tax-residence-certificate-in-dubai")]
        public ActionResult TRC()
        {
            string TRCService = "13";
            Session["SER"] = "13";
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 3;
            PL.ServiceType = TRCService;
            ServiceModelD.returnTable(PL);
            ViewBag.Service = ToSelectList(PL.dt, "SubServiceName", "SubServiceName");
            Session["txtPageName"] = "TRC";
            ViewData["IndexInsight"] = BlogSearch(10);
            Session["BlogSearch"] = 10;

            PL.OpCode = 48;
            ServiceModelD.returnTable(PL);
            ViewBag.AuditAsssuranceSubServices = ToSelectList(PL.dt, "Id", "SubServiceName");

            return ServicesQuery();
        }
        [Route("excise-tax-uae")]
        public ActionResult ExciseTaxServices()
        {
            string ExciseTax = "14";
            Session["SER"] = "14";
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 3;
            PL.ServiceType = ExciseTax;
            ServiceModelD.returnTable(PL);
            ViewBag.Service = ToSelectList(PL.dt, "SubServiceName", "SubServiceName");
            Session["txtPageName"] = "ExciseTaxServices";
            ViewData["IndexInsight"] = BlogSearch(10);
            Session["BlogSearch"] = 10;

            PL.OpCode = 68;
            ServiceModelD.returnTable(PL);
            ViewBag.AuditAsssuranceSubServices = ToSelectList(PL.dt, "Id", "SubServiceName");

            return ServicesQuery();
        }

        [HttpPost]
        public ActionResult ServicesQuery(string LeadDataType,string AutoId, string CompanyName, string TradeLicenseAuthority, string ConcernPerson, string CountryCodeContact, string ContactNumber, string EmailId, string Service, string AboutAMCA, Validation model , string SubServiceId)
        {
            if (this.IsCaptchaValid(errorText: ""))
            {

                ServiceModel PL = new ServiceModel();
                PL.OpCode = 70;
                PL.AutoId = Convert.ToInt32(AutoId);
                PL.CompanyName = CompanyName;
                PL.TradeLicenseAuthority = TradeLicenseAuthority;
                PL.ConcernPerson = ConcernPerson;
                PL.CountryCodeContact = CountryCodeContact;
                PL.ContactNumber = ContactNumber;
                PL.EmailId = EmailId;
                PL.Service = Service;
                PL.AboutAMCA = AboutAMCA;
                PL.LeadDataType = LeadDataType;
                PL.ServiceType = SubServiceId;
                var txtPageName = Session["txtPageName"].ToString();
                ServiceModelD.returnTable(PL);

                string serviceName = PL.dt.Rows[0]["ServiceName"].ToString();

                //Sending Mail
                var body = "";
                body += "<p style='margin-top: 0px'>Dear AMCA Marketing Team,</p>";
                body += "<p style='margin-top: 2px'>Please assign the following lead to one of the BDs:</p>";
                body += " <table width='600' border='1' cellpadding='5' cellspacing='0'> " +
                    "<tr> <td> <strong> Company Name </strong></td> <td>" + CompanyName + " </td></tr>" +
                    "<tr> <td> <strong> Contact Person </strong></td> <td>" + ConcernPerson + " </td></tr>" +
                    "<tr> <td> <strong> Trade License Authority </strong></td> <td>" + TradeLicenseAuthority + " </td></tr>" +
                    "<tr> <td> <strong> Contact Number/s </strong></td> <td>" + CountryCodeContact + " " + ContactNumber + " </td></tr>" +
                    "<tr> <td> <strong> Email Id </strong></td> <td>" + EmailId + " </td></tr> " +
                    "<tr> <td> <strong> Service </strong></td> <td>" + serviceName + " </td></tr>" +
                    "<tr> <td> <strong> Where did you hear about AMCA? </strong></td> <td>" + AboutAMCA + " </td></tr>" +
                    "<tr> <td> <strong> AMCA-AUDITING: </strong></td> <td>" + txtPageName + " </td></tr>" +
                    "</table>";
                body += "<p>Regards,<br>AMCA</p>";

                var toMail = new clsgeneral().getDepartmentReceiver(43);
                var msg = SendMail(toMail, "", "", "Assign Lead to BD", body);
                //var msg = "";
                // return RedirectToAction("Thankyou", "Pages");
                if (txtPageName.ToString() == "AuditAssurance")
                {
                    return RedirectToAction("ThankyouAudit", "Pages");
                }
                else
                {
                    return RedirectToAction("Thankyou", "Pages");
                }

            }
            else
            {
                var txtPageName = Session["txtPageName"].ToString();
            
               // BindDropDown();
                ServicesQuery();
                ViewBag.ErrorMessage = "Error: captcha is not valid.";

                ServiceModel PL = new ServiceModel();
                if (txtPageName == "AuditAssurance")
                { 
                    PL.OpCode = 40; 
                }
                if (txtPageName == "AccountingBookkeeping")
                {
                    PL.OpCode = 46;
                }
                if (txtPageName == "VatConsulting")
                {
                    PL.OpCode = 47;
                }
                if (txtPageName == "Liquidation")
                {
                    PL.OpCode = 49;
                }
                if (txtPageName == "CorporateTaxServices")
                {
                    PL.OpCode = 50;
                }
                if (txtPageName == "AccountingBookkeeping")
                {
                    PL.OpCode = 46;
                }
                if (txtPageName == "TRC")
                {
                    PL.OpCode = 48;
                }
                if (txtPageName == "ExciseTaxServices")
                {
                    PL.OpCode = 68;
                }

                ServiceModelD.returnTable(PL);
                ViewBag.AuditAsssuranceSubServices = ToSelectList(PL.dt, "Id", "SubServiceName");
                return View(txtPageName);

            }
        }
    
    }
}