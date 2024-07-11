using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMCAAuditing.Controllers
{
    public class CompanyController : Controller
    {
        // GET: Company
        public ActionResult Index()
        {
            return View();
        }
        [Route("profile")]
        public ActionResult CompanyProfile()
        {
            return View();
        }
        [Route("mission-vision")]
        public ActionResult MissionVission()
        {
            return View();
        }
        [Route("corporate-group")]
        public ActionResult CorporateGroup()
        {
            return View();
        }
    }
}