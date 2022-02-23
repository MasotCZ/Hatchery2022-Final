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
    public class CreditRequestAPIControllerPutTest
    {
        private CreditRequestController _controller;
        private ICreditRequestRepository _requestRepository;
        private ICreditPartnerRepository _partnerRepository;
        private IMapper _mapper;
        private CreditRequest _fromDb;
        private CreditRequestStatusChangeIncomingDto _dto;
        private CreditRequestDto _output;

        [TestInitialize]
        public void Init()
        {
            _requestRepository = Substitute.For<ICreditRequestRepository>();
            _mapper = Substitute.For<IMapper>();

            _controller = new CreditRequestController(_requestRepository, _partnerRepository, _mapper);

            _fromDb = new CreditRequest() { Id = 1, Name = "yep" };
            _dto = new CreditRequestStatusChangeIncomingDto() { ContactStatus = new CreditRequestStatusDto() { StatusCode = CreditRequestStatusCode.Accepted } };
            _output = new CreditRequestDto() { ContactStatus = _dto.ContactStatus };
        }

        [DataRow(1)]
        [TestMethod]
        public void PutOkTest(int id)
        {
            //arrange
            _requestRepository.SaveChangesAsync().Returns(1);
            _requestRepository.GetCreditRequestByIdAsync(Arg.Any<int>()).Returns(Task.FromResult(_fromDb));
            _mapper.Map(Arg.Any<CreditRequestStatusChangeIncomingDto>(), Arg.Any<CreditRequest>()).Returns(_fromDb);
            _mapper.Map<CreditRequestDto>(Arg.Any<CreditRequest>()).Returns(_output);

            //act
            var res = _controller.Put(id, _dto).Result;

            //assert
            _requestRepository.Received().GetCreditRequestByIdAsync(id);
            _requestRepository.Received().SaveChangesAsync();
            _mapper.Received().Map(_dto, _fromDb);
            _mapper.Received().Map<CreditRequestStatusChangeIncomingDto>(_fromDb);

            res.Result.ShouldBeOfType<OkObjectResult>();
            (res.Result as OkObjectResult).Value.ShouldBeOfType<CreditRequestDto>();
            ((res.Result as OkObjectResult).Value as CreditRequestDto).ShouldBe(_output);
        }

        [DataRow(1)]
        [TestMethod]
        public void PutNotFoundTest(int id)
        {
            //arrange
            _requestRepository.GetCreditRequestByIdAsync(Arg.Any<int>()).Returns(Task.FromResult<CreditRequest>(null));

            //act
            var res = _controller.Put(id, _dto).Result;

            //assert
            _requestRepository.Received().GetCreditRequestByIdAsync(id);

            res.Result.ShouldBeOfType<NotFoundResult>();
        }

        [DataRow(1)]
        [TestMethod]
        public void PutSaveChangeErrorTest(int id)
        {
            //arrange
            _requestRepository.SaveChangesAsync().Returns(3);
            _requestRepository.GetCreditRequestByIdAsync(Arg.Any<int>()).Returns(Task.FromResult(_fromDb));
            _mapper.Map(Arg.Any<CreditRequestStatusChangeIncomingDto>(), Arg.Any<CreditRequest>()).Returns(_fromDb);

            //act
            var res = _controller.Put(id, _dto).Result;

            //assert
            _requestRepository.Received().GetCreditRequestByIdAsync(id);
            _requestRepository.Received().SaveChangesAsync();
            _mapper.Received().Map(_dto, _fromDb);

            res.Result.ShouldBeOfType<BadRequestObjectResult>();
        }

        [DataRow(1)]
        [TestMethod]
        public void PutInternalServerErrorTest(int id)
        {
            //arrange
            _requestRepository.GetCreditRequestByIdAsync(id).Throws(new System.Exception());

            //act
            var res = _controller.Put(id, _dto).Result;

            //assert
            _requestRepository.Received().GetCreditRequestByIdAsync(id);

            (res.Result as ObjectResult).StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
        }
    }
}
