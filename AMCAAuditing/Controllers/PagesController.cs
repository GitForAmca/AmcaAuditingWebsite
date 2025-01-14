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
    public class PagesController : Controller
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
        public DataTable GetAllBlogsHome()
        {
            Insight ins = new Insight();
            DataSet ds = ins.GetAllBlogsHome("Home");
            DataTable dt = ds.Tables[0];
            return dt;
        }
        public List<InsightModel> BindAllInsight()
        {
            //DataTable dt = GetAllBlogs(count);
            DataTable dt = GetAllBlogsHome();
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
        void BindDropDown()
        {
            // Select Authority
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
            ViewBag.AboutAMCA = ToSelectList(PL.dt, "AboutAMCA", "AboutAMCA");

            //select service
            PL.OpCode = 10;
            ServiceModelD.returnTable(PL);
            ViewBag.Service = ToSelectList(PL.dt, "ServiceName", "ServiceName");
             
            //select service
            PL.OpCode = 23;
            ServiceModelD.returnTable(PL);
            ViewBag.ServiceMain = ToSelectList(PL.dt, "AutoID", "Name");

            //Select AllServices
            PL.OpCode = 64;
            ServiceModelD.returnTable(PL);
            ViewBag.AllServices = ToSelectList(PL.dt, "Id", "Name");

            //select service
            PL.OpCode = 64;
            ServiceModelD.returnTable(PL);
            ViewBag.MainServices = ToSelectList(PL.dt, "Id", "Name");
        }
        [HttpGet]
        public ActionResult ServicesQuery()
        {
            ViewData["IndexInsight"] = BindAllInsight();

            BindDropDown();
            return View();
        }

        //public DataTable GetAllBlogs(int? count)
        //{
        //    Insight ins = new Insight();
        //    DataSet ds = ins.GetAllBlogs();

        //    DataTable dt = new DataTable();
        //    if (count != null)
        //    {
        //        dt = ds.Tables[0];
        //    }
        //    return dt;
        //}
        public DataTable GetAllBlogs()
        {

            Insight ins = new Insight();
            DataSet ds = ins.GetAllBlogs();
            DataTable dt = ds.Tables[0];

            return dt;
        }

        public ActionResult BlogSearch(int? ServiceType)
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

            return PartialView("_BlogPartialData", datatable);
        }
        [Route("blogs")]
        public ActionResult Blogs()
        {
            BindDropDown();
            return View();
        }
        //[Route("blogs")]
        //public ActionResult Blogs(int? ServiceType)
        //{
        //    Insight ins = new Insight();
        //    DataSet ds = ins.GetAllBlogs();
        //    //DataSet ds = ins.GetAllBlogsBySearch(ServiceType);
        //    InsightModel insMod = new InsightModel();
        //    DataTable dt = ds.Tables[0];
        //    List<InsightModel> datatable = new List<InsightModel>();
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        datatable.Add(
        //           new InsightModel 
        //           {
        //               BlogID = (int)(row["AutoID"]),
        //               BlogTitle = Convert.ToString(row["BlogTitle"]),
        //               PageTitle = Convert.ToString(row["PageTitle"]),
        //               Keyword = Convert.ToString(row["Keyword"]),
        //               Description = Convert.ToString(row["Description"]),
        //               ImageURL = Convert.ToString(row["ImageURL"]),
        //               IsActive = Convert.ToString(row["IsActive"]),
        //               CreatedOn = Convert.ToString(row["CreatedOn"]),
        //               BlogContent = Convert.ToString(row["BlogContent"]),
        //               Designation = Convert.ToString(row["Designation"]),
        //               pageUrlText = Convert.ToString(row["pageUrlText"]),
        //               CreatedBy = Convert.ToString(row["CreatedBy"])
        //           }
        //        );
        //    }

        //    return View(datatable);
        //}

        public ActionResult Index()
        {
            //ViewData["IndexInsight"] = BindAllInsight();
            //GetAllBlogsHome();
            BindDropDown();
            Session["txtPageName"] = "Index";
            return ServicesQuery();
        }
        [Route("industries")]
        public ActionResult Industries()
        {
            return View();
        }
       
        [Route("contact-us")]
        public ActionResult ContactUs()
        {
            BindDropDown();
            Session["txtPageName"] = "ContactUs";
            return ServicesQuery();
        }
        [Route("thankyou")]
        public ActionResult ThankYou()
        {
            return View();
        }
        [Route("thankyouAudit")]
        public ActionResult ThankYouAudit()
        {
            return View();
        }
        [Route("application-submitted")]
        public ActionResult ThankyouCareer()
        {
            return View();
        }
        [Route("TermsandConditions")]
        public ActionResult TermsandConditions()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ServicesQuery(string LeadDataType, string AutoId, string CompanyName, string TradeLicenseAuthority, string ConcernPerson, string CountryCodeContact, string ContactNumber, string EmailId, string Service, string AboutAMCA, Validation model, string SubServiceId)
        {
            if (this.IsCaptchaValid(errorText: ""))
            {

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

                var msg = SendMail("cs3@amca.ae", "", "", "Assign Lead to BD", body);
               //var msg = "";
                if(txtPageName.ToString() == "AuditAssurance")
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
                ViewData["IndexInsight"] = BindAllInsight();
                //GetAllBlogsHome();
                BindDropDown();
                ViewBag.ErrorMessage = "Error: captcha is not valid.";
                return View(txtPageName);

            }
        }

        [HttpPost]
        public ActionResult EmailSubscribe(string AutoId, string EmailId, Validation model)
        {

            ServiceModel PL = new ServiceModel();
            PL.OpCode = 21;
            PL.AutoId = Convert.ToInt32(AutoId);
            PL.EmailId = EmailId;
            ServiceModelD.returnTable(PL);
            DataTable dt = PL.dt;
            ModelState.Clear();
            if (PL.dt.Rows.Count == 0)
            {
                PL.OpCode = 6;
                PL.AutoId = Convert.ToInt32(AutoId);
                PL.EmailId = EmailId;
                ServiceModelD.returnTable(PL);

                //Sending Mail
                var body = "";
                body += "<p style='margin-top: 0px'>Dear AMCA Team,</p>";
                body += "<p style='margin-top: 2px'>He wants our daily Newsletter, please save the below Email id in your Newsletter list.</p>";
                body += " <table width='600' border='1' cellpadding='5' cellspacing='0'> " +
                    "<tr> <td> <strong> Email Id </strong></td> <td>" + EmailId + " </td></tr> " +
                    "</table>";
                var msg = "";
                //var msg = new clsgeneral().SendMail("notification@amca.ae", "4J7UwO5p2VDzG8Nq", "crm@amcaauditing.com", "editor@amcaauditing.com", "", "New Subscriber", body, "smtp-relay.sendinblue.com", 587, true);
                return Json(new { isok = true, message = "Thank you! We will back soon." });
            }
            else
            {
                return Json(new { isok = false, message = EmailId + " :Already exist." });
            }
        }
        public JsonResult GetSubServices(int Id)
        {
            string jsondata = "";
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 25;
            PL.AutoId = Id;
            ServiceModelD.returnTable(PL);
            jsondata = JSONfromDT(PL.dt);
            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }
        public static string JSONfromDT(DataTable dataTable)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dataTable.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dataTable.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
                return serializer.Serialize(rows);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }


    }
}