namespace HobbyListAPI.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }
        public decimal Price { get; set; }
    }
}
