using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP_Cloud_Games.Domain.Entities
{
    public class Game
    {
        // Properties
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Genre { get; private set; }
        public DateTime ReleaseDate { get; private set; }
        public decimal Price { get; private set; }
        public decimal Discount { get; private set; }
        public decimal SalePrice => Price - (Price * Discount);

        // Constructor required by EF Core (reflection)
        private Game() { }

        // Constructor used when creating new instances
        public Game(string title, string description, string genre, decimal price, DateTime releaseDate)
        {
            Title = title;
            Description = description;
            Genre = genre;
            Price = price;
            ReleaseDate = releaseDate;
        }

    }
}
