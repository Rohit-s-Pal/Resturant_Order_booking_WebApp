using Estonia.Data;
using Estonia.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;

 
   

using System.Data.SqlClient;
using Microsoft.Extensions.Options;



namespace Estonia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppSettings _appSettings;

        public HomeController(IOptions<AppSettings> appSettings)
        {

            _appSettings = appSettings.Value;
        }

        public IActionResult Index()
        {
            List<noofpeople> noofpeopleList = Getnoofpeople(); // Ensure it matches the return type

            return View(noofpeopleList);
        }

        public IActionResult Privacy()
        {
            return View();
        }
 

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }





        private List<noofpeople> Getnoofpeople()
        {
            SqlParameter[] parameters = null;

            var DataSet = Dataaccess.GetDataSet("Proc_Noofpeople", CommandType.StoredProcedure, parameters, _appSettings.ConnectionString);

            // Using your existing listdata method for Menu Types
            List<noofpeople> noofpeopleList = (from rw in DataSet.Tables[0].AsEnumerable()
                                               select new noofpeople()
                                               {
                                                   Id = Convert.ToInt32(rw["Id"]),
                                                   noofpeoples = Convert.ToInt32(rw["No_of_People"])
                                               }).ToList();

            return noofpeopleList;
        }



        [HttpPost]
        public IActionResult BookNow(Booking model)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(_appSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_InsertBooking", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Name", model.Name);
                        cmd.Parameters.AddWithValue("@Email", model.Email);
                        cmd.Parameters.AddWithValue("@BookingDate", model.BookingDate);
                        cmd.Parameters.AddWithValue("@NoOfPeople", model.NoOfPeople);
                        cmd.Parameters.AddWithValue("@SpecialRequest", model.SpecialRequest);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return RedirectToAction("Index"); // Redirect after successful booking
            }
            return View("Index", Getnoofpeople()); // Reload the form with dropdown data if validation fails
        }







    }
}
