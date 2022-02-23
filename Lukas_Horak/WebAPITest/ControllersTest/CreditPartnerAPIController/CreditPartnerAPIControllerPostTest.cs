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
        private CreditPartnerFullInfoDto _dtoWithFutureEndDate;
        private CreditPartnerFullInfoDto _dtoWithPastEndDate;

        [TestInitialize]
        public void Init()
        {
            _repository = Substitute.For<ICreditPartnerRepository>();
            _mapper = Substitute.For<IMapper>();
            _controller = new CreditPartnerController(_repository, _mapper);
            _dtoWithFutureEndDate = new CreditPartnerFullInfoDto() { IdNumber = 1, EndDate = DateTime.Now.AddDays(1) };
            _dtoWithPastEndDate = new CreditPartnerFullInfoDto() { IdNumber = 1, EndDate = DateTime.Now.AddDays(-1) };
        }

        [TestMethod]
        public void PostOkTest()
        {
            //arrange
            _repository.GetCreditPartnerByIdAsync(Arg.Any<int>()).Returns(Task.FromResult<CreditPartner>(null));
            _repository.SaveChangesAsync().Returns(1);

            var fromDb = new CreditPartner() { IdNumber = 1 };
            var output = new CreditPartnerRegisteredDto() { Token = "10" };

            _mapper.Map<CreditPartner>(Arg.Any<CreditPartnerFullInfoDto>())
                .Returns(fromDb);
            _mapper.Map<CreditPartnerRegisteredDto>(Arg.Any<CreditPartner>())
                .Returns(output);

            //act
            var res = _controller.Post(_dtoWithFutureEndDate).Result;

            //assert
            res.Result.ShouldBeOfType<CreatedResult>();
            (res.Result as CreatedResult).Value.ShouldBeOfType<CreditPartnerRegisteredDto>();
            ((res.Result as CreatedResult).Value as CreditPartnerRegisteredDto).Token.ShouldBe(output.Token);

            _repository.Received().GetCreditPartnerByIdAsync(1);
            _mapper.Received().Map<CreditPartner>(_dtoWithFutureEndDate);
            _repository.Received().Add(fromDb);
            _mapper.Received().Map<CreditPartnerRegisteredDto>(fromDb);
        }

        [TestMethod]
        public void PostAlreadyRegisteredWhilePartnerIsInactiveTest()
        {
            //arrange
            var fromDb = new CreditPartner() { IdNumber = 1, EndDate = _dtoWithPastEndDate.EndDate };
            _repository.GetCreditPartnerByIdAsync(Arg.Any<int>()).Returns(Task.FromResult(fromDb));
            _repository.SaveChangesAsync().Returns(1);

            _mapper.Map(Arg.Any<CreditPartnerFullInfoDto>(), Arg.Any<CreditPartner>())
                .Returns(fromDb);

            var output = new CreditPartnerRegisteredDto() { Token = "10" };
            _mapper.Map<CreditPartnerRegisteredDto>(Arg.Any<CreditPartner>())
                .Returns(output);
            //act
            var res = _controller.Post(_dtoWithPastEndDate).Result;

            //assert
            //Assert.IsTrue(res.Result is NotFoundObjectResult, $"status is: {(res.Result as StatusCodeResult).StatusCode}");
            res.Result.ShouldBeOfType<OkObjectResult>();
            (res.Result as OkObjectResult).Value.ShouldBeOfType<CreditPartnerRegisteredDto>();
            ((res.Result as OkObjectResult).Value as CreditPartnerRegisteredDto).Token.ShouldBe(output.Token);

            _repository.Received().GetCreditPartnerByIdAsync(_dtoWithPastEndDate.IdNumber);
            _mapper.Received().Map(_dtoWithPastEndDate, fromDb);
            _mapper.Received().Map<CreditPartnerRegisteredDto>(fromDb);
        }

        [TestMethod]
        public void PostAlreadyRegisteredWhilePartnerIsActiveTest()
        {
            //arrange
            _repository.GetCreditPartnerByIdAsync(Arg.Any<int>()).Returns(Task.FromResult(new CreditPartner()));

            //act
            var res = _controller.Post(_dtoWithFutureEndDate).Result;

            //assert
            //Assert.IsTrue(res.Result is NotFoundObjectResult, $"status is: {(res.Result as StatusCodeResult).StatusCode}");
            res.Result.ShouldBeOfType<BadRequestObjectResult>();

            _repository.Received().GetCreditPartnerByIdAsync(1);
        }

        [DataRow(2)]
        [DataRow(3)]
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
            var res = _controller.Post(_dtoWithFutureEndDate).Result;

            //assert
            res.Result.ShouldBeOfType<BadRequestObjectResult>();

            _repository.Received().GetCreditPartnerByIdAsync(1);
            _mapper.Received().Map<CreditPartner>(_dtoWithFutureEndDate);
            _repository.Received().Add(toDb);
        }

        [TestMethod]
        public void PostInternalServerErrorTest()
        {
            //arrange
            _repository.GetCreditPartnerByIdAsync(Arg.Any<int>()).Throws(new System.Exception());

            //act
            var res = _controller.Post(_dtoWithFutureEndDate).Result;

            //assert
            (res.Result as ObjectResult).StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);

            _repository.Received().GetCreditPartnerByIdAsync(1);
        }
    }
}
