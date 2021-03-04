using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FYP_Appointment_Booking.Data;
using FYP_Appointment_Booking.Models;

namespace FYP_Appointment_Booking.Controllers
{
    public class AdminAppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminAppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AdminAppointments
        /* public async Task<IActionResult> Index()
         {
             var applicationDbContext = _context.Appointments.Include(a => a.Doctor).Include(a => a.Patient).Include(a => a.User);
             return View(await applicationDbContext.ToListAsync());
         }
        */
        //https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/search?view=aspnetcore-5.0
        public async Task<IActionResult> Index(string searchString)
        {
            var applicationDbContext = _context.Appointments.Include(a => a.Doctor).Include(a => a.Patient).Include(a => a.User);
            return View(await applicationDbContext.ToListAsync());
            var appointments = from a in _context.Appointments
                         select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                appointments = appointments.Where(a => a.Location.Contains(searchString));
            }

            return View(await appointments.ToListAsync());
        }

        // GET: AdminAppointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: AdminAppointments/Create
        public IActionResult Create()
        {
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "DoctorId");
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            return View();
        }

        // POST: AdminAppointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Location,Details,Confirmed,DoctorId,PatientId,UserId")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "DoctorId", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", appointment.PatientId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", appointment.UserId);
            return View(appointment);
        }

        // GET: AdminAppointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "DoctorId", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", appointment.PatientId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", appointment.UserId);
            return View(appointment);
        }

        // POST: AdminAppointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Location,Details,Confirmed,DoctorId,PatientId,UserId")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "DoctorId", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", appointment.PatientId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", appointment.UserId);
            return View(appointment);
        }

        // GET: AdminAppointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: AdminAppointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}
