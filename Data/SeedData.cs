using HobbyListAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HobbyListAPI.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            // Vérifie si des livres existent déjà
            if (context.Books.Any())
                return; // Si oui, on ne fait rien

            var books = new List<Book>
            {
                new Book
                {
                    Title = "The Hobbit",
                    PurchaseDate = DateTime.SpecifyKind(new DateTime(2025, 11, 1), DateTimeKind.Utc),
                    Price = 15.99M
                },
                new Book
                {
                    Title = "1984",
                    PurchaseDate = DateTime.SpecifyKind(new DateTime(2025, 10, 15), DateTimeKind.Utc),
                    Price = 12.50M
                },
                new Book
                {
                    Title = "Dune",
                    PurchaseDate = DateTime.SpecifyKind(new DateTime(2025, 9, 20), DateTimeKind.Utc),
                    Price = 20.00M
                }
            };



            context.Books.AddRange(books);
            context.SaveChanges();
        }
    }
}
