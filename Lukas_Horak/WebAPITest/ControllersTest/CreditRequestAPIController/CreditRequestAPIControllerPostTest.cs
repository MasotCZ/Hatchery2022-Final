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

namespace WebAPITest.ControllersTest.CreditRequestAPIController
{
    [TestClass]
    public class CreditRequestAPIControllerPostTest
    {
        private CreditRequestController _controller;
        private ICreditPartnerRepository _partnerRepository;
        private ICreditRequestRepository _requestRepository;
        private IMapper _mapper;
        private CreditRequestNewIncomingDto _dto;
        private CreditPartner _fromDb;
        private CreditRequestDto _output;

        [TestInitialize]
        public void Init()
        {
            _partnerRepository = Substitute.For<ICreditPartnerRepository>();
            _requestRepository = Substitute.For<ICreditRequestRepository>();
            _mapper = Substitute.For<IMapper>();
            _controller = new CreditRequestController(_requestRepository, _partnerRepository, _mapper);

            _dto = new CreditRequestNewIncomingDto() { Token = "22" };
            _fromDb = new CreditPartner() { Name = "kure" };
            _output = new CreditRequestDto() { Name = "ok" };
        }

        [TestMethod]
        public void PostOkTest()
        {
            //arrange
            _partnerRepository.GetActiveCreditPartnerByTokenAsync(Arg.Any<string>()).Returns(Task.FromResult(_fromDb));
            _requestRepository.SaveChangesAsync().Returns(2);

            var toDb = new CreditRequest();
            _mapper.Map<CreditRequest>(Arg.Any<CreditRequestDto>())
                .Returns(toDb);
            _mapper.Map<CreditRequestDto>(Arg.Any<CreditRequest>())
                .Returns(_output);

            //act
            var res = _controller.Post(_dto).Result;

            //assert
            res.Result.ShouldBeOfType<CreatedResult>();
            (res.Result as CreatedResult).Value.ShouldBeOfType<CreditRequestDto>();
            ((res.Result as CreatedResult).Value as CreditRequestDto).ShouldBe(_output);

            _partnerRepository.Received().GetActiveCreditPartnerByTokenAsync(_dto.Token);
            _mapper.Received().Map<CreditRequest>(_dto);
            _requestRepository.Received().Add(toDb);
            _requestRepository.Received().SaveChangesAsync();
            _mapper.Received().Map<CreditRequestDto>(toDb);
        }

        [TestMethod]
        public void PostNotRegisteredOrInactiveTest()
        {
            //arrange
            _partnerRepository.GetActiveCreditPartnerByTokenAsync(Arg.Any<string>()).Returns(Task.FromResult<CreditPartner>(null));

            //act
            var res = _controller.Post(_dto).Result;

            //assert
            //Assert.IsTrue(res.Result is NotFoundObjectResult, $"status is: {(res.Result as StatusCodeResult).StatusCode}");
            res.Result.ShouldBeOfType<BadRequestObjectResult>();

            _partnerRepository.Received().GetActiveCreditPartnerByTokenAsync(_dto.Token);
            _partnerRepository.Received(0).SaveChangesAsync();
        }

        [DataRow(0)]
        [DataRow(1)]
        [TestMethod]
        public void PostSaveChangeErrorTest(int changes)
        {
            //arrange
            _partnerRepository.GetActiveCreditPartnerByTokenAsync(Arg.Any<string>()).Returns(Task.FromResult(_fromDb));
            _requestRepository.SaveChangesAsync().Returns(changes);

            var toDb = new CreditRequest();
            _mapper.Map<CreditRequest>(Arg.Any<CreditRequestDto>())
                .Returns(toDb);

            //act
            var res = _controller.Post(_dto).Result;

            //assert
            res.Result.ShouldBeOfType<BadRequestObjectResult>();

            _partnerRepository.Received().GetActiveCreditPartnerByTokenAsync(_dto.Token);
            _mapper.Received().Map<CreditRequest>(_dto);
            _requestRepository.Received().Add(toDb);
            _requestRepository.Received().SaveChangesAsync();
        }

        [TestMethod]
        public void PostInternalServerErrorTest()
        {
            //arrange
            _partnerRepository.GetActiveCreditPartnerByTokenAsync(Arg.Any<string>()).Throws(new Exception());

            //act
            var res = _controller.Post(_dto).Result;

            //assert
            _partnerRepository.Received().GetActiveCreditPartnerByTokenAsync(_dto.Token);
            _partnerRepository.Received(0).SaveChangesAsync();

            res.Result.ShouldBeOfType<ObjectResult>();
            (res.Result as ObjectResult).StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
        }
    }
}
