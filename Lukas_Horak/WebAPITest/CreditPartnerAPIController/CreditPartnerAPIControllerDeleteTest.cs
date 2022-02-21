using AutoMapper;
using HatcheryFinal_Web_API.Controllers;
using HatcheryFinal_Web_API.Data.Entities;
using HatcheryFinal_Web_API.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using System.Net;
using System.Threading.Tasks;

namespace WebAPITest.CreditPartnerAPIController
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

        [TestMethod]
        public void TestDeleteOk()
        {
            //arrange
            _repository.GetCreditPartnerByIdAsync(0).Returns(new CreditPartner());
            _repository.SaveChangesAsync().Returns(1);

            //act
            var res = _controller.Delete(0).Result;

            //assert
            res.ShouldBeOfType<OkResult>();
        }

        [TestMethod]
        public void TestDeleteNotFound()
        {
            //arrange
            _repository.GetCreditPartnerByIdAsync(0).Returns(Task.FromResult<CreditPartner>(null));
            _repository.SaveChangesAsync().Returns(1);

            //act
            var res = _controller.Delete(0).Result;

            //assert
            //Assert.IsTrue(res.Result is NotFoundObjectResult, $"status is: {(res.Result as StatusCodeResult).StatusCode}");
            res.ShouldBeOfType<NotFoundResult>();
        }

        [DataRow(0)]
        [DataRow(2)]
        [TestMethod]
        public void TestDeleteSaveChangeError(int changes)
        {
            //arrange
            _repository.GetCreditPartnerByIdAsync(0).Returns(new CreditPartner());
            _repository.SaveChangesAsync().Returns(changes);

            //act
            var res = _controller.Delete(0).Result;

            //assert
            res.ShouldBeOfType<BadRequestObjectResult>();
        }

        [DataRow(0)]
        [DataRow(2)]
        [TestMethod]
        public void TestDeleteExceptionThrown(int changes)
        {
            //arrange
            _repository.GetCreditPartnerByIdAsync(0).Throws(new System.Exception());

            //act
            var res = _controller.Delete(0).Result;

            //assert
            (res as ObjectResult).StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
        }
    }
}
