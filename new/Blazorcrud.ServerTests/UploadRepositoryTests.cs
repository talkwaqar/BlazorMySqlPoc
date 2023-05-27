using Blazorcrud.Shared.Models;
using NUnit.Framework;

namespace Blazorcrud.ServerTests
{
    public class UploadRepositoryTests : RepositoryTestBase
    {
        [Test]
        // This test checks whether GetUpload method returns the correct upload based on the provided id
        public async Task GetUpload_ReturnsCorrectUpload()
        {
            // Arrange: Setting up the expected upload in the database.
            var expectedUpload = new Upload { Id = 1, FileName = "TestFile.txt", FileContent = "TestContent" };
            await AppDbContext.Uploads.AddAsync(expectedUpload);
            await AppDbContext.SaveChangesAsync();

            // Act: Calling GetUpload method with the expected upload's ID.
            var actualUpload = await UploadRepository.GetUpload(expectedUpload.Id);

            // Assert: Verifying that the returned upload matches the expected upload.
            Assert.NotNull(actualUpload);
            Assert.AreEqual(expectedUpload.Id, actualUpload.Id);
            Assert.AreEqual(expectedUpload.FileName, actualUpload.FileName);
            Assert.AreEqual(expectedUpload.FileContent, actualUpload.FileContent);
        }

        [Test]
        // This test checks whether AddUpload method adds an upload to the database correctly
        public async Task AddUpload_SuccessfullyAddsUpload()
        {
            // Arrange: Setting up a new upload to be added.
            var newUpload = new Upload { FileName = "NewFile.txt", FileContent = "NewContent" };

            // Act: Calling AddUpload method with the new upload.
            var addedUpload = await UploadRepository.AddUpload(newUpload);

            // Assert: Verifying that the added upload matches the new upload.
            Assert.NotNull(addedUpload);
            Assert.AreEqual(newUpload.FileName, addedUpload.FileName);
            Assert.AreEqual(newUpload.FileContent, addedUpload.FileContent);
        }

        [Test]
        // This test checks whether DeleteUpload method deletes an upload from the database correctly
        public async Task DeleteUpload_SuccessfullyDeletesUpload()
        {
            // Arrange: Setting up an upload to be deleted.
            var uploadToDelete = new Upload { Id = 2, FileName = "DeleteFile.txt", FileContent = "DeleteContent" };
            await AppDbContext.Uploads.AddAsync(uploadToDelete);
            await AppDbContext.SaveChangesAsync();

            // Act: Calling DeleteUpload method with the upload's ID.
            var deletedUpload = await UploadRepository.DeleteUpload(uploadToDelete.Id);

            // Assert: Verifying that the deleted upload matches the upload to be deleted and the upload no longer exists in the database.
            Assert.NotNull(deletedUpload);
            Assert.AreEqual(uploadToDelete.Id, deletedUpload.Id);
            Assert.AreEqual(uploadToDelete.FileContent, deletedUpload.FileContent);

            var searchUpload = await AppDbContext.Uploads.FindAsync(deletedUpload.Id);
            Assert.IsNull(searchUpload);
        }

        [Test]
        // This test checks whether GetUploads method returns all uploads in the database correctly
        public async Task GetUploads_ReturnsCorrectUploads()
        {
            // Arrange: Setting up two uploads in the database.
            var upload1 = new Upload { Id = 1, FileName = "Test1.txt", FileContent = "TestContent1" };
            var upload2 = new Upload { Id = 2, FileName = "Test2.txt", FileContent = "TestContent2" };
            await AppDbContext.AddRangeAsync(upload1, upload2);
            await AppDbContext.SaveChangesAsync();

            // Act: Calling GetUploads method.
            var uploads = UploadRepository.GetUploads(null, 1);

            // Assert: Verifying that the returned uploads list contains both uploads.
            Assert.AreEqual(2, uploads.Results.Count);
            Assert.Contains(upload1, uploads.Results.ToList());
            Assert.Contains(upload2, uploads.Results.ToList());
        }

        [Test]
        // This test checks whether UpdateUpload method updates an upload in the database correctly
        public async Task UpdateUpload_SuccessfullyUpdatesUpload()
        {
            // Arrange: Setting up an original upload in the database.
            var originalUpload = new Upload { Id = 1, FileName = "Original.txt", FileContent = "OriginalContent" };
            await AppDbContext.Uploads.AddAsync(originalUpload);
            await AppDbContext.SaveChangesAsync();

            // Preparing the updated upload.
            var updatedUpload = new Upload { Id = 1, FileName = "Updated.txt", FileContent = "UpdatedContent" };

            // Act: Calling UpdateUpload method with the updated upload.
            var result = await UploadRepository.UpdateUpload(updatedUpload);

            // Assert: Verifying that the returned upload matches the updated upload.
            Assert.NotNull(result);
            Assert.AreEqual(updatedUpload.Id, result.Id);
            Assert.AreEqual(updatedUpload.FileName, result.FileName);
            Assert.AreEqual(updatedUpload.FileContent, result.FileContent);
        }
    }
}
