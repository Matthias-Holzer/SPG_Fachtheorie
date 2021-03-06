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

        //GET : Offers/Add
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

        // POST: Offers/Add
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubjectId,From,To")] Offer offer)
        {
            var coachId = IsCoach();
            if (coachId == null)
                return Redirect("/");
            if (offer.To < DateTime.Now)
                ModelState.AddModelError("To", "Can not create Offers in the past");

            if (!ModelState.IsValid)
            {
                ViewData["SubjectId"] = new SelectList(_db.Subjects.Select(x => new
                {
                    SubjectId = x.Id,
                    Text = x.Term + "-" + x.Name + "-" + x.EducationType
                }), "SubjectId", "Text");
                return View(offer);
            }

            offer.TeacherId = coachId.Value;
            offer.Id = Guid.NewGuid();

            _db.Offers.Add(offer);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));            
        }

        // GET: Offers/Edit/{offerGuid}
        [Authorize]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (IsCoach == null)
                return Redirect("/");
            if (id == null)
                return NotFound();

            var offer = await _db.Offers
                .Include(x => x.Subject)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (offer == null)
                return NotFound();
            return View(offer);
        }

        // POST: Offers/Edit/{offerGuid}
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,To")] Offer offer)
        {
            if (IsCoach() == null)
                return Redirect("/");
            if (id != offer.Id)
                return NotFound();
            if (!ModelState.IsValid)
                return View(offer);

            var dbOffer = await _db.Offers
                .Include(x => x.Subject)
                .FirstOrDefaultAsync(x => x.Id == offer.Id);
            if (dbOffer == null)
                return NotFound();

            if (offer.To < dbOffer.To)
            {
                ModelState.AddModelError("To", "Can not set date before saved date");
                return View(dbOffer);
            }

            dbOffer.To = offer.To;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Offers/Delete/{offerGuid}
        [Authorize]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (IsCoach == null)
                return Redirect("/");
            if (id == null)
                return NotFound();
            var offer = await _db.Offers
                .Include(x => x.Appointments)
                .Include(x => x.Subject)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (offer == null)
                return NotFound();
            return View(offer);
        }

        // POST: Offers/Delete/{offerGuid}
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (IsCoach() == null)
                return Redirect("/");

            var offer = await _db.Offers
                .Include(x => x.Appointments)
                .Include(x => x.Subject)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (offer == null)
                return NotFound();

            if (offer.Appointments.Count > 0)
            {
                ModelState.AddModelError("Appointments", "Offer has Appointments, can therefore not be deleted");
                return View(offer);
            }

            _db.Offers.Remove(offer);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
