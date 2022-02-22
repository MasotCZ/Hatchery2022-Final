using AutoMapper;
using EntityFrameworkCore.Testing.NSubstitute;
using HatcheryFinal_Web_API.Controllers;
using HatcheryFinal_Web_API.Data;
using HatcheryFinal_Web_API.Data.Dto;
using HatcheryFinal_Web_API.Data.Entities;
using HatcheryFinal_Web_API.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using System.Threading.Tasks;

namespace WebAPITest.RepositoriesTest.CreditPartnerRepositoryTest
{
    [TestClass]
    public class CreditPartnerRepositoryGetCreditPartnerByIdAsyncTest
    {
        private BankDbContext _context;
        private ILogger<CreditPartnerRepository> _logger;
        private ICreditPartnerRepository _repository;

        [TestInitialize]
        public void Init()
        {
            _context = Create.MockedDbContextFor<BankDbContext>();

            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            var _logger = loggerFactory.CreateLogger<CreditPartnerRepository>();

            _repository = new CreditPartnerRepository(_context, _logger);
        }

        public void AddOkTest() { }
        public void DeleteOkTest() { }
        public void DeleteNotFoundTest() { }

        [TestMethod]
        public void SaveChangesOkTest()
        {
            //arrange
            var toAdd = new CreditPartner() { IdNumber = 1 };

            //act
            _repository.Add(toAdd);

            //assert
            _context.Received().Add(toAdd);
        }

        public void GetCrediPartnerByIdAsyncTest()
        {
            //arrange


            //act


            //assert
        }
    }
}
