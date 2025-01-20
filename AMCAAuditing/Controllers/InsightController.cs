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
    public class InsightController : Controller
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
        // GET: Insight
        public InsightModel GetBlogsContent(int? blogID, string blogTitle)
            {
                InsightModel insmodel = new InsightModel();
                Insight ins = new Insight();
                DataSet ds;
                if (blogTitle != null)
                {
                    ds = ins.GetBlogsByID(null, blogTitle);
                }
                else
                {
                    ds = ins.GetBlogsByID(blogID);
                }
                DataTable dt = ds.Tables[0];
                insmodel.BlogTitle = dt.Rows[0]["BlogTitle"].ToString();
                insmodel.PageTitle = dt.Rows[0]["PageTitle"].ToString();
                insmodel.Keyword = dt.Rows[0]["Keyword"].ToString();
                insmodel.Description = dt.Rows[0]["Description"].ToString();
                insmodel.ImageURL = dt.Rows[0]["ImageURL"].ToString();
                insmodel.IsActive = dt.Rows[0]["IsActive"].ToString();
                insmodel.CreatedOn = dt.Rows[0]["CreatedOn"].ToString();
                insmodel.BlogContent = dt.Rows[0]["BlogContent"].ToString();
                insmodel.Designation = dt.Rows[0]["Designation"].ToString();
                insmodel.CreatedBy = dt.Rows[0]["CreatedBy"].ToString();
                insmodel.pageUrlText = dt.Rows[0]["pageUrlText"].ToString();

                DataTable dt2 = ds.Tables[1];
                List<InsightModel> datatable = new List<InsightModel>();

                foreach (DataRow row in dt2.Rows)
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
                insmodel.listData = datatable;
                return insmodel;
            }
            //public ActionResult InsightView(int? blogID, string blogTitle)
                //{
                //    InsightModel insmodel = new InsightModel();
                //        Session["txtPageName"] = blogTitle;
                //        Session["BlogId"] = blogID;
                //        insmodel = GetBlogsContent(blogID, blogTitle);
                //        BindDropDown();
                //        return View(insmodel);
            //}
            public ActionResult InsightView(int? blogID, string blogTitle)
            {
                    Session["txtPageName"] = blogTitle;
                    Session["BlogId"] = blogID;
                    BindDropDown();
                    return ServicesQuery();
            }
            [HttpPost]
            public string getdata(int? blogID)
            {
                BindDropDown();
                Insight ins = new Insight();
                DataSet ds = ins.GetBlogUrl(blogID);
                string pageUrlText = ds.Tables[0].Rows[0]["pageUrlText"].ToString();
                Session["txtPageName"] = pageUrlText;
                Session["BlogId"] = blogID;
                return pageUrlText;
            }


            //Add for enquiry form
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
                PL.OpCode = 64;
                ServiceModelD.returnTable(PL);
                ViewBag.CommonServices = ToSelectList(PL.dt, "Id", "Name");
            }
            [HttpPost]
            public ActionResult ServicesQuery(string LeadDataType, string AutoId, string CompanyName, string TradeLicenseAuthority, string ConcernPerson, string CountryCodeContact, string ContactNumber, string EmailId, string Service, string AboutAMCA, Validation model, string SubServiceId)
            {
                if (this.IsCaptchaValid(errorText: ""))
                {

                    ServiceModel PL = new ServiceModel();
                    PL.OpCode = 5;
                    LeadDataType = "Entity";
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


                    var msg = SendMail("crm@amca.ae", "", "", "Assign Lead to BD", body);
                    //var msg = ""; 
                    return RedirectToAction("Thankyou", "Pages");

                }
                else
                {

                    BindDropDown();
                    ServicesQuery();
                    ViewBag.ErrorMessage = "Error: captcha is not valid.";
                    return View("InsightView");

                }
            }
            [HttpGet]
            public ActionResult ServicesQuery()
            {
                var txtPageName = Session["txtPageName"].ToString();
                int blogId = Convert.ToInt32(Session["BlogId"].ToString());
                InsightModel insmodel = new InsightModel();
                insmodel = GetBlogsContent(blogId, txtPageName);
                BindDropDown();
                return View(insmodel);
            }

        }
    
}