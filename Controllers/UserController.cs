 using Estonia.Data;
using Estonia.Models;
using Estonia.Service;
 using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
 using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
 

using Microsoft.AspNetCore.Hosting;


namespace Estonia.Controllers
{
    [Route("user")] // Base route for all actions in this controller
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly AppSettings _appSettings;
        private readonly ITokenService _tokenService;
        private readonly IWebHostEnvironment _env;
        public UserController(IOptions<AppSettings> appSettings,
            ITokenService tokenService,
    IWebHostEnvironment env)
        {
            _appSettings = appSettings.Value;
            _tokenService = tokenService;
            _env = env;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        // GET: /user/signup
        [HttpGet("signup")]
        public IActionResult SignUp()
        {
            return View();
        }

        // Sign-Up Action
        [HttpPost("signup")] // POST /user/signup
        public IActionResult SignUp([FromBody] UserRegisterModel userRegisterModel)
        {
            if (userRegisterModel == null ||
                string.IsNullOrEmpty(userRegisterModel.Username) ||
                string.IsNullOrEmpty(userRegisterModel.Password))
            {
                return BadRequest(new { Message = "Username or password is missing." });
            }

            try
            {
                // Check if user already exists
                if (UserExists(userRegisterModel.Username))
                {
                    return BadRequest(new { Message = "Username already exists." });
                }

                // Save user to the database
                SaveUser(userRegisterModel);

                return Ok(new { Message = "User registered successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred", Details = ex.Message });
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginModel userLoginModel)
        {
            if (userLoginModel == null || string.IsNullOrEmpty(userLoginModel.Username) || string.IsNullOrEmpty(userLoginModel.Password))
            {
                return BadRequest(new { Message = "Username or password is missing." });
            }

            try
            {
                var userId = AuthenticateUser(userLoginModel.Username, userLoginModel.Password);

                if (userId == 0)
                {
                    return Unauthorized(new { Message = "Invalid credentials." });
                }

                var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, userLoginModel.Username)
        };

                var token = _tokenService.GenerateAccessToken(claims);
                var refreshToken = _tokenService.GenerateRefreshToken();

                // Set cookie with token
                Response.Cookies.Append("access_token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = !_env.IsDevelopment(), // Now using the injected environment
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.UtcNow.AddHours(1)
                });

                return Ok(new
                {
                    Message = "Login successful!",
                    Token = token,
                    RefreshToken = refreshToken,
                    ExpiresIn = 3600,
                    UserName = userLoginModel.Username  
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "An error occurred",
                    Details = ex.Message
                });
            }
        }
        // Helper Methods for SignUp and Login

        private bool UserExists(string username)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Username", SqlDbType.NVarChar, 255) { Value = username }
            };

            var result = Dataaccess.ExecuteScalar("Proc_CheckUserExists", CommandType.StoredProcedure, parameters, _appSettings.ConnectionString);

            return Convert.ToInt32(result) > 0;
        }

        private void SaveUser(UserRegisterModel userRegisterModel)
        {
            using (var conn = new SqlConnection(_appSettings.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Proc_RegisterUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", userRegisterModel.Username);
                cmd.Parameters.AddWithValue("@Password", userRegisterModel.Password);  // In a real app, hash the password before storing
                cmd.Parameters.AddWithValue("@Email", userRegisterModel.Email);

                cmd.ExecuteNonQuery();
            }
        }

        private int AuthenticateUser(string username, string password)
        {
            using (var conn = new SqlConnection(_appSettings.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Proc_AuthenticateUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);  // In a real app, check hashed password

                var result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }
 
    
    }
}
