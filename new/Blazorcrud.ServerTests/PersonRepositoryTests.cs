using Blazorcrud.Shared.Models;
using NUnit.Framework;

namespace Blazorcrud.ServerTests
{
    public class PersonRepositoryTests : RepositoryTestBase
    {
        [Test]
        // This test checks whether GetPerson method returns the correct person based on the provided id
        public async Task GetPerson_ReturnsCorrectPerson()
        {
            // Arrange
            // We start by creating a person and adding it to the database
            var expectedPerson = new Person { PersonId = 1, PhoneNumber = "123456", FirstName = "Test", LastName = "User" };
            await AppDbContext.People.AddAsync(expectedPerson);
            await AppDbContext.SaveChangesAsync();

            // Act
            // Then we call the method we are testing
            var actualPerson = await PersonRepository.GetPerson(expectedPerson.PersonId);

            // Assert
            // Finally, we check if the returned person matches the one we have added
            Assert.NotNull(actualPerson);
            Assert.AreEqual(expectedPerson.PersonId, actualPerson.PersonId);
            Assert.AreEqual(expectedPerson.FirstName, actualPerson.FirstName);
            Assert.AreEqual(expectedPerson.LastName, actualPerson.LastName);
        }

        [Test]
        // This test checks whether AddPerson method adds a person to the database correctly
        public async Task AddPerson_SuccessfullyAddsPerson()
        {
            // Arrange
            // We start by creating a person
            var newPerson = new Person { FirstName = "New", PhoneNumber = "123456", LastName = "User" };

            // Act
            // Then we call the method we are testing
            var addedPerson = await PersonRepository.AddPerson(newPerson);

            // Assert
            // Finally, we check if the returned person matches the one we have added
            Assert.NotNull(addedPerson);
            Assert.AreEqual(newPerson.FirstName, addedPerson.FirstName);
            Assert.AreEqual(newPerson.LastName, addedPerson.LastName);
        }

        [Test]
        // This test checks whether DeletePerson method deletes a person from the database correctly
        public async Task DeletePerson_SuccessfullyDeletesPerson()
        {
            // Arrange
            // We start by creating a person and adding it to the database
            var personToDelete = new Person { PersonId = 2, FirstName = "Delete", PhoneNumber = "123456", LastName = "User" };
            await AppDbContext.People.AddAsync(personToDelete);
            await AppDbContext.SaveChangesAsync();

            // Act
            // Then we call the method we are testing
            var deletedPerson = await PersonRepository.DeletePerson(personToDelete.PersonId);

            // Assert
            // Finally, we check if the person was deleted successfully
            Assert.NotNull(deletedPerson);
            Assert.AreEqual(personToDelete.PersonId, deletedPerson.PersonId);

            var searchPerson = await AppDbContext.People.FindAsync(deletedPerson.PersonId);
            Assert.IsNull(searchPerson);
        }

        [Test]
        // This test checks whether GetPeople method returns all persons in the database correctly
        public async Task GetPeople_ReturnsCorrectPeople()
        {
            // Arrange
            // We start by creating two people and adding them to the database
            var person1 = new Person { PersonId = 1, FirstName = "Test1", LastName = "User1", PhoneNumber = "123456" };
            var person2 = new Person { PersonId = 2, FirstName = "Test2", LastName = "User2", PhoneNumber = "12345" };
            await AppDbContext.AddRangeAsync(person1, person2);
            await AppDbContext.SaveChangesAsync();

            // Act
            // Then we call the method we are testing
            var people = PersonRepository.GetPeople(null, 1);

            // Assert
            // Finally, we check if the returned list of persons matches the ones we have added
            Assert.AreEqual(2, people.Results.Count);
            Assert.Contains(person1, people.Results.ToList());
            Assert.Contains(person2, people.Results.ToList());
        }

        [Test]
        // This test checks whether UpdatePerson method updates a person in the database correctly
        public async Task UpdatePerson_SuccessfullyUpdatesPerson()
        {
            // Arrange
            // We start by creating a person and adding it to the database
            var originalPerson = new Person { PersonId = 1, FirstName = "Original", LastName = "User", Addresses = new List<Address>() , PhoneNumber = "123456"};
            await AppDbContext.People.AddAsync(originalPerson);
            await AppDbContext.SaveChangesAsync();

            // Here we prepare a person with updated data
            var updatedPerson = new Person { PersonId = 1, FirstName = "Updated", LastName = "User", Addresses = new List<Address>(), PhoneNumber = "123456" };

            // Act
            // Then we call the method we are testing
            var result = await PersonRepository.UpdatePerson(updatedPerson);

            // Assert
            // Finally, we check if the person was updated successfully
            Assert.NotNull(result);
            Assert.AreEqual(updatedPerson.PersonId, result.PersonId);
            Assert.AreEqual(updatedPerson.FirstName, result.FirstName);
            Assert.AreEqual(updatedPerson.LastName, result.LastName);
        }
    }
}
