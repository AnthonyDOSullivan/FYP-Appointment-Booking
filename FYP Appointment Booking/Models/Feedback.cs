using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_Appointment_Booking.Models
{
    public class Feedback
    {
       public int Id { get; set; }
        [Display(Name = "Please Leave your Feedback Below")]
        public string text { get; set; }
    }
}
