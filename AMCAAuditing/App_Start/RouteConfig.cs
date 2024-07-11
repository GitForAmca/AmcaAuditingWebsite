using AMCAAuditing.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AMCAAuditing
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();

            Insight ins = new Insight();
            DataSet ds = ins.GetBlogUrl(null);
            int inc = 0;
            var count = ds.Tables[0].Rows.Count;
            string[] routelist = new string[count];
            for (int i = 0; i < count; i++)
            {
                routelist[i] = ds.Tables[0].Rows[i]["pageUrlText"].ToString();
            }
            foreach (var route in routelist)
            {
                routes.MapRoute(
                  name: "Custom" + inc,
                  url: route,
                  defaults: new { controller = "Insight", action = "InsightView", blogID = 0, blogTitle = route }
                );
                inc++;
            }

            DataSet dsJob = ins.GetJobUrl(null);
            inc = 0;
            count = dsJob.Tables[0].Rows.Count;
            routelist = new string[count];
            for (int i = 0; i < count; i++)
            {
                routelist[i] = dsJob.Tables[0].Rows[i]["PageURL"].ToString();
            }
            foreach (var route in routelist)
            {
                routes.MapRoute(
                  name: "CustomJob" + inc,
                  url: route,
                  defaults: new { controller = "Career", action = "JobDescription", JobID = 0, JobTitle = route }
                );
                inc++;
            }

            routes.MapMvcAttributeRoutes();
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Pages", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
