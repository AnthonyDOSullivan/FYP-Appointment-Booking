using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_Appointment_Booking.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        [Display(Name = "Doctor Name")]
        public string Name { get; set; }

        public string Speciality  { get; set; }

        public List<Appointment> Appointments { get; set; }
        public List<Patient> Patients { get; set; }




        //About to try to have user ID related to Doctors and Patients (commiting in case anything goes wrong)

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

    }
}
