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
        private readonly AuthService _authService;
        private readonly AppointmentContext _appointmentContext;

        public OffersController(AuthService authService, AppointmentContext appointmentContext)
        {
            _authService = authService;
            _appointmentContext = appointmentContext;
        }

        private Guid? IsCoach()
        {
            var coach = _appointmentContext.Students.FirstOrDefault(x => x.Username == _authService.Username);
            if (coach != null && coach is Coach)
                return coach.Id;
            return null;
        }

        // /Offers/Index
        [Authorize]
        public IActionResult Index()
        {
            Guid? coachId = IsCoach();

            var context = _appointmentContext.Offers
                .Where(x => x.TeacherId == coachId)
                .Include(o => o.Subject)
                .Include(o => o.Appointments);
            return View(context);
        }
    }
}
