namespace Estonia.Models
{
	public class VideoInfo
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

    public class MenuInfo
    {
        public int Id { get; set; }               // Unique ID for the menu item
        public string? Name { get; set; }         // Name of the menu item
        public int MenuTypeId { get; set; }       // Foreign key to Menu Type
        public decimal Price { get; set; }        // Price of the menu item
        public string? Notes { get; set; }        // Additional notes about the item
        public string? ImageURL { get; set; }     // Image URL of the item
        public bool Availability { get; set; }    // True = Available, False = Not Available
        public bool IsActive { get; set; }        // True = Active, False = Inactive
        public bool IsDeleted { get; set; }       // True = Deleted (soft delete)
        public DateTime CreatedAt { get; set; }   // Timestamp of creation
        public bool IsVeg { get; set; }          // True = Vegetarian, False = Non-Vegetarian
    }

  public class MenuViewModel
    {
        public List<MenuTypeInfo> MenuTypes { get; set; }
        public List<MenuInfo> MenuItems { get; set; }
    }

    public class MenuTypeInfo
    {
        public int MenuTypeId { get; set; }
        public string? Name { get; set; }
    }


}
