using Estonia.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Estonia.Controllers
{
    public class BookingController : Controller
    {
        private readonly ILogger<BookingController> _logger;

        public BookingController(ILogger<BookingController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("Index");
        }

       
    }
}
