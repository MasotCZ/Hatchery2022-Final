using AutoMapper;
using HatcheryFinal_Web_API.Data;
using HatcheryFinal_Web_API.Data.Entities;
using HatcheryFinal_Web_API.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Shouldly;
using System;

namespace WebAPITest.RepositoriesTest.RepositoryBaseTest
{
    [TestClass]
    public class RepositoryBaseTest
    {
        private BankDbContext _context;
        private ICreditPartnerRepository _repository;

        [TestInitialize]
        public void Init()
        {
            var dbContextOptions = new DbContextOptionsBuilder<BankDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            //_context = Create.MockedDbContextFor<BankDbContext>(dbContextOptions, null);
            _context = Substitute.For<BankDbContext>(dbContextOptions, null);
            _repository = new CreditPartnerRepository(_context, NullLogger<CreditPartnerRepository>.Instance);
        }

        [TestMethod]
        public void AddTest()
        {
            //arrange
            var toAdd = new CreditPartner() { IdNumber = 1 };

            //act
            _repository.Add(toAdd);

            //assert
            _context.Received().Add(toAdd);
        }

        [TestMethod]
        public void DeleteTest()
        {
            //arrange
            var toDelete = new CreditPartner() { IdNumber = 1 };

            //act
            _repository.Remove(toDelete);

            //assert
            _context.Received().Remove(toDelete);
        }

        [DataRow(20)]
        [TestMethod]
        public void SaveChangesOkTest(int changes)
        {
            //arrange
            _context.SaveChangesAsync().Returns(changes);

            //act
            var res = _repository.SaveChangesAsync().Result;

            //assert
            _context.Received().SaveChangesAsync();

            res.ShouldBe(changes);
        }

    }
}
