using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AMCAAuditing.Models
{
    public class Candidate_Data_PL
    {
        public int OpCode { get; set; }
        public int AutoId { get; set; }
        public object AboutJob { get; set; }
        public object JobTitle { get; set; }
        public object JobPosition { get; set; }
        public object CandidateName { get; set; }
        public object EmailId { get; set; }
        public object MobileNo { get; set; }
        public object Nationality { get; set; }
        public object DoB { get; set; }
        public object Gender { get; set; }
        public object MaritalStatus { get; set; }
        public object Experience { get; set; }
        public object UAEExperience { get; set; }
        public object LastDesignation { get; set; }
        public object CurrentLocation { get; set; }
        public object SalaryExpectation { get; set; }
        public object NoticePeriod { get; set; }
        public object VisaType { get; set; }
        public string CV { get; set; }
        public string coverLetterFile { get; set; }
        public object exceptionMessage { get; set; }
        public bool isException { get; set; }
        public DataTable dt { get; set; }
        public string CountryCode { get; set; }
        public string LastSalary { get; set; }
        public string coverLetter { get; set; }
        public string HighestQualification { get; set; }
        public string JobType { get; set; }
        public string JobContent { get; set; }
        public string JobResponsibility { get; set; }
        public string JobRequirements { get; set; }
        public string MetaDescription { get; set; }
        public string PageURL { get; set; }
        public string PageTitle { get; set; }
        public string Keyword { get; set; }
        public string createdOn { get; set; }
        public string setLink { get; set; }
        public object IsActive { get; set; }
        public List<Candidate_Data_PL> listData { get; set; }
    }
}