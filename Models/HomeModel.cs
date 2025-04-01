namespace Estonia.Models
{
	public class Home
	{

		public int Id { get; set; }	
		public string? Title { get; set; }	
		public string? SS1 { get; set; }	
		public string? SS2 { get; set; }	
		public string? SS3 { get; set; }	
		public string? SS4 { get; set; }
		public string? Thumbnail { get; set; }	
		public string? VideoLink { get; set; }
		public string? Content { get; set; }
		public string? Date { get; set; }
		

	}

	public class noofpeople
	{
		public int Id { get; set; }
		public int noofpeoples { get; set; }
	}

    public class Booking
    {
        public int Id { get; set; } // Primary key (auto-increment in DB)
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BookingDate { get; set; }
        public int NoOfPeople { get; set; }
        public string SpecialRequest { get; set; }
    }


    public class OrderItem
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }




}
