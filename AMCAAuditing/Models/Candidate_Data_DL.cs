using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace AMCAAuditing.Models
{
    public class Candidate_Data_DL
    {
        public static void returnTable(Candidate_Data_PL PL)
        {
            try
            {
                SQLConnectivity SC = new SQLConnectivity();
                SqlCommand sqlCmd = new SqlCommand("MSP_Candidate_Data", SC.SqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.Add("@AutoId", SqlDbType.Int).Value = PL.AutoId;
                sqlCmd.Parameters.Add("@OpCode", SqlDbType.Int).Value = PL.OpCode;
                sqlCmd.Parameters.Add("@AboutJob", SqlDbType.VarChar).Value = PL.AboutJob;
                sqlCmd.Parameters.Add("@JobTitle", SqlDbType.VarChar).Value = PL.JobPosition;
                sqlCmd.Parameters.Add("@CandidateName", SqlDbType.VarChar).Value = PL.CandidateName;
                sqlCmd.Parameters.Add("@EmailId", SqlDbType.VarChar).Value = PL.EmailId;
                sqlCmd.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = PL.MobileNo;
                sqlCmd.Parameters.Add("@Nationality", SqlDbType.VarChar).Value = PL.Nationality;
                sqlCmd.Parameters.Add("@DoB", SqlDbType.Date).Value = PL.DoB;
                sqlCmd.Parameters.Add("@Gender", SqlDbType.VarChar).Value = PL.Gender;
                sqlCmd.Parameters.Add("@MaritalStatus", SqlDbType.VarChar).Value = PL.MaritalStatus;
                sqlCmd.Parameters.Add("@Experience", SqlDbType.VarChar).Value = PL.Experience;
                sqlCmd.Parameters.Add("@UAEExperience", SqlDbType.VarChar).Value = PL.UAEExperience;
                sqlCmd.Parameters.Add("@LastDesignation", SqlDbType.VarChar).Value = PL.LastDesignation;
                sqlCmd.Parameters.Add("@CurrentLocation", SqlDbType.VarChar).Value = PL.CurrentLocation;
                sqlCmd.Parameters.Add("@SalaryExpectation", SqlDbType.VarChar).Value = PL.SalaryExpectation;
                sqlCmd.Parameters.Add("@NoticePeriod", SqlDbType.VarChar).Value = PL.NoticePeriod;
                sqlCmd.Parameters.Add("@VisaType", SqlDbType.VarChar).Value = PL.VisaType;
                sqlCmd.Parameters.AddWithValue("@CV", PL.CV);
                sqlCmd.Parameters.AddWithValue("@CountryCode", PL.CountryCode);
                sqlCmd.Parameters.AddWithValue("@LastSalaryDrawn", PL.LastSalary);
                sqlCmd.Parameters.AddWithValue("@coverLetterText", PL.coverLetter);
                sqlCmd.Parameters.AddWithValue("@UploadedCoverLetter", PL.coverLetterFile);
                sqlCmd.Parameters.AddWithValue("@HighestQualification", PL.HighestQualification);
                sqlCmd.Parameters.AddWithValue("@PageURL", PL.PageURL);
                sqlCmd.Parameters.AddWithValue("websiteID", 2);
                SqlDataAdapter sqlAdp = new SqlDataAdapter(sqlCmd);
                PL.dt = new DataTable();
                sqlAdp.Fill(PL.dt);
            }
            catch (Exception ex)
            {

            }
        }
    }
}