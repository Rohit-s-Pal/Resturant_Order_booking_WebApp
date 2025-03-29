using Estonia.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Estonia.Controllers
{
    public class TestimonialController : Controller
    {
        private readonly ILogger<TestimonialController> _logger;

        public TestimonialController(ILogger<TestimonialController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("Index");
        }

       
    }
}
