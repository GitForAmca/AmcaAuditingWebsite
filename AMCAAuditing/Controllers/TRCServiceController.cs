using System;
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

    public class TRCServiceController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Capture UTM Source
            var query = Request.QueryString;
            if (!string.IsNullOrEmpty(query["utm_source"]))
            {
                Session["utm_source"] = query["utm_source"];
            }
            if (!string.IsNullOrEmpty(query["utm_medium"]))
            {
                Session["utm_medium"] = query["utm_medium"];
            }
            if (!string.IsNullOrEmpty(query["utm_campaign"]))
            {
                Session["utm_campaign"] = query["utm_campaign"];
            }
            if (!string.IsNullOrEmpty(query["utm_term"]))
            {
                Session["utm_term"] = query["utm_term"];
            }
            if (!string.IsNullOrEmpty(query["utm_content"]))
            {
                Session["utm_content"] = query["utm_content"];
            }
            base.OnActionExecuting(filterContext);
        }
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
        void BindDropDown()
        {

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

            string TRCServices = "13";
            PL.OpCode = 3;
            PL.ServiceType = TRCServices;
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
            ViewData["IndexInsight"] = BlogSearch(10);
            BindDropDown();
            return View();
        }

        // GET: TRCService
        public ActionResult Index()
        {
            return View();
        }


        [Route("trc-for-natural-person-in-uae")]
        public ActionResult TRCforNaturalPerson()
        {
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 61;
            ServiceModelD.returnTable(PL);
            ViewBag.TRC_SubServices = ToSelectList(PL.dt, "Id", "SubServiceName");

            Session["txtPageName"] = "TRCforNaturalPerson";
            return ServicesQuery();
        }
        [Route("trc-for-legal-person-in-uae")]
        public ActionResult TRCforLegalPerson()
        {
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 60;
            ServiceModelD.returnTable(PL);
            ViewBag.TRC_SubServices = ToSelectList(PL.dt, "Id", "SubServiceName");

            Session["txtPageName"] = "TRCforLegalPerson";
            return ServicesQuery();
        }

        //[Route("tax-residence-certificate-in-dubai")]
        //public ActionResult TRC()
        //{
        //    Session["txtPageName"] = "TRC";
        //    return ServicesQuery();
        //}

        [HttpPost]
        public ActionResult ServicesQuery(string LeadDataType, string AutoId, string CompanyName, string TradeLicenseAuthority, string ConcernPerson, string CountryCodeContact, string ContactNumber, string EmailId, string Service, string AboutAMCA, Validation model, string SubServiceId)
        {
            if (this.IsCaptchaValid(errorText: ""))
            {
                if(Session["txtPageName"].ToString() == "TRCforLegalPerson")
                {
                    LeadDataType = "Entity";
                }
                else
                {
                    LeadDataType = "Individual";
                }
                ServiceModel PL = new ServiceModel();
                PL.OpCode = 5;
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
                PL.UTMSource = Session["utm_source"] != null ? Session["utm_source"].ToString() : "";
                PL.UTMMedium = Session["utm_medium"] != null ? Session["utm_medium"].ToString() : "";
                PL.UTMCampaign = Session["utm_campaign"] != null ? Session["utm_campaign"].ToString() : "";
                PL.UTMTerm = Session["utm_term"] != null ? Session["utm_term"].ToString() : "";
                PL.UTMContent = Session["utm_content"] != null ? Session["utm_content"].ToString() : "";
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
                    "<tr> <td> <strong> AMCAAUDITING: </strong></td> <td>" + txtPageName + " </td></tr>" +
                    "</table>";
                body += "<p>Regards,<br>AMCA</p>";
                var toMail = new clsgeneral().getDepartmentReceiver(43);
                var msg = SendMail(toMail, "", "", "Assign Lead to BD", body);
                //var msg = "";
                return RedirectToAction("Thankyou", "Pages");

            }
            else
            {
                var txtPageName = Session["txtPageName"].ToString();
                //BindDropDown();
                ServicesQuery();
                ViewBag.ErrorMessage = "Error: captcha is not valid.";

                ServiceModel PL = new ServiceModel();
                if (txtPageName == "TRCforNaturalPerson")
                {
                    PL.OpCode = 61;
                    Session["txtPageName"] = "TRCforNaturalPerson";
                }
                if (txtPageName == "TRCforLegalPerson")
                {
                    PL.OpCode = 60;
                    Session["txtPageName"] = "TRCforLegalPerson";
                }

                ServiceModelD.returnTable(PL);
                ViewBag.TRC_SubServices = ToSelectList(PL.dt, "Id", "SubServiceName");

                return View(txtPageName);

            }
        }
    }
}
