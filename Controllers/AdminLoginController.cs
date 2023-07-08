using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FinalProject.Repository;

namespace FinalProject.Controllers
{
    public class AdminLoginController : Controller
    {
        // GET: AdminLogin
        public ActionResult Index()
        {
            return View();
        }

        // GET: Login
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Index(Admin lc)
        {

            string mainconn = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            SqlCommand sqlcomm = new SqlCommand("sp_AdminLogin", sqlconn);

            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Parameters.AddWithValue("@EmailAddress", lc.EmailAddress);
            sqlcomm.Parameters.AddWithValue("@Password", lc.Password);

            sqlconn.Open();
            SqlDataReader sqr = sqlcomm.ExecuteReader();

            if (sqr.Read())
            {
                FormsAuthentication.SetAuthCookie(lc.EmailAddress, true);
                Session["emailid"] = lc.EmailAddress.ToString();
               return RedirectToAction("AdminHome", "AdminLogin");
               

            }
            else
            {
                ViewData["message"] = "Username & Password are wrong !";
            }
            sqlconn.Close();
            return View();
        }

        

        public ActionResult AdminWelcome()
        {
            string displayimg = (string)Session["emailid"];
            string mainconn = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string sqlquery = "select * from [dbo].[AdminReg] where EmailAddress='" + displayimg + "'";
            sqlconn.Open();
            SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
            sqlcomm.Parameters.AddWithValue("EmailAddress", Session["emailid"].ToString());
            SqlDataReader sdr = sqlcomm.ExecuteReader();

            Admin user = new Admin();
            if (sdr.Read())
            {
                string s = sdr["Photo"].ToString();
                ViewData["Img"] = s;
                TempData["Oldimg"] = s;


                user.FirstName = sdr["FirstName"].ToString();
                user.LastName = sdr["LastName"].ToString();
                user.DateOfBirth = (DateTime)sdr["DateOfBirth"];
                user.Gender = sdr["Gender"].ToString();
                user.PhoneNumber = sdr["PhoneNumber"].ToString();
                user.EmailAddress = sdr["EmailAddress"].ToString();
                user.Address = sdr["Address"].ToString();
                user.Country = sdr["Country"].ToString();
                user.State = sdr["State"].ToString();
                user.City = sdr["City"].ToString();
                user.Postcode = sdr["Postcode"].ToString();
                user.PassportNumber = sdr["PassportNumber"].ToString();
                user.AdharNumber = sdr["AdharNumber"].ToString();
                user.Username = sdr["Username"].ToString();
                user.Password = sdr["Password"].ToString();

            }
            sqlconn.Close();
            return View(user);
        }
        public ActionResult Adminimgchange(HttpPostedFileBase file)
        {
            var emailId = (string)Session["emailid"];

            string imgpath = Server.MapPath((string)TempData["Oldimg"]);
            string fileimgpath = imgpath;
            FileInfo fi = new FileInfo(fileimgpath);
            if (fi.Exists)
            {
                fi.Delete();
            }

            if (file != null && file.ContentLength > 0)
            {
                string filename = Path.GetFileName(file.FileName);
                string filepath = Path.Combine(Server.MapPath("/Admin-Images/"), filename);
                file.SaveAs(filepath);

                string mainconn = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
                using (SqlConnection sqlconn = new SqlConnection(mainconn))
                {
                    sqlconn.Open();
                    string sqlquery = "UPDATE [dbo].[AdminReg] SET  Photo = @Photo WHERE EmailAddress = @EmailAddress";
                    SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
                    sqlcomm.Parameters.AddWithValue("@Photo", "/Admin-Images/" + filename);
                    sqlcomm.Parameters.AddWithValue("@EmailAddress", emailId);
                    sqlcomm.ExecuteNonQuery();

                }
            }

            return RedirectToAction("AdminWelcome", "AdminLogin");
        }

        //to view user 

        User_Repository usersDAL = new User_Repository();
        // GET: Product
        public ActionResult UsersView()
        {
            var UsersList = usersDAL.GetAllUsers();

            if (UsersList.Count == 0)
            {
                TempData["InfoMessage"] = "Currently Users not available in the Database";
            }
            return View(UsersList);
        }


        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var user = usersDAL.GetUsersByID(id).FirstOrDefault();

                if (user == null)
                {
                    TempData["InfoMessage"] = "Product not available with ID " + id.ToString();
                    return RedirectToAction("UsersView");
                }
                return View(user);
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }


        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var product = usersDAL.GetUsersByID(id).FirstOrDefault();

                if (product == null)
                {
                    TempData["InfoMessage"] = "User not available with ID " + id.ToString();
                    return RedirectToAction("UsersView");
                }
                return View(product);
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmation(int id, FormCollection collection)
        {

            try
            {
                string result = usersDAL.DeleteUser(id);

                if (result.Contains("deleted"))
                {
                    TempData["SuccessMessage"] = result;

                }
                else
                {
                    TempData["ErrorMessage"] = result;

                }
                return RedirectToAction("UsersView");
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }

        }

        public ActionResult AdminHome()
        {
            ViewBag.Message = "Admin Home page";

            return View();
        }


        //to view user 

        VisaApplication_Repository visadata = new VisaApplication_Repository();
        // GET: Product
        public ActionResult ApplicantsView()
        {
            var ApplicantList = visadata.GetAllApplicants();

            if (ApplicantList.Count == 0)
            {
                TempData["InfoMessage"] = "Currently Applicants not available in the Database";
            }
            return View(ApplicantList);
        }


        public ActionResult ChangeStatus(int id, string status)
        {
            // Perform necessary logic to update the status of the visa application with the provided ID

            
            if (status == "approve")
            {
                // Update the status to "Approved" in the database
                visadata.UpdateStatus(id, "Approved");
                TempData["SuccessMessage"] = "Application has been approved successfully.";
            }
            else if (status == "reject")
            {
                // Update the status to "Rejected" in the database
                visadata.UpdateStatus(id, "Rejected");
                TempData["SuccessMessage"] = "Application has been rejected.";
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid status specified.";
            }

            // Redirect back to the ApplicantsView page
            return RedirectToAction("ApplicantsView");
        }

        // GET: Product/Details/5
        public ActionResult ApplicantDetails(int id)
        {
            try
            {
                var applicant = visadata.GetApplicantsByID(id).FirstOrDefault();

                if (applicant == null)
                {
                    TempData["InfoMessage"] = "Applicants not available with ID " + id.ToString();
                    return RedirectToAction("ApplicantsView");
                }
                return View(applicant);
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }


        // GET: Product/Delete/5
        public ActionResult DeleteApplicant(int id)
        {
            try
            {
                
                var applicant = visadata.GetApplicantsByID(id).FirstOrDefault();

                if (applicant == null)
                {
                    TempData["InfoMessage"] = "Applicant is not available with ID " + id.ToString();
                    return RedirectToAction("ApplicantsView");
                }
                return View(applicant);
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult ApplicantDeleteConfirmation(int id, FormCollection collection)
        {

            try
            {
                string result = visadata.DeleteApplicants(id);

                if (result.Contains("deleted"))
                {
                    TempData["SuccessMessage"] = result;

                }
                else
                {
                    TempData["ErrorMessage"] = result;

                }
                return RedirectToAction("ApplicantsView");
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }

        }

    }
}