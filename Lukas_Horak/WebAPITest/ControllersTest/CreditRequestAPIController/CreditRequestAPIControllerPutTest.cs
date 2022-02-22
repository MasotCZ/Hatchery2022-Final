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
        private ICreditRequestRepository _repository;
        private IMapper _mapper;
        private CreditRequest _fromDb;
        private CreditRequestDto _dto;

        [TestInitialize]
        public void Init()
        {
            _repository = Substitute.For<ICreditRequestRepository>();
            _mapper = Substitute.For<IMapper>();

            _controller = new CreditRequestController(_repository, _mapper);

            _fromDb = new CreditRequest() { Id = 1, Name = "yep" };
            _dto = new CreditRequestDto() { Name = "yep" };
        }

        [DataRow(1)]
        [TestMethod]
        public void PutOkTest(int id)
        {
            //arrange
            _repository.SaveChangesAsync().Returns(1);
            _repository.GetCreditRequestById(Arg.Any<int>()).Returns(Task.FromResult(_fromDb));
            _mapper.Map(Arg.Any<CreditRequestDto>(), Arg.Any<CreditRequest>()).Returns(_fromDb);
            _mapper.Map<CreditRequestDto>(Arg.Any<CreditRequest>()).Returns(_dto);

            //act
            var res = _controller.Put(id, _dto).Result;

            //assert
            _repository.Received().GetCreditRequestById(id);
            _repository.Received().SaveChangesAsync();
            _mapper.Received().Map(_dto, _fromDb);
            _mapper.Received().Map<CreditRequestDto>(_fromDb);

            res.Result.ShouldBeOfType<OkObjectResult>();
            (res.Result as OkObjectResult).Value.ShouldBeOfType<CreditRequestDto>();
            ((res.Result as OkObjectResult).Value as CreditRequestDto).Name.ShouldBe(_dto.Name);
        }

        [DataRow(1)]
        [TestMethod]
        public void PutNotFoundTest(int id)
        {
            //arrange
            _repository.GetCreditRequestById(Arg.Any<int>()).Returns(Task.FromResult<CreditRequest>(null));

            //act
            var res = _controller.Put(id, _dto).Result;

            //assert
            _repository.Received().GetCreditRequestById(id);

            res.Result.ShouldBeOfType<NotFoundResult>();
        }

        [DataRow(1)]
        [TestMethod]
        public void PutSaveChangeErrorTest(int id)
        {
            //arrange
            _repository.SaveChangesAsync().Returns(0);
            _repository.GetCreditRequestById(Arg.Any<int>()).Returns(Task.FromResult(_fromDb));
            _mapper.Map(Arg.Any<CreditRequestDto>(), Arg.Any<CreditRequest>()).Returns(_fromDb);

            //act
            var res = _controller.Put(id, _dto).Result;

            //assert
            _repository.Received().GetCreditRequestById(id);
            _repository.Received().SaveChangesAsync();
            _mapper.Received().Map(_dto, _fromDb);

            res.Result.ShouldBeOfType<BadRequestObjectResult>();
        }

        [DataRow(1)]
        [TestMethod]
        public void PutInternalServerErrorTest(int id)
        {
            //arrange
            _repository.GetCreditRequestById(id).Throws(new System.Exception());

            //act
            var res = _controller.Put(id, _dto).Result;

            //assert
            _repository.Received().GetCreditRequestById(id);

            (res.Result as ObjectResult).StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
        }
    }
}
