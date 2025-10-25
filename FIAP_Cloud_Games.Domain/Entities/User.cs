using System.Text.RegularExpressions;

namespace FIAP_Cloud_Games.Domain.Entities
{
    /// <summary>
    /// Represents a system user (client or administrator).
    /// </summary>
    public class User
    {
        // Properties
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Role { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public DateTime CreatedDate { get; private set; } = DateTime.Now;
        public List<Game> Library { get; private set; }

        // Constructors
        private User() { }

        // Constructor used when creating new instances
        public User(string name, string role, string email, string password)
        {
            UpdateName(name);
            UpdateEmail(email);
            ChangePassword(password);
            AssignRole(role);

            Id = Guid.NewGuid();
            Library = new List<Game>();
        }

        // Guard Methods
        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Invalid name.");
            Name = name;
        }

        private static readonly Regex EmailRegex = new Regex(
        @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public void UpdateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !EmailRegex.IsMatch(email))
                throw new ArgumentException("Invalid email.");

            Email = email;
        }

        // Business Methods
        public void ChangePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Invalid password.");
            Password = password;
        }

        public void AssignRole(string role)
        {
            if (role != "User" && role != "Admin")
                throw new ArgumentException("Invalid role.");
            Role = role;
        }

        public void AcquireGame(Game game)
        {
            if (!Library.Any(g => g.Id == game.Id))
                Library.Add(game);
        }
    }
}
