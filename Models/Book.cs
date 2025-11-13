using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "Le titre du livre est obligatoire.")]
        [StringLength(200, ErrorMessage = "Le titre ne peut pas dépasser 200 caractères.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "La date d'achat est obligatoire.")]
        public DateTime PurchaseDate { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix doit être supérieur à 0.")]
        public decimal Price { get; set; }

        public BookStatus Status { get; set; } = BookStatus.ToBuy;
    }
}
