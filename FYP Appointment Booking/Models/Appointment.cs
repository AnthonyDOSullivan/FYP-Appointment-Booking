using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FYP_Appointment_Booking.Models
{
    public class Appointment
    {
  


        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string Details { get; set; }
        public bool Confirmed { get; set; }
        public string Email { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public int PatientId { get; set; }

        public Patient Patient { get; set; }
    }
}