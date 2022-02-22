using EntityFrameworkCore.Testing.NSubstitute;
using HatcheryFinal_Web_API.Data;
using HatcheryFinal_Web_API.Data.Entities;
using HatcheryFinal_Web_API.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        private BankDbContext _context;
        private ICreditPartnerRepository _repository;
        private DbSet<CreditPartner> _data;

        private DbContextOptions<BankDbContext> _options;
        private IConfiguration _config;

        [TestInitialize]
        public void Init()
        {
            //----new

            _options = new DbContextOptionsBuilder<BankDbContext>()
                .UseInMemoryDatabase(databaseName: "BankDb").Options;

            //----old

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>()
                {
                    { "ConnectionStringDefault", "BankDb"}
                })
                .Build();

            //var dbContextOptions = new DbContextOptionsBuilder<BankDbContext>().Options;
            //.UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            //_context = Create.MockedDbContextFor<BankDbContext>(dbContextOptions, config);
            //_context = Substitute.For<IBankDbContext>() as BankDbContext;

            //_context = Substitute.For<BankDbContext>(dbContextOptions, null);
            //_repository = new CreditPartnerRepository(_context, NullLogger<CreditPartnerRepository>.Instance);

            //_data = (new List<CreditPartner>()
            //{
            //    new CreditPartner(){ IdNumber = 1, Requests = new List<CreditRequest>() { new CreditRequest() { Id = 1} } },
            //    new CreditPartner(){ IdNumber = 2, Requests = new List<CreditRequest>() { new CreditRequest() { Id = 2} } },
            //    new CreditPartner(){ IdNumber = 3, Requests = new List<CreditRequest>() { new CreditRequest() { Id = 3} } },
            //}).AsQueryable() as DbSet<CreditPartner>;
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public void GetCreditPartnerByIdAsyncReturnsDataTest(int id)
        {
            //arrange
            //data
            //_context.CreditPartners.Returns(_data);

            using (var context = new BankDbContext(_options, _config))
            {
                context.CreditPartners.Add(
                new CreditPartner() { IdNumber = 1, Requests = new List<CreditRequest>() { new CreditRequest() { Id = 1 } } }
                );
                context.CreditPartners.Add(
                new CreditPartner() { IdNumber = 2, Requests = new List<CreditRequest>() { new CreditRequest() { Id = 2 } } }
                );
                context.CreditPartners.Add(
                new CreditPartner() { IdNumber = 2, Requests = new List<CreditRequest>() { new CreditRequest() { Id = 3 } } }
                );

                context.SaveChanges();
            }

            CreditPartner res;
            //act
            using (var context = new BankDbContext(_options, _config))
            {
                _repository = new CreditPartnerRepository(context, NullLogger<CreditPartnerRepository>.Instance);

                res = _repository.GetCreditPartnerByIdAsync(id).Result;
            }


            //assert
            _ = _context.Received().CreditPartners;

            res.IdNumber.ShouldBe(id);
            res.Requests.ShouldBe(null);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public void GetCreditPartnerByIdAsyncReturnsDataIncludingRequestsTest(int id)
        {
            //arrange
            //data
            _context.CreditPartners.Returns(_data);

            //act
            var res = _repository.GetCreditPartnerByIdAsync(id, true).Result;

            //assert
            _ = _context.Received().CreditPartners;

            res.IdNumber.ShouldBe(id);
            res.Requests.ShouldNotBe(null);
        }

        [TestMethod]
        [DataRow(0)]
        public void GetCreditPartnerByIdAsyncReturnsNoDataTest(int id)
        {
            //arrange
            //data
            _context.CreditPartners.Returns(_data);

            //act
            var res = _repository.GetCreditPartnerByIdAsync(id).Result;

            //assert
            _ = _context.Received().CreditPartners;

            res.ShouldBe(null);
        }

        [TestMethod]
        [DataRow(0)]
        public void GetCreditPartnerByIdAsyncReturnsNoDataIncludingRequestsTest(int id)
        {
            //arrange
            //data
            _context.CreditPartners.Returns(_data);

            //act
            var res = _repository.GetCreditPartnerByIdAsync(id, true).Result;

            //assert
            _ = _context.Received().CreditPartners;

            res.ShouldBe(null);
        }

        [TestMethod]
        [DataRow(1)]
        public void GetCrediPartnerByIdAsyncReturnsDataIncludingRequestsWithNoRequestsTest(int id)
        {
            //arrange
            //data
            _context.CreditPartners.Returns(
                    (new List<CreditPartner>()
                {
                    new CreditPartner(){ IdNumber = 1},
                }).AsQueryable() as DbSet<CreditPartner>);

            //act
            var res = _repository.GetCreditPartnerByIdAsync(id, true).Result;

            //assert
            _ = _context.Received().CreditPartners;

            res.IdNumber.ShouldBe(1);
            res.Requests.ShouldBe(null);
        }
    }
}
