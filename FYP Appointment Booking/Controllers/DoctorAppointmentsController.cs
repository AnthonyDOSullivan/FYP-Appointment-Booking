using System;
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
        public async Task<IActionResult> Index()
        {
            //Review this code post meeting w/ Andrea 
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //UserId of currently logged in user
            var Usr = await _context.Users.Where(u => u.Id == userId).FirstAsync();
            var applicationDbContext = _context.Appointments.Where(a => a.DoctorId == Usr.DoctorId).Include(a => a.Doctor).Include(a => a.Patient).Include(a => a.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Appointments/Details/5
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

            //This will need to be fixed so that admin staff can load appointments for users with different IDs to theirs 
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //UserId of currently logged in user

            if (appointment.UserId != userId)
            {
                return View("PrivacyError");

            }
            return View(appointment);
        }
        // GET: Appointments/Create
        public IActionResult Create()
        {
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "DoctorId");
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.ApllicationUsers, "Id", "Id");
            return View();
        }

        // POST: Appointments/Create
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
            ViewData["UserId"] = new SelectList(_context.ApllicationUsers, "Id", "Id", appointment.UserId);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
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
            ViewData["UserId"] = new SelectList(_context.ApllicationUsers, "Id", "Id", appointment.UserId);
            return View(appointment);

            //This will need to be fixed so that admin staff can load appointments for users with different IDs to theirs 
            // var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //UserId of currently logged in user
            //Remove the return line when adding the var UserId line

            /*  if (appointment.UserId != userId)
              {
                  return View("PrivacyError");

              }
              return View(appointment);  - Or is this what is messing it up ?*/
        }

        // POST: Appointments/Edit/5
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
            ViewData["UserId"] = new SelectList(_context.ApllicationUsers, "Id", "Id", appointment.UserId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        [Authorize]
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
            /*This will need to be fixed so that admin staff can load appointments for users with different IDs to theirs 
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //UserId of currently logged in user

            if (appointment.UserId != userId)
            {
                return View("PrivacyError");

            } Again is this what is messing it up? Replace with Patient ID*/

            return View(appointment);
        }

        // POST: Appointments/Delete/5
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
