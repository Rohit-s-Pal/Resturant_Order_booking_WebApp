using Estonia.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Estonia.Controllers
{
    public class TeamController : Controller
    {
        private readonly ILogger<TeamController> _logger;

        public TeamController(ILogger<TeamController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("Index");
        }

       
    }
}
