using Blazorcrud.Server.Authorization;
using Blazorcrud.Shared.Models;
using Moq;
using NUnit.Framework;

namespace Blazorcrud.ServerTests
{
    public class UserRepositoryTests : RepositoryTestBase
    {
        [Test]
        // Test case to ensure that GetUser method returns the correct user.
        public async Task GetUser_ReturnsCorrectUser()
        {
            // Arrange: Setting up expected user in the database.
            var expectedUser = new User
            {
                Id = 1,
                Username = "Test1",
                FirstName = "FirstName1",
                LastName = "LastName1",
                Password = "Password1"
            };
            await AppDbContext.Users.AddAsync(expectedUser);
            await AppDbContext.SaveChangesAsync();

            // Act: Calling GetUser method with the expected user's ID.
            var actualUser = await UserRepository.GetUser(expectedUser.Id);

            // Assert: Verifying that the returned user matches the expected user.
            Assert.NotNull(actualUser);
            Assert.AreEqual(expectedUser.Id, actualUser.Id);
            Assert.AreEqual(expectedUser.Username, actualUser.Username);
        }

        [Test]
        // Test case to ensure that AddUser method successfully adds a new user.
        public async Task AddUser_SuccessfullyAddsUser()
        {
            // Arrange: Setting up a new user to be added.
            var newUser = new User
            {
                Id = 1,
                Username = "Test1",
                FirstName = "FirstName1",
                LastName = "LastName1",
                Password = "Password1"
            };

            // Act: Calling AddUser method with the new user.
            var addedUser = await UserRepository.AddUser(newUser);

            // Assert: Verifying that the added user matches the new user.
            Assert.NotNull(addedUser);
            Assert.AreEqual(newUser.Username, addedUser.Username);
        }

        [Test]
        // Test case to ensure that DeleteUser method successfully deletes a user.
        public async Task DeleteUser_SuccessfullyDeletesUser()
        {
            // Arrange: Setting up a user to be deleted.
            var userToDelete = new User
            {
                Id = 1,
                Username = "Test1",
                FirstName = "FirstName1",
                LastName = "LastName1",
                Password = "Password1"
            };
            await AppDbContext.Users.AddAsync(userToDelete);
            await AppDbContext.SaveChangesAsync();

            // Act: Calling DeleteUser method with the user's ID.
            var deletedUser = await UserRepository.DeleteUser(userToDelete.Id);

            // Assert: Verifying that the deleted user matches the user to be deleted and the user no longer exists in the database.
            Assert.NotNull(deletedUser);
            Assert.AreEqual(userToDelete.Id, deletedUser.Id);

            var searchUser = await AppDbContext.Users.FindAsync(deletedUser.Id);
            Assert.IsNull(searchUser);
        }

        [Test]
        // Test case to ensure that GetUsers method returns the correct number of users and specific users.
        public async Task GetUsers_ReturnsCorrectUsers()
        {
            // Arrange: Setting up two users in the database.
            var user1 = new User
            {
                Id = 1,
                Username = "Test1",
                FirstName = "FirstName1",
                LastName = "LastName1",
                Password = "Password1"
            };
            var user2 = new User
            {
                Id = 2,
                Username = "Test2",
                FirstName = "FirstName2",
                LastName = "LastName2",
                Password = "Password2"
            };
            await AppDbContext.AddRangeAsync(user1, user2);
            await AppDbContext.SaveChangesAsync();

            // Act: Calling GetUsers method.
            var users = UserRepository.GetUsers(null, 1);

            // Assert: Verifying that the returned users list contains both users.
            Assert.AreEqual(2, users.Results.Count);
            Assert.Contains(user1, users.Results.ToList());
            Assert.Contains(user2, users.Results.ToList());
        }

        [Test]
        // Test case to ensure that UpdateUser method successfully updates a user's details.
        public async Task UpdateUser_SuccessfullyUpdatesUser()
        {
            // Arrange: Setting up an original user in the database.
            var originalUser = new User
            {
                Id = 1,
                Username = "Original",
                FirstName = "OriginalFirstName",
                LastName = "OriginalLastName",
                Password = "OriginalPassword"
            };
            await AppDbContext.Users.AddAsync(originalUser);
            await AppDbContext.SaveChangesAsync();

            // Preparing the updated user.
            var updatedUser = new User
            {
                Id = 1,
                Username = "Updated",
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                Password = "UpdatedPassword"
            };

            // Act: Calling UpdateUser method with the updated user.
            var result = await UserRepository.UpdateUser(updatedUser);

            // Assert: Verifying that the returned user matches the updated user.
            Assert.NotNull(result);
            Assert.AreEqual(updatedUser.Id, result.Id);
            Assert.AreEqual(updatedUser.Username, result.Username);
            Assert.AreEqual(updatedUser.FirstName, result.FirstName);
            Assert.AreEqual(updatedUser.LastName, result.LastName);
        }

        [Test]
        // Test case to ensure that Authenticate method returns the correct user upon successful authentication.
        public void Authenticate_ReturnsCorrectUser()
        {
            // Arrange: Setting up a user in the database and mock authentication request.
            var expectedUser = new User
            {
                Id = 1,
                Username = "TestUser",
                FirstName = "Test",
                LastName = "User",
                Password = "TestPassword",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("TestPassword")
            };
            AppDbContext.Users.Add(expectedUser);
            AppDbContext.SaveChanges();

            var authRequest = new AuthenticateRequest { Username = "TestUser", Password = "TestPassword" };
            MockJwtUtils.Setup(jwt => jwt.GenerateToken(It.IsAny<User>())).Returns("TestToken");

            // Act: Calling Authenticate method with the authentication request.
            var actualUser = UserRepository.Authenticate(authRequest);

            // Assert: Verifying that the authenticated user matches the expected user and token.
            Assert.NotNull(actualUser);
            Assert.AreEqual(expectedUser.Id, actualUser.Id);
            Assert.AreEqual(expectedUser.Username, actualUser.Username);
            Assert.AreEqual("TestToken", actualUser.Token);
        }
    }
}
