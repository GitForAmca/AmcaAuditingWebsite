using AMCAAuditing.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AMCAAuditing.BusinessLogic
{
    public class Insight
    {
        public DataSet GetBlogsByID(int? BlogID = null, string BlogTitle = null, string ServiceModels = null)
        {
            SQLConnectivity SC = new SQLConnectivity();
            SqlCommand sqlCmd = new SqlCommand("dbo.BlogData_sl", SC.SqlCon);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("BlogID", BlogID);
            sqlCmd.Parameters.AddWithValue("BlogTitle", BlogTitle);
            sqlCmd.Parameters.AddWithValue("websiteID", 2);
            sqlCmd.Parameters.AddWithValue("ServiceModels", ServiceModels);
            SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        public DataSet GetAllBlogs()
        {
            SQLConnectivity SC = new SQLConnectivity();
            SqlCommand sqlCmd = new SqlCommand("dbo.BlogData_s", SC.SqlCon);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("websiteID", 2);
            SqlDataAdapter da = new SqlDataAdapter(sqlCmd);

            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        public DataSet GetAllBlogsBySearch(int? Service)
        {
            SQLConnectivity SC = new SQLConnectivity();
            SqlCommand sqlCmd = new SqlCommand("dbo.BlogData_s", SC.SqlCon);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("websiteID", 2);
            sqlCmd.Parameters.AddWithValue("Service", Service);
            SqlDataAdapter da = new SqlDataAdapter(sqlCmd);

            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        public DataSet GetAllBlogsHome(string Page = null)
        {
            SQLConnectivity SC = new SQLConnectivity();
            SqlCommand sqlCmd = new SqlCommand("dbo.BlogData_s", SC.SqlCon);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("websiteID", 2);
            sqlCmd.Parameters.AddWithValue("Page", "Home");
            SqlDataAdapter da = new SqlDataAdapter(sqlCmd);

            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        public DataSet GetBlogUrl(int? BlogID)
        {
            SQLConnectivity SC = new SQLConnectivity();
            SqlCommand sqlCmd = new SqlCommand("dbo.BlogPageUrl_ss", SC.SqlCon);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("BlogID", BlogID);
            sqlCmd.Parameters.AddWithValue("websiteID", 2);
            SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public DataSet GetJobUrl(int? JobID)
        {
            SQLConnectivity SC = new SQLConnectivity();
            SqlCommand sqlCmd = new SqlCommand("dbo.MSP_Candidate_Data", SC.SqlCon);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@Autoid", JobID);
            sqlCmd.Parameters.AddWithValue("@OpCode", 6);
            sqlCmd.Parameters.AddWithValue("@DoB", DateTime.Now);
            sqlCmd.Parameters.AddWithValue("websiteID", 2);
            SqlDataAdapter da = new SqlDataAdapter(sqlCmd);

            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        public List<InsightModel> BindInsight(string ServiceModel)
        {
            DataSet ds = GetBlogsByID(null, null, ServiceModel);
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
                       Designation = Convert.ToString(row["Designation"])
                   }
                );
            }
            return datatable;
        }
    }
}