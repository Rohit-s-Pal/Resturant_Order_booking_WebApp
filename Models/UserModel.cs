namespace Estonia.Models
{
	public class UserModel
	{

        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }


    }

    public class UserRegisterModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class UserLoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }


}
