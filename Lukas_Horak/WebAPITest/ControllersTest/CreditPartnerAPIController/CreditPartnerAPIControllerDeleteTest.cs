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

namespace WebAPITest.ControllersTest.CreditPartnerAPIController
{
    [TestClass]
    public class CreditPartnerAPIControllerDeleteTest
    {
        private CreditPartnerController _controller;
        private ICreditPartnerRepository _repository;
        private IMapper _mapper;

        [TestInitialize]
        public void Init()
        {
            _repository = Substitute.For<ICreditPartnerRepository>();
            _mapper = Substitute.For<IMapper>();
            _controller = new CreditPartnerController(_repository, _mapper);
        }

        [DataRow(1)]
        [TestMethod]
        public void DeleteOkTest(int id)
        {
            //arrange
            var fromDb = new CreditPartner() { IdNumber = 1 };
            _repository.GetCreditPartnerByIdAsync(Arg.Any<int>()).Returns(Task.FromResult(fromDb));
            _repository.SaveChangesAsync().Returns(1);

            //act
            var res = _controller.Delete(id).Result;

            //assert
            res.ShouldBeOfType<OkResult>();

            _repository.Received().GetCreditPartnerByIdAsync(id);
            _repository.Received().Remove(fromDb);
            _repository.Received().SaveChangesAsync();
        }

        [DataRow(1)]
        [TestMethod]
        public void DeleteNotFoundTest(int id)
        {
            //arrange
            _repository.GetCreditPartnerByIdAsync(Arg.Any<int>()).Returns(Task.FromResult<CreditPartner>(null));

            //act
            var res = _controller.Delete(id).Result;

            //assert
            res.ShouldBeOfType<NotFoundResult>();
            _repository.Received().GetCreditPartnerByIdAsync(id);
            _repository.Received(0).SaveChangesAsync();
        }

        [DataRow(1, 0)]
        [DataRow(1, 2)]
        [TestMethod]
        public void DeleteSaveChangeErrorTest(int id, int changes)
        {
            //arrange
            var fromDb = new CreditPartner() { IdNumber = 1 };
            _repository.GetCreditPartnerByIdAsync(Arg.Any<int>()).Returns(Task.FromResult(fromDb));
            _repository.SaveChangesAsync().Returns(changes);

            //act
            var res = _controller.Delete(id).Result;

            //assert
            res.ShouldBeOfType<BadRequestObjectResult>();
            _repository.Received().GetCreditPartnerByIdAsync(id);
            _repository.Received().Remove(fromDb);
            _repository.Received().SaveChangesAsync();
        }

        [DataRow(1)]
        [TestMethod]
        public void DeleteInternalServerErrorTest(int id)
        {
            //arrange
            _repository.GetCreditPartnerByIdAsync(Arg.Any<int>()).Throws(new System.Exception());

            //act
            var res = _controller.Delete(id).Result;

            //assert
            (res as ObjectResult).StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);

            _repository.Received().GetCreditPartnerByIdAsync(id);
            _repository.Received(0).SaveChangesAsync();
        }
    }
}
