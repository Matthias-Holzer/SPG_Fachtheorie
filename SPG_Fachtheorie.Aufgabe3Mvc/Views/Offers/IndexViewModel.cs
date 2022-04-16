using Microsoft.AspNetCore.Mvc.Rendering;
using SPG_Fachtheorie.Aufgabe2.Model;
using System.Collections.Generic;

namespace SPG_Fachtheorie.Aufgabe3Mvc.Views.Offers
{
    public record IndexViewModel
    (
        List<Offer> Offers
    );
}
