using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe2;
using SPG_Fachtheorie.Aufgabe2.Model;
using SPG_Fachtheorie.Aufgabe3Mvc.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        private Guid? IsCoach()
        {
            var student = _db.Students.FirstOrDefault(x => x.Username == _authService.Username);
            if (student is Coach)
                return student.Id;
            else
                return null;
        }

        // GET : Offers
        public IActionResult Index()
        {
            Guid? coachId = IsCoach();
            if (coachId == null)
                return Redirect("/");

            var appointmentContext = _db.Offers
                .Where(x => x.TeacherId == coachId)
                .Include(x => x.Subject)
                .Include(x => x.Appointments);
            return View(appointmentContext);
        }

        //GET : Offers/Details/{Guid}
        public IActionResult Details(Guid? id)
        {
            if (IsCoach == null)
                return Redirect("/");
            if (id == null)
                return NotFound();

            var appointmentContext = _db.Appointments
                .Include(x => x.Student)
                .Where(x => x.OfferId == id);
            return View(appointmentContext);
        }

        //POST : Offers/Add/{Guid}
        [Authorize]
        public IActionResult Create()
        {
            if (IsCoach == null)
                return Redirect("/");
            ViewData["SubjectId"] = new SelectList(_db.Subjects
                .Select(x => new
                {
                    SubjectId = x.Id,
                    Text = x.Term + " - " + x.Name + " - " + x.EducationType
                }), "SubjectId", "Text");
            return View();
        }

        
        
    }
}
