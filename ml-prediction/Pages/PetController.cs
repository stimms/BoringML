using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ml_prediction.Pages
{
    public class PetController : Controller
    {
        [Route("/api/pets")]
        public IActionResult Index()
        {
            return new JsonResult(new List<string> { "Cat", "Dog" });
        }
    }
}