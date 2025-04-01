using Estonia.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Estonia.Data;
using System.Data;
using Estonia.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

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


 
        [HttpPost]
        public IActionResult PlaceOrder([FromBody] List<OrderItem> cartItems)
        {
            if (cartItems == null || cartItems.Count == 0)
            {
                return BadRequest(new { Message = "Cart is empty." });  // Ensure JSON response
            }

            try
            {
                int orderId = SaveOrder(cartItems);
                return Ok(new { Message = "Order placed successfully!", OrderId = orderId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred", Details = ex.Message });  // Ensure JSON response
            }
        }

        private int SaveOrder(List<OrderItem> cartItems)
        {

            var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            var userId = GetUserIdFromJwtToken(HttpContext);

            if (userId == 0)
            {
                throw new UnauthorizedAccessException("Please log in to order.");
            }


            using (SqlConnection conn = new SqlConnection(_appSettings.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Proc_InsertOrder", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId); // Replace with actual user ID if needed
                cmd.Parameters.AddWithValue("@TotalAmount", cartItems.Sum(item => item.Price * item.Quantity));

                int orderId = Convert.ToInt32(cmd.ExecuteScalar());

                foreach (var item in cartItems)
                {
                    SqlCommand itemCmd = new SqlCommand("Proc_InsertOrderItem", conn);
                    itemCmd.CommandType = CommandType.StoredProcedure;
                    itemCmd.Parameters.AddWithValue("@OrderId", orderId);
                    itemCmd.Parameters.AddWithValue("@MenuItemId", item.MenuItemId);
                    itemCmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                    itemCmd.Parameters.AddWithValue("@Price", item.Price);
                    itemCmd.ExecuteNonQuery();
                }

                return orderId;
            }
        }


        // Helper method to extract user ID from JWT token
        private int GetUserIdFromJwtToken(HttpContext httpContext)
        {
            try
            {
                // 1. Get token from header
                var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
                Console.WriteLine($"Authorization Header: {authHeader}"); // Debugging line

                if (string.IsNullOrEmpty(authHeader))
                {
                    Console.WriteLine("Authorization header is missing.");
                    return 0;
                }

                var token = authHeader.Split(' ').Last();
                Console.WriteLine($"Extracted Token: {token}"); // Debugging line

                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("Token is missing in Authorization header.");
                    return 0;
                }

                // 2. Validate and decode token
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                // 3. Try different common user ID claim types
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c =>
                    c.Type == ClaimTypes.NameIdentifier ||
                    c.Type == JwtRegisteredClaimNames.Sub ||
                    c.Type == "userId" ||
                    c.Type == "sub");

                if (userIdClaim == null)
                {
                    Console.WriteLine("User ID claim not found in token.");
                    return 0;
                }

                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    Console.WriteLine($"Invalid User ID: {userIdClaim.Value}");
                    return 0;
                }

                Console.WriteLine($"Extracted User ID: {userId}");
                return userId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting user ID: {ex.Message}");
                return 0;
            }
        }


    }
}
