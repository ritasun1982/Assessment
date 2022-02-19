using Assessment.API.Application.BussinessLogicLayer.Interfaces;
using Assessment.API.Application.BussinessLogicLayer.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.API.Application.BussinessLogicLayer.Test.Services
{
    [TestClass]
   public  class ValidationServiceTests
    {

        private MockRepository _mockRepository;
        private IValidationService _target;
        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _target = new ValidationService();
        }
        [TestCleanup]
        public void TestCleanup()
        {
            _mockRepository.VerifyAll();
        }


        [TestMethod]
        public void TestValidEmailAddress()
        {
            Assert.IsTrue(_target.ValidateEmailAddress("test@test.com"));
        }

        [TestMethod]
        public void TestInvalidEmailAddress()
        {
            Assert.IsFalse(_target.ValidateEmailAddress("test@test.com|"));
        }
    }
}
