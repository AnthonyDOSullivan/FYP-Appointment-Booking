﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FYP_Appointment_Booking.Data;
using FYP_Appointment_Booking.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace FYP_Appointment_Booking.Controllers
{
    public class DoctorAppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorAppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DoctorAppointments
        public async Task<IActionResult> Index(string searchString)
        {
            //Review this code post meeting w/ Andrea 
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //UserId of currently logged in user
            var Usr = await _context.Users.Where(u => u.Id == userId).FirstAsync();
            var applicationDbContext = _context.Appointments.Where(a => a.DoctorId == Usr.DoctorId).Include(a => a.Doctor).Include(a => a.Patient).Include(a => a.User);
            // return View(await applicationDbContext.ToListAsync());
            //https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/search?view=aspnetcore-5.0


            var appointments = from a in _context.Appointments.Where(a => a.DoctorId == Usr.DoctorId)
                                   select a;

                if (!String.IsNullOrEmpty(searchString))
                {
              
                appointments = appointments.Where(a => a.Location.Contains(searchString)).Where(a => a.DoctorId == Usr.DoctorId).Include(a => a.Doctor).Include(a => a.Patient).Include(a => a.User);
            }
        
            appointments = appointments.OrderBy(a => a.Date);


            return View(await applicationDbContext.ToListAsync());
        }
        

        
        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}
