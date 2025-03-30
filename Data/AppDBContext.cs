using Microsoft.EntityFrameworkCore;
using Estonia.Models;


namespace test.Data
{
	public class AppDBContext : DbContext
	{
		public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) 
		{

		}
 
	}
}
