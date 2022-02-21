using AutoMapper;
using EntityFrameworkCore.Testing.NSubstitute;
using HatcheryFinal_Web_API.Controllers;
using HatcheryFinal_Web_API.Data;
using HatcheryFinal_Web_API.Data.Dto;
using HatcheryFinal_Web_API.Data.Entities;
using HatcheryFinal_Web_API.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
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
        public void TestDeleteOk()
        {
            //arrange
            var repository = Substitute.For<ICreditPartnerRepository>();
            repository.GetCreditPartnerByIdAsync(0).Returns(new CreditPartner());
            repository.SaveChangesAsync().Returns(1);

            var mapper = Substitute.For<IMapper>();

            var controller = new CreditPartnerController(repository, mapper);

            //act
            var res = controller.Delete(0);

            //assert
            Assert.IsTrue(res.Result is OkResult);
        }

        /*[TestMethod]
        public async void TestDelete()
        {
            var expected = new CreditPartnerFullInfoDto();

            var mockedDbContext = Create.MockedDbContextFor<BankDbContext>();
            mockedDbContext.Set<CreditPartner>();

            var mapper = Substitute.For<IMapper>();
            mapper.Map<CreditPartnerFullInfoDto, CreditPartner>(Arg.Any<CreditPartnerFullInfoDto>()).Returns(Arg.Any<CreditPartner>());

            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            var logger = loggerFactory.CreateLogger<CreditPartnerRepository>();

            var repository = new CreditPartnerRepository(mockedDbContext, logger);

            var controller = new CreditPartnerController(repository, mapper);



            var expectedDomainReturn = new DomainItem(0); //Illustrative purposes only
            mockDomain.Setup(x => x.DomainCall(0)).Returns(expectedDomainReturn); //Illustrative purposes only

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<DomainItem, ServiceItem>(It.IsAny<DomainItem>()))
                .Returns(expected);


            var service = new Service(mockDomain.Object, mockMapper.Object);
            var result = service.Get(0);

            mockDomain.Verify(x => x.DomainCall(0), Times.Once);
            mockMapper.Verify(x => x.Map<DomainItem, ServiceItem>(expectedDomainReturn), Times.Once);
        }*/
    }
}
