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
    public class CreditPartnerAPIControllerPutTest
    {
        private CreditPartnerController _controller;
        private ICreditPartnerRepository _repository;
        private IMapper _mapper;
        private CreditPartnerChangeEndDateIncomingDto _dto;
        private DateTime _endDate;

        [TestInitialize]
        public void Init()
        {
            _repository = Substitute.For<ICreditPartnerRepository>();
            _mapper = Substitute.For<IMapper>();
            _controller = new CreditPartnerController(_repository, _mapper);
            _endDate = new DateTime(2020, 10, 10);
            _dto = new CreditPartnerChangeEndDateIncomingDto() { EndDate = _endDate };
        }

        [DataRow("1")]
        [TestMethod]
        public void PutOkTest(string token)
        {
            //arrange
            var fromDb = new CreditPartner() { IdNumber = 1 };
            _repository.GetCreditPartnerByTokenAsync(Arg.Any<string>()).Returns(Task.FromResult(fromDb));
            _repository.SaveChangesAsync().Returns(1);

            var output = new CreditPartnerFullInfoDto() { EndDate = _endDate };

            _mapper.Map(Arg.Any<CreditPartnerChangeEndDateIncomingDto>(), Arg.Any<CreditPartner>()).Returns(fromDb);
            _mapper.Map<CreditPartnerFullInfoDto>(Arg.Any<CreditPartner>())
                .Returns(output);

            //act
            var res = _controller.Put(token, _dto).Result;

            //assert
            //specifikovat na non default
            //repo receveived s id 0, specific ID
            res.Result.ShouldBeOfType<OkObjectResult>();
            (res.Result as OkObjectResult).Value.ShouldBeOfType<CreditPartnerFullInfoDto>();
            ((res.Result as OkObjectResult).Value as CreditPartnerFullInfoDto).EndDate.ShouldBe(_endDate);

            _repository.Received().GetCreditPartnerByTokenAsync(token);
            _repository.Received().SaveChangesAsync();

            _mapper
                .Received()
                .Map(_dto, fromDb);

            _mapper
                .Received()
                .Map<CreditPartnerFullInfoDto>(fromDb);
        }

        [DataRow("1")]
        [TestMethod]
        public void PutNotFoundTest(string token)
        {
            //arrange
            _repository.GetCreditPartnerByTokenAsync(Arg.Any<string>()).Returns(Task.FromResult<CreditPartner>(null));

            //act
            var res = _controller.Put(token, _dto).Result;

            //assert
            res.Result.ShouldBeOfType<NotFoundResult>();

            _repository.Received().GetCreditPartnerByTokenAsync(token);
        }

        [DataRow("1", 2)]
        [DataRow("1", 3)]
        [TestMethod]
        public void PutSaveChangeErrorTest(string token, int changes)
        {
            //arrange
            var fromDb = new CreditPartner() { IdNumber = 1 };
            _repository.GetCreditPartnerByTokenAsync(Arg.Any<string>()).Returns(Task.FromResult(fromDb));
            _repository.SaveChangesAsync().Returns(changes);

            _mapper.Map(Arg.Any<CreditPartnerChangeEndDateIncomingDto>(), Arg.Any<CreditPartner>()).Returns(fromDb);

            //act
            var res = _controller.Put(token, _dto).Result;

            //assert
            res.Result.ShouldBeOfType<BadRequestObjectResult>();

            _repository.Received().GetCreditPartnerByTokenAsync(token);
            _mapper
                .Received()
                .Map(_dto, fromDb);
            _repository.Received().SaveChangesAsync();
        }

        [DataRow("1")]
        [TestMethod]
        public void PutInternalServerErrorTest(string token)
        {
            //arrange
            _repository.GetCreditPartnerByTokenAsync(Arg.Any<string>()).Throws(new Exception());

            //act
            var res = _controller.Put(token, _dto).Result;

            //assert
            (res.Result as ObjectResult).StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);

            _repository.Received().GetCreditPartnerByTokenAsync(token);
        }
    }
}
