using FIAP_Cloud_Games.Domain.Entities;

namespace FIAP_Cloud_Games.Tests.Domain.Entities 
{ 
    public class GameTests
    {
        private static Game CreateValidGame(
            string title = "Hollow Knight",
            string description = "Metroidvania indie",
            string genre = "Action",
            decimal price = 100.00m,
            DateTime? releaseDate = null,
            decimal discount = 0.10m)
        {
            return new Game(
                title,
                description,
                genre,
                price,
                releaseDate ?? new DateTime(2017, 02, 24),
                discount
            );
        }

        [Fact]
        public void Ctor_DeveCriarComValoresValidos()
        {
            // arrange
            var before = DateTime.UtcNow;

            // act
            var game = CreateValidGame();

            var after = DateTime.UtcNow;

            // assert
            Assert.NotEqual(Guid.Empty, game.Id);
            Assert.Equal("Hollow Knight", game.Title);
            Assert.Equal("Metroidvania indie", game.Description);
            Assert.Equal("Action", game.Genre);
            Assert.Equal(100.00m, game.Price);
            Assert.Equal(0.10m, game.Discount);
            Assert.Equal(new DateTime(2017, 02, 24), game.ReleaseDate);

            // AddedAtDate ~ agora (tolerância de 2s)
            Assert.True(game.AddedAtDate >= before && game.AddedAtDate <= after);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void UpdateTitle_DeveFalhar_QuandoTituloInvalido(string invalid)
        {
            var game = CreateValidGame();

            var ex = Assert.Throws<ArgumentException>(() => game.UpdateTitle(invalid));
            Assert.Contains("Invalid title", ex.Message);
        }

        [Fact]
        public void UpdateTitle_DeveAtualizar_QuandoValido()
        {
            var game = CreateValidGame();

            game.UpdateTitle("New Title");

            Assert.Equal("New Title", game.Title);
        }

        [Fact]
        public void UpdateDescription_DeveAceitarNullEVirarVazio()
        {
            var game = CreateValidGame(description: "x");

            game.UpdateDescription(null);

            Assert.Equal(string.Empty, game.Description);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void UpdateGenre_DeveFalhar_QuandoGeneroInvalido(string invalid)
        {
            var game = CreateValidGame();

            var ex = Assert.Throws<ArgumentException>(() => game.UpdateGenre(invalid));
            Assert.Contains("Invalid genre", ex.Message);
        }

        [Fact]
        public void UpdateGenre_DeveAtualizar_QuandoValido()
        {
            var game = CreateValidGame();

            game.UpdateGenre("RPG");

            Assert.Equal("RPG", game.Genre);
        }

        [Fact]
        public void UpdatePrice_DeveFalhar_QuandoNegativo()
        {
            var game = CreateValidGame();

            var ex = Assert.Throws<ArgumentException>(() => game.UpdatePrice(-0.01m));
            Assert.Contains("Price cannot be negative", ex.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(59.90)]
        [InlineData(9999.99)]
        public void UpdatePrice_DeveAtualizar_QuandoValido(decimal price)
        {
            var game = CreateValidGame();

            game.UpdatePrice(price);

            Assert.Equal(price, game.Price);
        }

        [Theory]
        [InlineData(-0.01)]
        [InlineData(1.01)]
        public void SetDiscount_DeveFalhar_ForaDoIntervalo(decimal invalid)
        {
            var game = CreateValidGame();

            var ex = Assert.Throws<ArgumentException>(() => game.SetDiscount((decimal)invalid));
            Assert.Contains("between 0 and 1", ex.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(0.25)]
        [InlineData(1)]
        public void SetDiscount_DeveAceitarBordasEValoresValidos(decimal value)
        {
            var game = CreateValidGame();

            game.SetDiscount(value);

            Assert.Equal(value, game.Discount);
        }

        [Fact]
        public void SalePrice_DeveCalcularComBaseEmPrecoEDesconto()
        {
            var game = CreateValidGame(price: 200m, discount: 0.25m);

            Assert.Equal(150m, game.SalePrice); // 200 - (200 * 0.25)
        }

        [Fact]
        public void SalePrice_DeveRefletirMudancasEmPrecoOuDesconto()
        {
            var game = CreateValidGame(price: 100m, discount: 0.10m);
            Assert.Equal(90m, game.SalePrice);

            game.UpdatePrice(80m);
            Assert.Equal(72m, game.SalePrice); // 80 - (80 * 0.10)

            game.SetDiscount(0.50m);
            Assert.Equal(40m, game.SalePrice); // 80 - (80 * 0.50)
        }

        [Fact]
        public void SetReleaseDate_DeveFalhar_QuandoDefault()
        {
            var game = CreateValidGame();

            var ex = Assert.Throws<ArgumentException>(() => game.SetReleaseDate(default));
            Assert.Contains("Release date is required", ex.Message);
        }

        [Fact]
        public void SetReleaseDate_DeveAtualizar_QuandoValido()
        {
            var game = CreateValidGame();
            var newDate = new DateTime(2020, 12, 31);

            game.SetReleaseDate(newDate);

            Assert.Equal(newDate, game.ReleaseDate);
        }

        [Fact]
        public void Ctor_DeveFalhar_QuandoAlgumParametroInvalido()
        {
            // title inválido
            Assert.Throws<ArgumentException>(() =>
                new Game(" ", "desc", "Action", 10m, new DateTime(2020, 1, 1)));

            // genre inválido
            Assert.Throws<ArgumentException>(() =>
                new Game("Title", "desc", " ", 10m, new DateTime(2020, 1, 1)));

            // price negativo
            Assert.Throws<ArgumentException>(() =>
                new Game("Title", "desc", "Action", -1m, new DateTime(2020, 1, 1)));

            // discount fora do intervalo
            Assert.Throws<ArgumentException>(() =>
                new Game("Title", "desc", "Action", 10m, new DateTime(2020, 1, 1), 1.1m));

            // release date default
            Assert.Throws<ArgumentException>(() =>
                new Game("Title", "desc", "Action", 10m, default, 0m));
        }
    }
}