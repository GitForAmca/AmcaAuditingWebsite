using AMCAAuditing.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using CaptchaMvc.HtmlHelpers;
using System.Net;

namespace AMCAAuditing.Controllers
{
    public class ExperiencedCareerRolesController : Controller
    {
        // GET: ExperiencedCareerRoles
        public ActionResult Index()
        {
            return View();
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
        [HttpPost]
        public ActionResult SubmitJob(ExperienceDesignation job)
        {
            TempData["Job"] = job;
            return RedirectToAction("ApplyJobs", "ExperiencedCareerRoles");
        }
        [HttpGet]
        [Route("amca-jobs")]
        public ActionResult ApplyJobs()
        {
            ExperienceDesignation job = TempData["Job"] as ExperienceDesignation;

            //Select About Job
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 12;
            ServiceModelD.returnTable(PL);
            ViewBag.AboutJob = ToSelectList(PL.dt, "Id", "Name");

            //Select Country
            PL.OpCode = 13;
            ServiceModelD.returnTable(PL);
            ViewBag.Nationality = ToSelectList(PL.dt, "Autoid", "CountryName");
            //Select Country

            PL.OpCode = 13;
            ServiceModelD.returnTable(PL);
            ViewBag.CurrentLocation = ToSelectList(PL.dt, "Autoid", "CountryName");

            PL.OpCode = 18;
            ServiceModelD.returnTable(PL);
            ViewBag.JobTitle = ToSelectList(PL.dt, "Id", "JobPosition");

            PL.OpCode = 19;
            ServiceModelD.returnTable(PL);
            ViewBag.CountryCodeContact = ToSelectList(PL.dt, "CountryISDCode", "CountryName");

            return View(job);
        }
        [NonAction]
        public string SendMail(string FromMailID, string fromEmailPassword, string ToMailID, string CC, string BCC, string subject, string body, string servername, int PortNo, bool ssl)
        {
            servername = "smtp-relay.sendinblue.com"; PortNo = 587; ssl = false; FromMailID = "notification@amca.ae"; fromEmailPassword = "4J7UwO5p2VDzG8Nq";
            string msg = string.Empty;
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

        [HttpPost]
        public ActionResult RequestJob(string AboutJob, string JobPosition,string JobTitle, string CandidateName, string EmailId, string MobileNo,
            string Nationality, string DoB, string Gender, string MaritalStatus, string Experience, string UAEExperience,
            string LastDesignation, string CurrentLocation, string SalaryExpectation, string NoticePeriod, string VisaType,
            HttpPostedFileBase PostedFile, ExperienceDesignation model, string CountryCodeContact, string LastSalaryDrawn, string coverLetter,
            HttpPostedFileBase coverLetterFile, string HighestQualification)
        {
            if (this.IsCaptchaValid(errorText: ""))
            {
                var FTPURL = "ftp://amca.ae@45.66.245.92/httpdocs/";
                string _FileName = Path.GetFileNameWithoutExtension(PostedFile.FileName);
                string extension = Path.GetExtension(PostedFile.FileName);
                _FileName = _FileName + DateTime.Now.ToString("yymmddssfff") + extension;
                string relativeFileName = "CandidateCV/" + _FileName;

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FTPURL + "CandidateCV/" + _FileName);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential("amca.ae", "*(&#hdsj^%^23JKN6512");

                using (Stream requestStream = request.GetRequestStream())
                {
                    PostedFile.InputStream.CopyTo(requestStream);
                }
                string CoverrelativeFileName = "";
                if (coverLetterFile != null)
                {
                    if (coverLetterFile.ContentLength > 0)
                    {
                        string _CoverFileName = Path.GetFileNameWithoutExtension(coverLetterFile.FileName);
                        string Coverextension = Path.GetExtension(coverLetterFile.FileName);
                        _CoverFileName = _CoverFileName + DateTime.Now.ToString("yymmddssfff") + Coverextension;
                        CoverrelativeFileName = "CandidateCV/CoverLetter/" + _CoverFileName;

                        FtpWebRequest request2 = (FtpWebRequest)WebRequest.Create(FTPURL + "CandidateCV/CoverLetter/" + _CoverFileName);
                        request2.Method = WebRequestMethods.Ftp.UploadFile;
                        request2.Credentials = new NetworkCredential("amca.ae", "*(&#hdsj^%^23JKN6512");

                        using (Stream requestStream = request2.GetRequestStream())
                        {
                            coverLetterFile.InputStream.CopyTo(requestStream);
                        }
                    }
                }

                Candidate_Data_PL PL = new Candidate_Data_PL();
                PL.OpCode = 1;
                PL.AboutJob = AboutJob;
                PL.AutoId = Convert.ToInt32(JobPosition);
                PL.JobPosition = JobTitle;
                PL.CandidateName = CandidateName;
                PL.EmailId = EmailId;
                PL.MobileNo = MobileNo;
                PL.Nationality = Nationality;
                PL.DoB = DoB;
                PL.Gender = Gender;
                PL.MaritalStatus = MaritalStatus;
                PL.Experience = Experience;
                PL.UAEExperience = UAEExperience;
                PL.LastDesignation = LastDesignation;
                PL.CurrentLocation = CurrentLocation;
                PL.SalaryExpectation = SalaryExpectation;
                PL.NoticePeriod = NoticePeriod;
                PL.VisaType = VisaType;
                PL.CV = relativeFileName;
                PL.CountryCode = CountryCodeContact;
                PL.LastSalary = LastSalaryDrawn;
                PL.coverLetter = coverLetter;
                PL.coverLetterFile = CoverrelativeFileName;
                PL.HighestQualification = HighestQualification;
                Candidate_Data_DL.returnTable(PL);
                if (PL.dt.Rows.Count > 0)
                {
                    string Autoid = PL.dt.Rows[0]["Autoid"].ToString();
                    PL.OpCode = 2;
                    PL.AutoId = Convert.ToInt32(Autoid);
                    Candidate_Data_DL.returnTable(PL);

                    /// Select Candidate Id
                    PL.OpCode = 3;
                    PL.AutoId = Convert.ToInt32(Autoid);
                    Candidate_Data_DL.returnTable(PL);
                    DataTable dt = PL.dt;
                    TempData["Id"] = PL.dt.Rows[0]["CandidateId"].ToString();
                    TempData["Name"] = PL.dt.Rows[0]["CandidateName"].ToString();
                    TempData["ApplyJob"] = PL.dt.Rows[0]["JobTitle"].ToString();

                    //Sending Mail
                    var mail = "<p>Dear HR,</p>";
                    mail += "<p>Please go thru the link: <a href='https://portal.amca.ae'>AMCA Portal</a> and find the below candidate details.</p>";

                    mail += "Candidate Id: " + PL.dt.Rows[0]["CandidateId"].ToString();
                    mail += "<br>Candidate Name: " + PL.dt.Rows[0]["CandidateName"].ToString();
                    mail += "<br>Job Position: " + PL.dt.Rows[0]["JobTitle"].ToString();
                    mail += "<p>Thank you!</p>";
                    mail += "<p>Regards,<br>AMCA</p>";

                   //var msg = SendMail("notification@amca.ae", "4J7UwO5p2VDzG8Nq", "hr@amcaauditing.com", "careers@amcaauditing.com", "", "Job Application", mail, "smtp-relay.sendinblue.com", 587, true);
                   var msg = "";
                    return RedirectToAction("ThankyouCareer", "Pages");
                }
            }
            ViewBag.ErrorMessage = "Error: Captcha is not valid";
            ApplyJobs();
            return View("ApplyJobs");
        }
    }
}
