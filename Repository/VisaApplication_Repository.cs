using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.IO;

namespace FinalProject.Repository
{
    public class VisaApplication_Repository
    {
        String conString = ConfigurationManager.ConnectionStrings["Myconnection"].ToString();



        ///Get All Users <summary>
        /// Get All Users
        /// </summary>
        /// <returns></returns>
        public List<VisaApplication> GetAllApplicants()
        {
            List<VisaApplication> ApplicantList = new List<VisaApplication>();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SPS_GetAllVisaApplications";
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtUsers = new DataTable();

                connection.Open();
                sqlDA.Fill(dtUsers);
                connection.Close();

                foreach (DataRow datarow in dtUsers.Rows)
                {
                   
                    ApplicantList.Add(new VisaApplication
                    {
                        ApplicationID = Convert.ToInt32(datarow["ApplicationID"]),
                        FirstName = datarow["FirstName"].ToString(),
                        LastName = datarow["LastName"].ToString(),
                        DateOfBirth = Convert.ToDateTime(datarow["DateOfBirth"]),
                        EmailID = datarow["EmailID"].ToString(),
                        Phone = datarow["Phone"].ToString(),
                        Address = datarow["Address"].ToString(),
                        ExpectedDateOfArrival = Convert.ToDateTime(datarow["ExpectedDateOfArrival"]),
                        ExpectedDateOfDeparture = Convert.ToDateTime(datarow["ExpectedDateOfDeparture"]),
                        VisaService = datarow["VisaService"].ToString(),
                        Gender = datarow["Gender"].ToString(),
                        TownCityOfBirth = datarow["TownCityOfBirth"].ToString(),
                        CountryOfBirth = datarow["CountryOfBirth"].ToString(),
                        CitizenshipNationalIdNo = datarow["CitizenshipNationalIdNo"].ToString(),
                        Religion = datarow["Religion"].ToString(),
                        EducationalQualification = datarow["EducationalQualification"].ToString(),
                        PassportType = datarow["PassportType"].ToString(),
                        Nationality = datarow["Nationality"].ToString(),
                        PassportNumber = datarow["PassportNumber"].ToString(),
                        PlaceOfIssue = datarow["PlaceOfIssue"].ToString(),
                        DateOfIssue = Convert.ToDateTime(datarow["DateOfIssue"]),
                        DateOfExpiry = Convert.ToDateTime(datarow["DateOfExpiry"]),
                        PassportOrICNo = datarow["PassportOrICNo"].ToString(),
                        PortOfArrival = datarow["PortOfArrival"].ToString(),
                        ReferenceNameInIndia = datarow["ReferenceNameInIndia"].ToString(),
                        ReferenceAddressInIndia = datarow["ReferenceAddressInIndia"].ToString(),
                        ReferencePhone = datarow["ReferencePhone"].ToString(),
                        
                        Status = datarow["Status"].ToString()
                    });
                }
            }
            return ApplicantList;
        }


        public void UpdateStatus(int applicationId, string status)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SPU_UpdateVisaApplicationStatus";
                command.Parameters.AddWithValue("@ApplicationID", applicationId);
                command.Parameters.AddWithValue("@Status", status);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }



        ///Get All Userss by ID <summary>
        /// Get All Userss by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<VisaApplication> GetApplicantsByID(int ApplicationID)
        {
            List<VisaApplication> ApplicantList = new List<VisaApplication>();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SPS_GetVisaApplicationByID";
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtVisaApplications = new DataTable();

                connection.Open();
                sqlDA.Fill(dtVisaApplications);
                connection.Close();

                foreach (DataRow datarow in dtVisaApplications.Rows)
                {
                    ApplicantList.Add(new VisaApplication
                    {
                        ApplicationID = Convert.ToInt32(datarow["ApplicationID"]),
                        FirstName = datarow["FirstName"].ToString(),
                        LastName = datarow["LastName"].ToString(),
                        DateOfBirth = Convert.ToDateTime(datarow["DateOfBirth"]),
                        EmailID = datarow["EmailID"].ToString(),
                        Phone = datarow["Phone"].ToString(),
                        Address = datarow["Address"].ToString(),
                        ExpectedDateOfArrival = Convert.ToDateTime(datarow["ExpectedDateOfArrival"]),
                        ExpectedDateOfDeparture = Convert.ToDateTime(datarow["ExpectedDateOfDeparture"]),
                        VisaService = datarow["VisaService"].ToString(),
                        Gender = datarow["Gender"].ToString(),
                        TownCityOfBirth = datarow["TownCityOfBirth"].ToString(),
                        CountryOfBirth = datarow["CountryOfBirth"].ToString(),
                        CitizenshipNationalIdNo = datarow["CitizenshipNationalIdNo"].ToString(),
                        Religion = datarow["Religion"].ToString(),
                        EducationalQualification = datarow["EducationalQualification"].ToString(),
                        PassportType = datarow["PassportType"].ToString(),
                        Nationality = datarow["Nationality"].ToString(),
                        PassportNumber = datarow["PassportNumber"].ToString(),
                        PlaceOfIssue = datarow["PlaceOfIssue"].ToString(),
                        DateOfIssue = Convert.ToDateTime(datarow["DateOfIssue"]),
                        DateOfExpiry = Convert.ToDateTime(datarow["DateOfExpiry"]),
                        PassportOrICNo = datarow["PassportOrICNo"].ToString(),
                        PortOfArrival = datarow["PortOfArrival"].ToString(),
                        ReferenceNameInIndia = datarow["ReferenceNameInIndia"].ToString(),
                        ReferenceAddressInIndia = datarow["ReferenceAddressInIndia"].ToString(),
                        ReferencePhone = datarow["ReferencePhone"].ToString(),
                        
                        Status = datarow["Status"].ToString()
                    });
                }
            }
            return ApplicantList;
        }




        ///Delete Product <summary>
        /// Delete Product
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public string DeleteApplicants(int applicationId)
        {
            string result = "";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("SPS_DeleteVisaApplication", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ApplicationID", applicationId);
                command.Parameters.Add("@OUTPUTMESSAGE", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;

                connection.Open();
                command.ExecuteNonQuery();
                result = command.Parameters["@OUTPUTMESSAGE"].Value.ToString();
                connection.Close();
            }
            return result;
        }
    }
}