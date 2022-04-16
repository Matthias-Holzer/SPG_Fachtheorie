using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe2;
using SPG_Fachtheorie.Aufgabe2.Model;
using SPG_Fachtheorie.Aufgabe3Mvc.Services;
using System;
using System.Linq;

namespace SPG_Fachtheorie.Aufgabe3Mvc.Controllers
{
    public class OffersController : Controller
    {
        private readonly AppointmentContext _db;
        private readonly AuthService _authService;

        public OffersController(AppointmentContext db, AuthService authService)
        {
            _db = db;
            _authService = authService;
        }

        private Guid? isCoach()
        {
            var student = _db.Students.FirstOrDefault(x => x.Username == _authService.Username);
            if (student is Coach)
                return student.Id;
            else
                return null;
        }

        // GET : Offers
        [Authorize]
        public IActionResult Index()
        {
            Guid? coachId = isCoach();
            if (coachId == null)
                return Redirect("/");

            var appointmentContext = _db.Offers
                .Where(x => x.TeacherId == coachId)
                .Include(x => x.Subject)
                .Include(x => x.Appointments);
            return View(appointmentContext);
        }
    }
}
