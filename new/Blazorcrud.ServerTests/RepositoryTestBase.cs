using Blazorcrud.Server.Authorization;
using Blazorcrud.Server.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorcrud.ServerTests
{

    public class RepositoryTestBase
    {
        protected AppDbContext AppDbContext { get; private set; }
        protected PersonRepository PersonRepository { get; private set; }
        protected UploadRepository UploadRepository { get; private set; }
        protected UserRepository UserRepository { get; private set; }
        protected Mock<IJwtUtils> MockJwtUtils { get; private set; }


        [SetUp]
        public void BaseSetup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;

            AppDbContext = new AppDbContext(options);
            PersonRepository = new PersonRepository(AppDbContext);
            UploadRepository = new UploadRepository(AppDbContext);
            MockJwtUtils = new Mock<IJwtUtils>();
            UserRepository = new UserRepository(AppDbContext, MockJwtUtils.Object);
        }

        [TearDown]
        public void BaseTearDown()
        {
            AppDbContext.Database.EnsureDeleted();
        }
    }
}
