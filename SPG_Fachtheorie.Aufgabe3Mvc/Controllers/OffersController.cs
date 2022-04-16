using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SPG_Fachtheorie.Aufgabe2;
using SPG_Fachtheorie.Aufgabe3Mvc.Services;
using SPG_Fachtheorie.Aufgabe3Mvc.Views.Offers;
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

        [HttpGet]
        public IActionResult Index()
        {
            var offers = _db.Offers.ToList();
            return View(new IndexViewModel(
                 Offers: offers
                 ));
        }
    }
}
