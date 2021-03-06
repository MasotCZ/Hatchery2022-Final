using EntityFrameworkCore.Testing.NSubstitute;
using HatcheryFinal_Web_API.Data;
using HatcheryFinal_Web_API.Data.Entities;
using HatcheryFinal_Web_API.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebAPITest.RepositoriesTest.CreditPartnerRepositoryTest
{

    [TestClass]
    public class CreditPartnerRepositoryGetCreditPartnerByIdAsyncTest
    {
        private Mock<IBankDbContext> _context;
        private ICreditPartnerRepository _repository;
        private Mock<DbSet<CreditPartner>> _mockSet;
        private IConfiguration _config;
        private DbContextOptions? _options;

        [TestInitialize]
        public void Init()
        {
            //----new

            _context = new Mock<IBankDbContext>();
            _repository = new CreditPartnerRepository(_context.Object, NullLogger<CreditPartnerRepository>.Instance);

            //probly has cache
            _options = new DbContextOptionsBuilder<BankDbContext>()
                .UseInMemoryDatabase(databaseName: "Test").Options;

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>()
                {
                    { "ConnectionStrings:BankDb", "Test"}
                })
                .Build();

            using (var context = new BankDbContext(_options, _config))
            {
                context.CreditPartners.Add(
                new CreditPartner() { IdNumber = 1, Name = "one", StartDate = DateTime.Now, Requests = new List<CreditRequest>() { new CreditRequest() { Id = 1, Phone = "1" } } }
                );
                context.CreditPartners.Add(
                new CreditPartner() { IdNumber = 2, Name = "two", StartDate = DateTime.Now, Requests = new List<CreditRequest>() { new CreditRequest() { Id = 2, Phone = "2" } } }
                );
                context.CreditPartners.Add(
                new CreditPartner() { IdNumber = 3, Name = "three", StartDate = DateTime.Now, Requests = new List<CreditRequest>() { new CreditRequest() { Id = 3, Phone = "3" } } }
                );

                var ok = context.CreditPartners.ToArray();

                context.SaveChanges();
            }

            //var ok = Substitute.For<IBankDbContext>();
            //_repository = new CreditPartnerRepository(_context as BankDbContext, NullLogger<CreditPartnerRepository>.Instance);

            //var dbContextOptions = new DbContextOptionsBuilder<BankDbContext>().Options;
            //.UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            //_context = Create.MockedDbContextFor<BankDbContext>(dbContextOptions, config);
            //_context = Substitute.For<IBankDbContext>() as BankDbContext;

            //_context = Substitute.For<BankDbContext>(dbContextOptions, null);
            //_repository = new CreditPartnerRepository(_context, NullLogger<CreditPartnerRepository>.Instance);

            //var data = (new List<CreditPartner>()
            //{
            //    new CreditPartner(){ IdNumber = 1, Requests = new List<CreditRequest>() { new CreditRequest() { Id = 1} } },
            //    new CreditPartner(){ IdNumber = 2, Requests = new List<CreditRequest>() { new CreditRequest() { Id = 2} } },
            //    new CreditPartner(){ IdNumber = 3, Requests = new List<CreditRequest>() { new CreditRequest() { Id = 3} } },
            //}).AsQueryable();
            //_mockSet = Utils.CreateMockDbSetMoq(data);
        }

        [TestCleanup]
        public void CleanUp()
        {
            using (var context = new BankDbContext(_options, _config))
            {
                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public void GetCreditPartnerByIdAsyncReturnsDataTest(int id)
        {
            ////arrange
            //_context.CreditPartners.Returns(_mockSet);

            ////act
            //var res = _repository.GetCreditPartnerByIdAsync(id).Result;

            ////assert
            //res.IdNumber.ShouldBe(id);
            //res.Requests.ShouldBe(null);

            //InMemory
            using (var context = new BankDbContext(_options, _config))
            {
                var repo = new CreditPartnerRepository(context, NullLogger<CreditPartnerRepository>.Instance);
                var rr = repo.GetCreditPartnerByIdAsync(id).Result;
            }

            ////MOQ
            ////arrange
            //_context.Setup(m => m.CreditPartners).Returns(_mockSet.Object);

            ////act
            //var res = _repository.GetCreditPartnerByIdAsync(id).Result;

            //assert
            //mockSet.Verify(m => m.Add(It.IsAny<CreditPartner>()), Times.Once());
            //mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        [DataRow(1)]
        public void GetCreditPartnerByIdAsyncReturnsDataIncludingRequestsTest(int id)
        {
            ////arrange
            ////data
            //_context.CreditPartners.Returns(_mockSet);

            ////act
            //var res = _repository.GetCreditPartnerByIdAsync(id, true).Result;

            ////assert
            //_ = _context.Received().CreditPartners;

            //res.IdNumber.ShouldBe(id);
            //res.Requests.ShouldNotBe(null);
        }

        [TestMethod]
        [DataRow(0)]
        public void GetCreditPartnerByIdAsyncReturnsNoDataTest(int id)
        {
            ////arrange
            ////data
            //_context.CreditPartners.Returns(_mockSet);

            ////act
            //var res = _repository.GetCreditPartnerByIdAsync(id).Result;

            ////assert
            //_ = _context.Received().CreditPartners;

            //res.ShouldBe(null);
        }

        [TestMethod]
        [DataRow(0)]
        public void GetCreditPartnerByIdAsyncReturnsNoDataIncludingRequestsTest(int id)
        {
            ////arrange
            ////data
            //_context.CreditPartners.Returns(_mockSet);

            ////act
            //var res = _repository.GetCreditPartnerByIdAsync(id, true).Result;

            ////assert
            //_ = _context.Received().CreditPartners;

            //res.ShouldBe(null);
        }

        [TestMethod]
        [DataRow(1)]
        public void GetCrediPartnerByIdAsyncReturnsDataIncludingRequestsWithNoRequestsTest(int id)
        {
            ////arrange
            ////data
            //_context.CreditPartners.Returns(
            //        (new List<CreditPartner>()
            //    {
            //        new CreditPartner(){ IdNumber = 1},
            //    }).AsQueryable() as DbSet<CreditPartner>);

            ////act
            //var res = _repository.GetCreditPartnerByIdAsync(id, true).Result;

            ////assert
            //_ = _context.Received().CreditPartners;

            //res.IdNumber.ShouldBe(1);
            //res.Requests.ShouldBe(null);
        }
    }
}
