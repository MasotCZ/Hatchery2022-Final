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
using System.Threading.Tasks;

namespace WebAPITest.ControllersTest.CreditRequestAPIController
{
    [TestClass]
    public class CreditRequestAPIControllerGetTest
    {
        private CreditRequestController _controller;
        private ICreditRequestRepository _requestRepository;
        private ICreditPartnerRepository _partnerRepository;
        private IMapper _mapper;
        private CreditRequest[] _fromDb;
        private CreditRequestOutgoingWithIdDto[] _output;

        [TestInitialize]
        public void Init()
        {
            _requestRepository = Substitute.For<ICreditRequestRepository>();
            _mapper = Substitute.For<IMapper>();

            _controller = new CreditRequestController(_requestRepository, _partnerRepository, _mapper);

            _fromDb = new CreditRequest[]
            {
                new CreditRequest() { Id = 0 , Name = "ok"},
                new CreditRequest() { Id = 1 , Name = "ok"},
                new CreditRequest() { Id = 2 , Name = "ok"},
                new CreditRequest() { Id = 3 , Name = "ok"}
            };

            _output = new CreditRequestOutgoingWithIdDto[]
            {
                new CreditRequestOutgoingWithIdDto() { Name = "ok"},
                new CreditRequestOutgoingWithIdDto() { Name = "ok"},
                new CreditRequestOutgoingWithIdDto() { Name = "ok"},
                new CreditRequestOutgoingWithIdDto() { Name = "ok"}
            };
        }

        [TestMethod]
        public void GetOkTest()
        {
            //arrange
            _requestRepository.GetAllUnfulfilledActiveCreditRequestsAsync().Returns(Task.FromResult(_fromDb));
            _mapper.Map<CreditRequestOutgoingWithIdDto[]>(Arg.Any<CreditRequest[]>()).Returns(_output);

            //act
            var res = _controller.Get().Result;

            //assert
            _requestRepository.Received().GetAllUnfulfilledActiveCreditRequestsAsync();
            _mapper.Received().Map<CreditRequestOutgoingWithIdDto[]>(_fromDb);

            res.Result.ShouldBeOfType<OkObjectResult>();
            (res.Result as OkObjectResult).Value.ShouldBeOfType<CreditRequestOutgoingWithIdDto[]>();
            ((res.Result as OkObjectResult).Value as CreditRequestOutgoingWithIdDto[]).ShouldBe(_output);
        }

        [TestMethod]
        public void GetInternalServerErrorTest()
        {
            //arrange
            _requestRepository.GetAllUnfulfilledActiveCreditRequestsAsync().Throws(new System.Exception());

            //act
            var res = _controller.Get().Result;

            //assert
            _requestRepository.Received().GetAllUnfulfilledActiveCreditRequestsAsync();

            (res.Result as ObjectResult).StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
        }
    }
}
