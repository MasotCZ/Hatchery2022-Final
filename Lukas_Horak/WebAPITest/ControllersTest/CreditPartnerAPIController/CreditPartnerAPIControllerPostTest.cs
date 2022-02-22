using AutoMapper;
using HatcheryFinal_Web_API.Controllers;
using HatcheryFinal_Web_API.Data.Dto;
using HatcheryFinal_Web_API.Data.Entities;
using HatcheryFinal_Web_API.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using System;
using System.Threading.Tasks;

namespace WebAPITest.ControllersTest.CreditPartnerAPIController
{
    [TestClass]
    public class CreditPartnerAPIControllerPostTest
    {
        private CreditPartnerController _controller;
        private ICreditPartnerRepository _repository;
        private IMapper _mapper;
        private CreditPartnerFullInfoDto _dto;

        [TestInitialize]
        public void Init()
        {
            _repository = Substitute.For<ICreditPartnerRepository>();
            _mapper = Substitute.For<IMapper>();
            _controller = new CreditPartnerController(_repository, _mapper);
            _dto = new CreditPartnerFullInfoDto() { IdNumber = 1 };
        }

        [TestMethod]
        public void PostOkTest()
        {
            //arrange
            _repository.GetCreditPartnerByIdAsync(Arg.Any<int>()).Returns(Task.FromResult<CreditPartner>(null));
            _repository.SaveChangesAsync().Returns(1);

            var toDb = new CreditPartner() { IdNumber = 1 };
            var output = new CreditPartnerRegisteredDto() { Token = "10" };

            _mapper.Map<CreditPartner>(Arg.Any<CreditPartnerFullInfoDto>())
                .Returns(toDb);
            _mapper.Map<CreditPartnerRegisteredDto>(Arg.Any<CreditPartner>())
                .Returns(output);

            //act
            var res = _controller.Post(_dto).Result;

            //assert
            res.Result.ShouldBeOfType<OkObjectResult>();
            (res.Result as OkObjectResult).Value.ShouldBeOfType<CreditPartnerRegisteredDto>();
            ((res.Result as OkObjectResult).Value as CreditPartnerRegisteredDto).Token.ShouldBe("10");

            _repository.Received().GetCreditPartnerByIdAsync(1);
            _mapper.Received().Map<CreditPartner>(_dto);
            _repository.Received().Add(toDb);
            _mapper.Received().Map<CreditPartnerRegisteredDto>(toDb);
        }

        [TestMethod]
        public void PostAlreadyRegisteredTest()
        {
            //arrange
            _repository.GetCreditPartnerByIdAsync(Arg.Any<int>()).Returns(Task.FromResult(new CreditPartner()));

            //act
            var res = _controller.Post(_dto).Result;

            //assert
            //Assert.IsTrue(res.Result is NotFoundObjectResult, $"status is: {(res.Result as StatusCodeResult).StatusCode}");
            res.Result.ShouldBeOfType<BadRequestObjectResult>();

            _repository.Received().GetCreditPartnerByIdAsync(1);
        }

        [DataRow(0)]
        [DataRow(2)]
        [TestMethod]
        public void PostSaveChangeErrorTest(int changes)
        {
            //arrange
            _repository.GetCreditPartnerByIdAsync(Arg.Any<int>()).Returns(Task.FromResult<CreditPartner>(null));
            _repository.SaveChangesAsync().Returns(changes);

            var toDb = new CreditPartner() { IdNumber = 1 };

            _mapper.Map<CreditPartner>(Arg.Any<CreditPartnerFullInfoDto>())
                .Returns(toDb);

            //act
            var res = _controller.Post(_dto).Result;

            //assert
            res.Result.ShouldBeOfType<BadRequestObjectResult>();

            _repository.Received().GetCreditPartnerByIdAsync(1);
            _mapper.Received().Map<CreditPartner>(_dto);
            _repository.Received().Add(toDb);
        }

        [TestMethod]
        public void PostInternalServerErrorTest()
        {
            //arrange
            _repository.GetCreditPartnerByIdAsync(Arg.Any<int>()).Throws(new System.Exception());

            //act
            var res = _controller.Post(_dto).Result;

            //assert
            (res.Result as ObjectResult).StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);

            _repository.Received().GetCreditPartnerByIdAsync(1);
        }
    }
}
