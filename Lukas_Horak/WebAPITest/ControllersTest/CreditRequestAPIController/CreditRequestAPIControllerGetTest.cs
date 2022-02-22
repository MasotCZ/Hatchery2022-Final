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
        private ICreditRequestRepository _repository;
        private IMapper _mapper;
        private CreditRequest[] _fromDb;
        private CreditRequestDto[] _output;

        [TestInitialize]
        public void Init()
        {
            _repository = Substitute.For<ICreditRequestRepository>();
            _mapper = Substitute.For<IMapper>();

            _controller = new CreditRequestController(_repository, _mapper);

            _fromDb = new CreditRequest[]
            {
                new CreditRequest() { Id = 0 , Name = "ok"},
                new CreditRequest() { Id = 1 , Name = "ok"},
                new CreditRequest() { Id = 2 , Name = "ok"},
                new CreditRequest() { Id = 3 , Name = "ok"}
            };

            _output = new CreditRequestDto[]
            {
                new CreditRequestDto() { Name = "ok"},
                new CreditRequestDto() { Name = "ok"},
                new CreditRequestDto() { Name = "ok"},
                new CreditRequestDto() { Name = "ok"}
            };
        }

        [TestMethod]
        public void GetOkTest()
        {
            //arrange
            _repository.GetAllUnfulfilledCreditRequestsAsync().Returns(Task.FromResult(_fromDb));
            _mapper.Map<CreditRequestDto[]>(Arg.Any<CreditRequest[]>()).Returns(_output);

            //act
            var res = _controller.Get().Result;

            //assert
            _repository.Received().GetAllUnfulfilledCreditRequestsAsync();
            _mapper.Received().Map<CreditRequestDto[]>(_fromDb);

            res.Result.ShouldBeOfType<OkObjectResult>();
            (res.Result as OkObjectResult).Value.ShouldBeOfType<CreditRequestDto[]>();
            ((res.Result as OkObjectResult).Value as CreditRequestDto[]).ShouldBe(_output);
        }

        [TestMethod]
        public void GetInternalServerErrorTest()
        {
            //arrange
            _repository.GetAllUnfulfilledCreditRequestsAsync().Throws(new System.Exception());

            //act
            var res = _controller.Get().Result;

            //assert
            _repository.Received().GetAllUnfulfilledCreditRequestsAsync();

            (res.Result as ObjectResult).StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
        }
    }
}
