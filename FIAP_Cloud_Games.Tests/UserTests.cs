using FIAP_Cloud_Games.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP_Cloud_Games.Tests.Domain.Entities
{
    public class UserTests
    {
        private static User createValidUser(
            string name = "Fillipy",
            string role = "Admin",
            string email = "lip-crs@hotmail.com",
            string password = "koehmaon",
            DateTime? createdDate = null,
            List<Game>? library = null
        )
        {
            library ??= new List<Game>
            {
                new Game("Hollow Knight", "Metroidvania indie", "Action", 100.00m, new DateTime(2017, 02, 24), 0.10m),
                new Game("Hollow Knight2", "Metroidvania indie2", "Action2", 101.00m, new DateTime(2017, 02, 24), 0.10m)
            };


            return new User(
                name,
                role,
                email,
                password
            );
        }

        [Fact]
        public void Constructor_ValidParameters_ShouldCreateUser()
        {
            // Arrange
            var before = DateTime.UtcNow;
            // Act
            var user = createValidUser();
            var after = DateTime.UtcNow;
            // Assert
            Assert.NotEqual(Guid.Empty, user.Id);
            Assert.Equal("Fillipy", user.Name);
            Assert.Equal("Admin", user.Role);
            Assert.Equal("lip-crs@hotmail.com", user.Email);
            Assert.Equal("koehmaon", user.Password);
            Assert.True(user.CreatedDate >= before && user.CreatedDate <= after);
        }


        [Fact]
        public void Constructor_InvalidEmail_ShouldThrowArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>( () =>
            {
                createValidUser("Fillipy", "Admin", "yes@.com"); ;
            });
            Assert.Equal("Invalid email.", ex.Message);
        }

        [Fact]
        public void Constructor_EmptyName_ShouldThrowArgumentException()
        {
            // Arrange

            // Act & Assert
        }

        [Fact]
        public void Constructor_EmptyPassword_ShouldThrowArgumentException()
        {
            // Arrange

            // Act & Assert
        }

        [Fact]
        public void Constructor_InvalidRole_ShouldThrowArgumentException()
        {
            // Arrange

            // Act & Assert
        }

        // -------------------------------------------------
        // UPDATE NAME
        // -------------------------------------------------

        [Fact]
        public void UpdateName_ValidName_ShouldUpdateSuccessfully()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public void UpdateName_EmptyName_ShouldThrowArgumentException()
        {
            // Arrange

            // Act & Assert
        }

        [Fact]
        public void UpdateName_NullName_ShouldThrowArgumentException()
        {
            // Arrange

            // Act & Assert
        }

        // -------------------------------------------------
        // UPDATE EMAIL
        // -------------------------------------------------

        [Fact]
        public void UpdateEmail_ValidEmail_ShouldUpdateSuccessfully()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public void UpdateEmail_InvalidEmail_ShouldThrowArgumentException()
        {
            // Arrange

            // Act & Assert
        }

        [Fact]
        public void UpdateEmail_EmptyEmail_ShouldThrowArgumentException()
        {
            // Arrange

            // Act & Assert
        }

        // -------------------------------------------------
        // CHANGE PASSWORD
        // -------------------------------------------------

        [Fact]
        public void ChangePassword_ValidPassword_ShouldUpdateSuccessfully()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public void ChangePassword_EmptyPassword_ShouldThrowArgumentException()
        {
            // Arrange

            // Act & Assert
        }

        [Fact]
        public void ChangePassword_NullPassword_ShouldThrowArgumentException()
        {
            // Arrange

            // Act & Assert
        }

        // -------------------------------------------------
        // ASSIGN ROLE
        // -------------------------------------------------

        [Fact]
        public void AssignRole_ValidUserRole_ShouldSetRoleSuccessfully()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public void AssignRole_ValidAdminRole_ShouldSetRoleSuccessfully()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public void AssignRole_InvalidRole_ShouldThrowArgumentException()
        {
            // Arrange

            // Act & Assert
        }

        // -------------------------------------------------
        // ACQUIRE GAME
        // -------------------------------------------------

        [Fact]
        public void AcquireGame_NewGame_ShouldAddToLibrary()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public void AcquireGame_ExistingGame_ShouldNotDuplicate()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public void AcquireGame_NullGame_ShouldThrowArgumentException()
        {
            // Arrange

            // Act & Assert
        }
    }
}

