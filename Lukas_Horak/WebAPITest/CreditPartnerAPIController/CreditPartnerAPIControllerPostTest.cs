using AutoMapper;
using EntityFrameworkCore.Testing.NSubstitute;
using HatcheryFinal_Web_API.Controllers;
using HatcheryFinal_Web_API.Data;
using HatcheryFinal_Web_API.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPITest.CreditPartnerAPIController
{
    [TestClass]
    internal class CreditPartnerAPIControllerPostTest
    {

    }

    [TestClass]
    internal class CreditPartnerAPIControllerPutTest
    {

    }

    [TestClass]
    internal class CreditPartnerAPIControllerDeleteTest
    {
        [TestMethod]
        public async void TestDelete()
        {
            //arrange
            var context = Create.MockedDbContextFor<BankDbContext>();

            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            var logger = loggerFactory.CreateLogger<CreditPartnerRepository>();
            var repo = new CreditPartnerRepository(context, logger);
            var mapper = new Mapper(new BankMapperProfile());
                


            var controller = new CreditPartnerController(repo,  );

            repo.Add(new  );

            var expected = new CreditPartner();
            

            //act

            await controller.Delete(111);

            //assert
            context.ver
        }
    }
}
