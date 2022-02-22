﻿using AutoMapper;
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
        private CreditPartnerUnregisterIncomingDto _dto;
        private DateTime _endDate;

        [TestInitialize]
        public void Init()
        {
            _repository = Substitute.For<ICreditPartnerRepository>();
            _mapper = Substitute.For<IMapper>();
            _controller = new CreditPartnerController(_repository, _mapper);
            _endDate = new DateTime(2020, 10, 10);
            _dto = new CreditPartnerUnregisterIncomingDto() { EndDate = _endDate };
        }

        [DataRow(1)]
        [TestMethod]
        public void PutOkTest(int id)
        {
            //arrange
            var fromDb = new CreditPartner() { IdNumber = 1 };
            _repository.GetCreditPartnerByIdAsync(Arg.Any<int>()).Returns(Task.FromResult(fromDb));
            _repository.SaveChangesAsync().Returns(1);

            var output = new CreditPartnerFullInfoDto() { EndDate = _endDate };

            _mapper.Map(Arg.Any<CreditPartnerUnregisterIncomingDto>(), Arg.Any<CreditPartner>()).Returns(fromDb);
            _mapper.Map<CreditPartnerFullInfoDto>(Arg.Any<CreditPartner>())
                .Returns(output);

            //act
            var res = _controller.Put(id, _dto).Result;

            //assert
            //specifikovat na non default
            //repo receveived s id 0, specific ID
            res.Result.ShouldBeOfType<OkObjectResult>();
            (res.Result as OkObjectResult).Value.ShouldBeOfType<CreditPartnerFullInfoDto>();
            ((res.Result as OkObjectResult).Value as CreditPartnerFullInfoDto).EndDate.ShouldBe(_endDate);

            _repository.Received().GetCreditPartnerByIdAsync(id);
            _repository.Received().SaveChangesAsync();

            _mapper
                .Received()
                .Map(_dto, fromDb);

            _mapper
                .Received()
                .Map<CreditPartnerFullInfoDto>(fromDb);
        }

        [DataRow(1)]
        [TestMethod]
        public void PutNotFoundTest(int id)
        {
            //arrange
            _repository.GetCreditPartnerByIdAsync(Arg.Any<int>()).Returns(Task.FromResult<CreditPartner>(null));

            //act
            var res = _controller.Put(id, _dto).Result;

            //assert
            res.Result.ShouldBeOfType<NotFoundResult>();

            _repository.Received().GetCreditPartnerByIdAsync(id);
        }

        [DataRow(1, 0)]
        [DataRow(1, 2)]
        [TestMethod]
        public void PutSaveChangeErrorTest(int id, int changes)
        {
            //arrange
            var fromDb = new CreditPartner() { IdNumber = 1 };
            _repository.GetCreditPartnerByIdAsync(Arg.Any<int>()).Returns(Task.FromResult(fromDb));
            _repository.SaveChangesAsync().Returns(changes);

            _mapper.Map(Arg.Any<CreditPartnerUnregisterIncomingDto>(), Arg.Any<CreditPartner>()).Returns(fromDb);

            //act
            var res = _controller.Put(id, _dto).Result;

            //assert
            res.Result.ShouldBeOfType<BadRequestObjectResult>();

            _repository.Received().GetCreditPartnerByIdAsync(id);
            _mapper
                .Received()
                .Map(_dto, fromDb);
            _repository.Received().SaveChangesAsync();
        }

        [DataRow(1)]
        [TestMethod]
        public void PutInternalServerErrorTest(int id)
        {
            //arrange
            _repository.GetCreditPartnerByIdAsync(Arg.Any<int>()).Throws(new Exception());

            //act
            var res = _controller.Put(id, _dto).Result;

            //assert
            (res.Result as ObjectResult).StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);

            _repository.Received().GetCreditPartnerByIdAsync(id);
        }
    }
}