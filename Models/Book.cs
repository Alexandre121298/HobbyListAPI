namespace HobbyListAPI.Models
{
    public enum BookStatus
    {
        ToBuy,
        Purchased
    }

    public class Book
    {
        public int Id { get; set; } // clé primaire auto-incrémentée
        public string Title { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }
        public decimal Price { get; set; }
        public BookStatus Status { get; set; } = BookStatus.ToBuy;
    }
}
