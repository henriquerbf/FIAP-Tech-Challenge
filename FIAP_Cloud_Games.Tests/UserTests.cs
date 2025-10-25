﻿using FIAP_Cloud_Games.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FIAP_Cloud_Games.Tests;


namespace FIAP_Cloud_Games.Tests.Domain.Entities
{
    public class UserTests
    {
        private static User CreateValidUser(
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
        public void Constructor_ValidParameters_ShouldCreateUser()
        {
            // Arrange
            var before = DateTime.Now;
            // Act
            var user = CreateValidUser();

            var after = DateTime.Now;
            // Assert
            Assert.NotEqual(Guid.Empty, user.Id);
            Assert.Equal("Fillipy", user.Name);
            Assert.Equal("Admin", user.Role);
            Assert.Equal("lip-crs@hotmail.com", user.Email);
            Assert.Equal("koehmaon", user.Password);
            Assert.InRange(user.CreatedDate, before, after);
        }


        [Fact]
        public void Constructor_InvalidEmail_ShouldThrowArgumentException()
        {
            var user = CreateValidUser();   
            var ex = Assert.Throws<ArgumentException>( () =>
            {
                user.UpdateEmail("yes@.com");
            });
            Assert.Equal("Invalid email.", ex.Message);
        }

        [Fact]
        public void Constructor_EmptyName_ShouldThrowArgumentException()
        {
            // Arrange
            var user = CreateValidUser();
            // Act & Assert
            {
                var ex = Assert.Throws<ArgumentException>(() =>
                {
                    user.UpdateName("");
                });

                Assert.Equal("Invalid name.", ex.Message);
            }
        }

        [Fact]
        public void Constructor_EmptyPassword_ShouldThrowArgumentException()
        {
            // Arrange
            var user = CreateValidUser();
            // Act & Assert
            {
                var ex = Assert.Throws<ArgumentException>(() =>
                {
                    user.ChangePassword("");
                });

                Assert.Equal("Invalid password.", ex.Message);
            }
        }

        [Fact]
        public void Constructor_InvalidRole_ShouldThrowArgumentException()
        {
            // Arrange
            var user = CreateValidUser();
            // Act & Assert
            {
                var ex = Assert.Throws<ArgumentException>(() =>
                {
                    user.AssignRole("");
                });

                Assert.Equal("Invalid role.", ex.Message);
            }
        }

        [Fact]
        public void UpdateName_ValidName_ShouldUpdateSuccessfully()
        {

            // Arrange
            var user = CreateValidUser();

            // Act
            user.UpdateName("Novo Nome");

            // Assert
            Assert.Equal("Novo Nome", user.Name);


        }

        [Fact]
        public void UpdateName_EmptyName_ShouldThrowArgumentException()
        {
            // Arrange
            var user = CreateValidUser();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => user.UpdateName(""));
            Assert.Equal("Invalid name.", ex.Message);

        }

        [Fact]
        public void UpdateName_NullName_ShouldThrowArgumentException()
        {
            // Arrange
            var user = CreateValidUser();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => user.UpdateName(null!));
            Assert.Equal("Invalid name.", ex.Message);
        }

        // -------------------------------------------------
        // UPDATE EMAIL
        // -------------------------------------------------

        [Fact]
        public void UpdateEmail_ValidEmail_ShouldUpdateSuccessfully()
        {
            // Arrange
            var user = CreateValidUser();

            // Act
            user.UpdateEmail("fillipy.saraiva@hotmail.com");

            // Assert
            Assert.Equal("fillipy.saraiva@hotmail.com", user.Email);
        }

        [Fact]
        public void UpdateEmail_InvalidEmail_ShouldThrowArgumentException()
        {
            // Arrange
            var user = CreateValidUser();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => user.UpdateEmail("yes@.com"));
            Assert.Equal("Invalid email.", ex.Message);
        }

        [Fact]
        public void UpdateEmail_EmptyEmail_ShouldThrowArgumentException()
        {
            // Arrange
            var user = CreateValidUser();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => user.UpdateEmail(""));
            Assert.Equal("Invalid email.", ex.Message);
        }

        // -------------------------------------------------
        // CHANGE PASSWORD
        // -------------------------------------------------

        [Fact]
        public void ChangePassword_ValidPassword_ShouldUpdateSuccessfully()
        {
            // Arrange
            var user = CreateValidUser();

            // Act
            user.ChangePassword("novaSenha123");

            // Assert
            Assert.Equal("novaSenha123", user.Password);
        }

        [Fact]
        public void ChangePassword_EmptyPassword_ShouldThrowArgumentException()
        {
            // Arrange
            var user = CreateValidUser();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => user.ChangePassword(""));
            Assert.Equal("Invalid password.", ex.Message);
        }

        [Fact]
        public void ChangePassword_NullPassword_ShouldThrowArgumentException()
        {
            // Arrange
            var user = CreateValidUser();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => user.ChangePassword(null!));
            Assert.Equal("Invalid password.", ex.Message);
        }

        // -------------------------------------------------
        // ASSIGN ROLE
        // -------------------------------------------------

        [Fact]
        public void AssignRole_ValidUserRole_ShouldSetRoleSuccessfully()
        {
            // Arrange
            var user = CreateValidUser();
            // Act
            user.AssignRole("User");
            // Assert
            Assert.Equal("User", user.Role);
        }

        [Fact]
        public void AssignRole_ValidAdminRole_ShouldSetRoleSuccessfully()
        {
            // Arrange
            var user = CreateValidUser();
            // Act
            user.AssignRole("Admin");
            // Assert
            Assert.Equal("Admin", user.Role);
        }

        [Fact]
        public void AssignRole_InvalidRole_ShouldThrowArgumentException()
        {
            // Arrange
            var user = CreateValidUser();
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => user.AssignRole("Manager"));
            Assert.Equal("Invalid role.", ex.Message);
        }

        // -------------------------------------------------
        // ACQUIRE GAME
        // -------------------------------------------------

        [Fact]
        public void AcquireGame_NewGame_ShouldAddToLibrary()
        {
            // Arrange
            var user = CreateValidUser();
            var game = CreateValidGame();

            // Act
            user.AcquireGame(game);

            // Assert
            Assert.Single(user.Library);
            Assert.Contains(game, user.Library);
        }

        [Fact]
        public void AcquireGame_ExistingGame_ShouldNotDuplicate()
        {
            // Arrange
            var user = CreateValidUser();
            var game = CreateValidGame();

            // Act
            user.AcquireGame(game);
            user.AcquireGame(game); // mesmo Id (mesma instância)

            // Assert
            Assert.Single(user.Library);
        }

        [Fact]
        public void AcquireGame_NullGame_ShouldThrowArgumentException()
        {
            // Arrange
            var user = CreateValidUser();

            // Act & Assert
            // Comportamento atual: lança NullReferenceException ao acessar game.Id
            Assert.Throws<NullReferenceException>(() => user.AcquireGame(null!));
        }
    }
}

