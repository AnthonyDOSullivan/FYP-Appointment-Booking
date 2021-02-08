using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_Appointment_Booking.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
        public string Email { get; set; }

        public string Address { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public List<Appointment>? Appointments { get; set; }

        //About to try to have user ID related to Doctors and Patients (commiting in case anything goes wrong)

        

    }
}
