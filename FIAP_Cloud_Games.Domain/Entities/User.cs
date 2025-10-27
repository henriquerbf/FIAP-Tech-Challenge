using System.Text.Json;
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
            var errors = new List<string>();
            
            //Is Not Null Or White Spaces
            if (string.IsNullOrWhiteSpace(password))
                errors.Add("Password cannot be empty.");
            //mín. 8 caracteres
            if (password.Length < 8)
                errors.Add("Password must be at least 8 characters long.");
            //números
            if (!Regex.IsMatch(password, "[0-9]"))
                errors.Add("Password must contain at least one number.");
            //letras
            if (!Regex.IsMatch(password, "[a-zA-Z]"))
                errors.Add("Password must contain at least one letter.");
            //caracteres especiais
            if (!Regex.IsMatch(password, @"[!@#$%^&*(),.?""':{}|<>_\-+=\\/\[\]~]"))
                errors.Add("Password must contain at least one special character.");


            if (errors.Any())
            {
                string json = JsonSerializer.Serialize(errors);
                throw new ArgumentException(json);
            }

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
            try
            {
                if (game == null) throw 
                        new ArgumentException("Objeto nulo para classe game.");

                if (!Library.Any(g => g.Id == game.Id))
                    Library.Add(game);
            }
            catch (Exception e)
            {
                var message = e.Message;
                throw;
            }
        }
    }
}
