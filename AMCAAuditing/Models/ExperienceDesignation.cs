using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AMCAAuditing.Models
{
    public class ExperienceDesignation
    {
        public string JobTitle { get; set; }
        [Required(ErrorMessage = "Please select Job Position")]
        public string JobPosition { get; set; }

        public string CandidateId { get; set; }

       [Required(ErrorMessage = "Please select one")]
        public string AboutJob { get; set; }

        [Required(ErrorMessage = "Please Enter your Name")]
        public string CandidateName { get; set; }

        [Required(ErrorMessage = "Choose Your DOB")]
        public string DoB { get; set; }
     

        [Required(ErrorMessage = "Please select one")]
        public string Nationality { get; set; }

       [Required(ErrorMessage = "Please select one")]
        public string Experience { get; set; }

       [Required(ErrorMessage = "Please select one")]
        public string UAEExperience { get; set; }


        public string LastDesignation { get; set; }

       [Required(ErrorMessage = "Please Enter your Location")]
        public string CurrentLocation { get; set; }

        [Required(ErrorMessage = "Please Select your Criteria")]
        public string SalaryExpectation { get; set; }

        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif)$", ErrorMessage = "Only Image files allowed.")]
        [Required(ErrorMessage = "Please Upload CV")]
        public string CV { get; set; }

        public string file { get; set; }

        [Required(ErrorMessage = "Please select file.")]
        //[RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.pdf)$", ErrorMessage = "Only PDF files Allowed")]
        [RegularExpression(@"^.*\.(jpg|JPG|gif|GIF|docx|DOCX|pdf|PDF)$", ErrorMessage = "Only jpg,gif,docx & pdf files are Allowed")]
        public HttpPostedFileBase PostedFile { get; set; }


        [Required(ErrorMessage = "Enter Email")]
        [EmailAddress(ErrorMessage = "Please Enter Valid Email Id")]
        public string EmailId { get; set; }

       [Required(ErrorMessage = "Please Enter Your Mobile No")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Select Your Gender")]
        public string Gender { get; set; }

       [Required(ErrorMessage = "Select Your Notice Period")]
        public string NoticePeriod { get; set; }

        [Required(ErrorMessage = "Select your VISA type")]
        public string VisaType { get; set; }

       [Required(ErrorMessage = "Select Trade License Authority")]
        public string TradeLicenseAuthority { get; set; }

       [Required(ErrorMessage = "Select Marital Status")]
        public string MaritalStatus { get; set; }
       [Required(ErrorMessage = "Select Country Code")]
        public string CountryCodeContact { get; set; }
        public string LastSalaryDrawn { get; set; }
        public string coverLetter { get; set; }
        public string coverLetterFile { get; set; }
        public string HighestQualification { get; set; }
    }
}