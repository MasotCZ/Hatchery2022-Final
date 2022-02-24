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

namespace WebAPITest.ControllersTest.ProfitReportAPIController
{
    [TestClass]
    public class ProfitReportAPICOntrollerGetMosSuccesfulTest
    {
        private PartnerReportController _controller;
        private IProfitabilityRepository _repository;
        private IMapper _mapper;

        [TestInitialize]
        public void Init()
        {
            _repository = Substitute.For<IProfitabilityRepository>();
            _mapper = Substitute.For<IMapper>();
            _controller = new PartnerReportController(_repository, _mapper);
        }

        [TestMethod]
        public void GetMostProfitableOkTest()
        {
            //arrange
            var partner = new CreditPartner()
            {
                IdNumber = 1,
                Requests = new CreditRequest[1]
            { new CreditRequest { ContactStatus = new CreditRequestStatus()
                { StatusCode = CreditRequestStatusCode.Accepted } } }
            };

            var partnerDto = new CreditPartnerFullInfoDto() { IdNumber = 1 };
            _repository.GetMostSuccessfulPartnerAsync(Arg.Any<bool>()).Returns(partner);
            _mapper.Map<CreditPartnerFullInfoDto>(Arg.Any<CreditPartner>()).Returns(partnerDto);

            //act
            var res = _controller.MostSuccessfulPartner().Result;

            //assert
            _repository.Received().GetMostSuccessfulPartnerAsync(true);
            _mapper.Received().Map<CreditPartnerFullInfoDto>(partner);

            res.Result.ShouldBeOfType<OkObjectResult>();
            (res.Result as OkObjectResult).Value.ShouldBeOfType<ProfitabilityReportDto>();
            ((res.Result as OkObjectResult).Value as ProfitabilityReportDto).Partner.ShouldBe(partnerDto);
        }

        [TestMethod]
        public void GetMostProfitableNotFoundWithNullRequestsTest()
        {
            //arrange
            var partner = new CreditPartner() { IdNumber = 1, Requests = null };
            _repository.GetMostSuccessfulPartnerAsync(Arg.Any<bool>()).Returns(partner);

            //act
            var res = _controller.MostSuccessfulPartner().Result;

            //assert
            _repository.Received().GetMostSuccessfulPartnerAsync(true);

            res.Result.ShouldBeOfType<NotFoundObjectResult>();
        }


        [TestMethod]
        public void GetMostProfitableNotFoundWithNoRequestsTest()
        {
            //arrange
            var partner = new CreditPartner() { IdNumber = 1, Requests = new CreditRequest[0] };
            _repository.GetMostSuccessfulPartnerAsync(Arg.Any<bool>()).Returns(partner);

            //act
            var res = _controller.MostSuccessfulPartner().Result;

            //assert
            _repository.Received().GetMostSuccessfulPartnerAsync(true);

            res.Result.ShouldBeOfType<NotFoundObjectResult>();
        }

        [TestMethod]
        public void GetMostProfitableNotFoundWithNoAcceptedRequestsTest()
        {
            //arrange
            var partner = new CreditPartner()
            {
                IdNumber = 1,
                Requests = new CreditRequest[1]
            { new CreditRequest { ContactStatus = new CreditRequestStatus()
                { StatusCode = CreditRequestStatusCode.Unfulfilled } } }
            };

            _repository.GetMostSuccessfulPartnerAsync(Arg.Any<bool>()).Returns(partner);

            //act
            var res = _controller.MostSuccessfulPartner().Result;

            //assert
            _repository.Received().GetMostSuccessfulPartnerAsync(true);

            res.Result.ShouldBeOfType<NotFoundObjectResult>();
        }

        [TestMethod]
        public void GetMostProfitableInternalServerErrorTest()
        {
            //arrange
            _repository.GetMostSuccessfulPartnerAsync(Arg.Any<bool>()).Throws(new System.Exception());

            //act
            var res = _controller.MostSuccessfulPartner().Result;

            //assert
            _repository.Received().GetMostSuccessfulPartnerAsync(true);

            (res.Result as ObjectResult).StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
        }
    }
}
