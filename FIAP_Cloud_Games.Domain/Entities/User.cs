using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP_Cloud_Games.Domain.Entities
{
    public class User
    {
        // Properties
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Role { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public DateTime CreatedDate { get; private set; } = DateTime.Now;

        // Constructors
        private User() { }

        // Constructor used when creating new instances
        public User(string name, string tole, string email, string password)
        {
            Name = name;
            Role = tole;
            Email = email;
            Password = password;
        }
    }
}
