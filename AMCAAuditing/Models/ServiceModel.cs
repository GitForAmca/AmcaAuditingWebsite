using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace AMCAAuditing.Models
{
    public class ServiceModel
    {
        public int OpCode { get; set; }
        public int ServiceID { get; set; }
        public string ServiceType { get; set; }
        public int AutoId { get; set; }
        public string ServiceName { get; set; }
        public string CompanyName { get; set; }
        public string TradeLicenseAuthority { get; set; }
        public object ScriptManager { get; set; }
        public string ConcernPerson { get; set; }
        public string CountryCodeContact { get; set; }
        public string ContactNumber { get; set; }
        public string EmailId { get; set; }
        public string Service { get; set; }
        public string AboutAMCA { get; set; }
        public string MessageContact { get; set; }
        public string exceptionMessage { get; set; }
        public bool isException { get; set; }
        public DataTable dt { get; set; }
        public string txtPageName { get; set; }
        public string LeadDataType { get; set; }
    }
}