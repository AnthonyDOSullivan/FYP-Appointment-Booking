using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_Appointment_Booking.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }
        [PersonalData]
        public DateTime DOB { get; set; }
        


        public int? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public int? PatientId { get; set; }

        public Patient? Patient { get; set; }

    }
}
