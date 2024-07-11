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

namespace AMCAAuditing.Controllers
{
    public class CareerController : Controller
    {
        public ActionResult JobDescription(int? JobID, string JobTitle)
        {
            Candidate_Data_PL jobModel = new Candidate_Data_PL();
            jobModel = GetJobsContent(JobID, JobTitle);
            return View(jobModel);
        }
        public Candidate_Data_PL GetJobsContent(int? JobID, string JobTitle)
        {
            Candidate_Data_PL PL = new Candidate_Data_PL();
            PL = GetAllJobs();
            PL.OpCode = 7;
            PL.PageURL = JobTitle;
            PL.DoB = DateTime.Now;
            Candidate_Data_DL.returnTable(PL);
            List<Candidate_Data_PL> datatable = new List<Candidate_Data_PL>();
            if (PL.dt.Rows.Count > 0)
            {
                PL.JobTitle = Convert.ToString(PL.dt.Rows[0]["JobTitle"]);
                PL.JobType = Convert.ToString(PL.dt.Rows[0]["JobType"]);
                PL.JobContent = Convert.ToString(PL.dt.Rows[0]["JobContent"]);
                PL.JobResponsibility = Convert.ToString(PL.dt.Rows[0]["JobResponsibility"]);
                PL.JobRequirements = Convert.ToString(PL.dt.Rows[0]["JobRequirements"]);
                PL.Keyword = Convert.ToString(PL.dt.Rows[0]["Keyword"]);
                PL.MetaDescription = Convert.ToString(PL.dt.Rows[0]["MetaDescription"]);
                PL.PageURL = Convert.ToString(PL.dt.Rows[0]["PageURL"]);
                PL.PageTitle = Convert.ToString(PL.dt.Rows[0]["PageTitle"]);
                DateTime aDate = (DateTime)PL.dt.Rows[0]["createdOn"];
                PL.createdOn = aDate.ToString("MMMM dd, yyyy");
            }
            foreach (Candidate_Data_PL row in PL.listData)
            {
                if (row.JobTitle.ToString() == PL.JobTitle.ToString())
                {
                    row.setLink = "ActiveSubLink";
                }
            }
            return PL;
        }
        public Candidate_Data_PL GetAllJobs() 
        {
            Candidate_Data_PL PL = new Candidate_Data_PL();
            PL.OpCode = 4;
            PL.DoB = DateTime.Now;
            Candidate_Data_DL.returnTable(PL);
            List<Candidate_Data_PL> datatable = new List<Candidate_Data_PL>();
            foreach (DataRow row in PL.dt.Rows)
            {
                DateTime aDate = (DateTime)row["createdOn"];
                datatable.Add(
                    new Candidate_Data_PL
                    {
                        AutoId = Convert.ToInt32(row["Autoid"]),
                        JobTitle = Convert.ToString(row["JobTitle"]),
                        JobType = Convert.ToString(row["JobType"]),
                        JobContent = Convert.ToString(row["JobContent"]),
                        JobResponsibility = Convert.ToString(row["JobResponsibility"]),
                        JobRequirements = Convert.ToString(row["JobRequirements"]),
                        Keyword = Convert.ToString(row["Keyword"]),
                        MetaDescription = Convert.ToString(row["MetaDescription"]),
                        PageURL = Convert.ToString(row["PageURL"]),
                        PageTitle = Convert.ToString(row["PageTitle"]),
                        createdOn = aDate.ToString("MMMM dd, yyyy"),
                        CountryCode = Convert.ToString(row["Region"]),
                        CurrentLocation = Convert.ToString(row["Location"]),
                        IsActive = Convert.ToString(row["Active"]),
                    }
                );
            }
            Candidate_Data_PL candidateModel = new Candidate_Data_PL();
            candidateModel.listData = datatable;
            return candidateModel;
        }

        [Route("experienced-job-search")]
        public ActionResult ListAllJobs()
        {
            Candidate_Data_PL candidateModel = new Candidate_Data_PL();
            candidateModel = GetAllJobs();
            return View(candidateModel);
        }
        [Route("internships-jobs-in-dubai")]
        public ActionResult StudentAndCampusRole()
        {
            Candidate_Data_PL candidateModel = new Candidate_Data_PL();
            candidateModel = GetAllJobs();
            return View(candidateModel);
        }

        [HttpPost]
        public string getdata(int? JobID)
        {
            Insight ins = new Insight();
            Candidate_Data_PL PL = new Candidate_Data_PL();
            PL.OpCode = 6;
            PL.DoB = DateTime.Now;
            if (JobID != null)
            {
                PL.AutoId = (int)JobID;
            }
            Candidate_Data_DL.returnTable(PL);
            //DataSet ds = ins.GetJobUrl(JobID);
            string pageUrlText = PL.dt.Rows[0]["pageUrl"].ToString();
            return pageUrlText;
        }
    }
}