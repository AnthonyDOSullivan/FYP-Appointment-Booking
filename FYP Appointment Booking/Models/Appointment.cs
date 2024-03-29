﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FYP_Appointment_Booking.Models
{
    public class Appointment
    {
  


        public int Id { get; set; }

        [DataType(DataType.Date)]

        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }




        public string Location { get; set; }
        public string Details { get; set; }
        public bool Confirmed { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int? DoctorId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Doctor? Doctor { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int? PatientId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Patient? Patient { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string? UserId { get; set; }
        [HiddenInput(DisplayValue = false)]
        public ApplicationUser? User { get; set; }
    }
}