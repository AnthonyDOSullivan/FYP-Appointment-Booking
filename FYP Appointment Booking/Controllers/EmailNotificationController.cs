using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web;
//using System.Web.Optimization;
using FYP_Appointment_Booking.Models;
using System.Net.Mail;
// using System.Resources.Constants;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace FYP_Appointment_Booking.Controllers
{



    // using System.Web.UI;
    // using System.Web.UI.WebControls;

    /// <summary>  
    /// Email notification controller class.  
    /// </summary>  
    [Authorize(Policy = "writepolicy")]
    public class EmailNotification : Controller
    {
        #region Index view method.  

        #region Get: /EmailNotification/Index method.  

        /// <summary>  
        /// Get: /EmailNotification/Index method.  
        /// </summary>          
        /// <returns>Return index view</returns>  
        public ActionResult Index()
        {
            try
            {
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }

            // Info.  
            return this.View();
        }

        #endregion

        #region POST: /EmailNotification/Index  

        /// <summary>  
        /// POST: /EmailNotification/Index  
        /// </summary>  
        /// <param name="model">Model parameter</param>  
        /// <returns>Return - Response information</returns>  
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(EmailNotifyViewModel model)
        {
            try
            {
                // Verification  
                if (ModelState.IsValid)
                {
                    // Initialization.  
                    string emailMsg = "Dear " + model.ToEmail + ", <br /><br />This is a test <b style='color: red'> Reminder </b><br />  <a href = 'https://localhost:44398/Appointments'> Confirm </a><br /> " + "<a href = 'https://localhost:44398/Appointments'>Reschedule</a> <br />" + "<br />Kind Regards, <br />CUH Appointments";
                    string emailSubject = EmailInfo.EMAIL_SUBJECT_DEFAULT + "Appointment";

                    // Sending Email.  
                    await this.SendEmailAsync(model.ToEmail, emailMsg, emailSubject);


                    // Info.  
                    //return this.Json(new { EnableSuccess = true, SuccessTitle = "Success", SuccessMsg = "Notification has been sent successfully! to '" + model.ToEmail + "' Check your email." });
                    //return RedirectToAction("Success");
                    return View("Success");
                
                }
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);

                // Info  
                return this.Json(new { EnableError = true, ErrorTitle = "Error", ErrorMsg = ex.Message });
            }

            // Info  
            return this.Json(new { EnableError = true, ErrorTitle = "Error", ErrorMsg = "Something goes wrong, please try again later" });
        }

        #endregion

        #endregion

        #region Helper  

        #region Send Email method.  

        /// <summary>  
        ///  Send Email method.  
        /// </summary>  
        /// <param name="email">Email parameter</param>  
        /// <param name="msg">Message parameter</param>  
        /// <param name="subject">Subject parameter</param>  
        /// <returns>Return await task</returns>  
        public async Task<bool> SendEmailAsync(string email, string msg, string subject = "")
        {
            // Initialization.  
            bool isSend = false;

            try
            {
                // Initialization.  
                var body = msg;
                var message = new MailMessage();

                // Settings.  
                message.To.Add(new MailAddress(email));
                message.From = new MailAddress(EmailInfo.FROM_EMAIL_ACCOUNT);
                message.Subject = !string.IsNullOrEmpty(subject) ? subject : EmailInfo.EMAIL_SUBJECT_DEFAULT;
                message.Body = body;
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    // Settings.  
                    var credential = new NetworkCredential
                    {
                        UserName = EmailInfo.FROM_EMAIL_ACCOUNT,
                        Password = EmailInfo.FROM_EMAIL_PASSWORD
                    };

                    // Settings.  
                    smtp.Credentials = credential;
                    smtp.Host = EmailInfo.SMTP_HOST_GMAIL;
                    smtp.Port = Convert.ToInt32(EmailInfo.SMTP_PORT_GMAIL);
                    smtp.EnableSsl = true;

                    // Sending  
                    await smtp.SendMailAsync(message);

                    // Settings.  
                    isSend = true;
                }
            }
            catch (Exception ex)
            {
                // Info  
                throw ex;
            }

            // info.  
            return isSend;
        }

        #endregion

        #endregion
    }
}
