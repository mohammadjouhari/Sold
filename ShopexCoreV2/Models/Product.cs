namespace ShopexCoreV2.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
		public string Description { get; set; }
        public int Price { get; set; }
		public string Type { get; set; }
		public string Model { get; set; }
		public string ImagePath { get; set; }
		public int UserId { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
    }
}
