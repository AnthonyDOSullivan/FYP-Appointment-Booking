﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_Appointment_Booking.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>  
    /// Email notification view model class.  
    /// </summary>  
    public class EmailNotifyViewModel
    {
        #region Properties  

        /// <summary>  
        /// Gets or sets to email address.  
        /// </summary>  
        [Required]
        [Display(Name = "To (Email Address)")]
        public string ToEmail { get; set; }

        #endregion
    }
   
}
