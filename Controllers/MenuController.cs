using Estonia.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Estonia.Data;
using System.Data;
using Estonia.Models;
 
using System.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Estonia.Controllers
{
    public class MenuController : Controller
    {
        private readonly ILogger<MenuController> _logger;
        private readonly AppSettings _appSettings;
        public MenuController(IOptions<AppSettings> appSettings)
        {

            _appSettings = appSettings.Value;
        }

        public IActionResult Index()
        {

            var menuData = GetMenuByType();
            return View("Index", menuData);
        }


        private MenuViewModel GetMenuByType()
        {
            SqlParameter[] parameters = null;

            var DataSet = Dataaccess.GetDataSet("Proc_MenuList", CommandType.StoredProcedure, parameters, _appSettings.ConnectionString);
            MenuViewModel menuViewModel = new MenuViewModel();

            // Using your existing listdata method for Menu Types
            menuViewModel.MenuTypes = (from rw in DataSet.Tables[0].AsEnumerable()
                                       select new MenuTypeInfo()
                                       {
                                           MenuTypeId = Convert.ToInt32(rw["Id"]),
                                           Name = Convert.ToString(rw["MenuTypeName"])
                                       }).ToList();

            // Using your existing listdata method for Menu Items
            menuViewModel.MenuItems = (from rw in DataSet.Tables[1].AsEnumerable()
                                       select new MenuInfo()
                                       {
                                           Id = Convert.ToInt32(rw["ID"]),
                                           Name = Convert.ToString(rw["Name"]),
                                           MenuTypeId = Convert.ToInt32(rw["MenuTypeId"]),
                                           Price = Convert.ToDecimal(rw["Price"]),
                                           Notes = Convert.ToString(rw["Notes"]),
                                           ImageURL = Convert.ToString(rw["ImageURL"]),
                                           Availability = Convert.ToBoolean(rw["Availability"]),
                                           IsActive = Convert.ToBoolean(rw["IsActive"]),
                                           IsDeleted = Convert.ToBoolean(rw["IsDeleted"]),
                                           CreatedAt = Convert.ToDateTime(rw["CreatedAt"]),
                                           IsVeg = Convert.ToBoolean(rw["IsVeg"])
                                       }).ToList();

            return menuViewModel;
        }
    }
}
