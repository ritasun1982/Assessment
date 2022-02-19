using Assessment.API.Application.BussinessLogicLayer.Interfaces;
using Assessment.API.Application.Controllers;
using Assessment.API.Application.DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Assessment.API.Application.Test
{
    [TestClass]
    public class ClientControllerTests
    {
        private MockRepository _mockRepository;
        private ClientController _target;
        private Mock<IClientService> _clientService;
        private Mock<IValidationService> _validationService;
        private Mock<ILogger<ClientController>> _logger;


        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _clientService = _mockRepository.Create<IClientService>();
            _validationService = _mockRepository.Create<IValidationService>();
            _logger = new Mock<ILogger<ClientController>>(MockBehavior.Loose);

            _target = new ClientController(_clientService.Object, _validationService.Object, _logger.Object);
        }


        [TestCleanup]
        public void TestCleanup()
        {
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        public void GetClients()
        {
            var clients = new List<Client>();
            clients.Add(new Client
            {
                UniqueId = 1,
                Email = "email@test.com",
                Name = "Tom",
                CreatedDateTime = DateTime.Now
            });

            _clientService.Setup(c => c.GetAllClients()).Returns(clients);

            var actual = _target.Get();

            var okResult = actual as OkObjectResult;
            var returnClients = okResult.Value as List<Client>;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(1, returnClients.Count);

        }

        [TestMethod]
        public void GetClientById()
        {
            var id = 1;
            var client = new Client
            {
                UniqueId = id,
                Email = "email@test.com",
                Name = "Tom",
                CreatedDateTime = DateTime.Now
            };

            _clientService.Setup(c => c.GetClient(id)).Returns(client);

            var actual = _target.Get(id);

            var okResult = actual as OkObjectResult;
            var returnClient = okResult.Value as Client;

            Assert.IsNotNull(okResult);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.AreEqual(1, returnClient.UniqueId);
            Assert.AreEqual("Tom", returnClient.Name);
        }

        [TestMethod]
        public void GetClientById_Not_Found()
        {
            var id = 1;
            Client client = null;

            _clientService.Setup(c => c.GetClient(id)).Returns(client);

            var actual = _target.Get(id);

            var notFoundResult = actual as NotFoundObjectResult;

            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
            Assert.AreEqual("The client couldn't be found.", notFoundResult.Value);

        }

        [TestMethod]
        public void CreateClient()
        {
            var client = new Client
            {
                Name = "Tom",
                Email = "Test@test.com"
            };


            var returnClient = new Client
            {
                UniqueId = 1,
                Name = "Tom",
                Email = "Test@test.com",
                CreatedDateTime = DateTime.Now
            };


            _clientService.Setup(c => c.CreateClient(client)).ReturnsAsync(returnClient);
            _validationService.Setup(v => v.ValidateEmailAddress(client.Email)).Returns(true);

            var actual = _target.Post(client);

            var okResult = actual as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);

        }

        [TestMethod]
        public void CreateClient_Email_Is_Invalid()
        {
            var client = new Client
            {
                Name = "Tom",
                Email = "Test@test.com|"
            };

            var returnClient = new Client
            {
                UniqueId = 1,
                Name = "Tom",
                Email = "Test@test.com|",
                CreatedDateTime = DateTime.Now
            };

            _validationService.Setup(v => v.ValidateEmailAddress(client.Email)).Returns(false);

            var actual = _target.Post(client);

            var badRequest = actual as BadRequestObjectResult;


            Assert.IsNotNull(badRequest);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, badRequest.StatusCode);

        }

        [TestMethod]
        public void CreateClient_Null()
        {
            Client client = null;

            var actual = _target.Post(client);

            var badRequest = actual as BadRequestObjectResult;


            Assert.IsNotNull(badRequest);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, badRequest.StatusCode);

        }

        [TestMethod]
        public void UpdateClient_Not_Found()
        {
            var id = 1;
            var client = new Client
            {
                UniqueId = id,
                Name = "Tom",
                Email = "Test@test.com",
                CreatedDateTime = DateTime.Now
            };

            Client returnedClient = null;

            _clientService.Setup(c => c.GetClient(client.UniqueId)).Returns(returnedClient);

            var actual = _target.Put(id, client);

            var notFoundResult = actual as NotFoundObjectResult;

            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);


        }

        [TestMethod]
        public void UpdateClient()
        {
            var id = 1;
            var client = new Client
            {
                UniqueId = id,
                Name = "Tim",
                Email = "Test@test.com",
                CreatedDateTime = DateTime.Parse("2022-02-19 11:42:35")
            };

            _clientService.Setup(c => c.GetClient(client.UniqueId)).Returns(client);
            _clientService.Setup(c => c.UpdateClient(id, client)).Returns(Task.FromResult(client));
            _validationService.Setup(v => v.ValidateEmailAddress(client.Email)).Returns(true);

            var actual = _target.Put(id, client);

            var okResult = actual as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);

        }

        [TestMethod]
        public void UpdateClient_Email_Invalid()
        {
            var id = 1;
            var client = new Client
            {
                UniqueId = id,
                Name = "Tim",
                Email = "Test@test.com|",
                CreatedDateTime = DateTime.Parse("2022-02-19 11:42:35")
            };

            _clientService.Setup(c => c.GetClient(client.UniqueId)).Returns(client);
            _validationService.Setup(v => v.ValidateEmailAddress(client.Email)).Returns(false);

            var actual = _target.Put(id, client);

            var badRequestResult = actual as BadRequestObjectResult;

            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);

        }

        [TestMethod]
        public void DeleteClient()
        {
            var id = 1;
            var client = new Client
            {
                UniqueId = id,
                Name = "Tim",
                Email = "Test@test.com",
                CreatedDateTime = DateTime.Parse("2022-02-19 11:42:35")
            };

            _clientService.Setup(c => c.GetClient(id)).Returns(client);
            _clientService.Setup(c => c.DeleteClient(id)).Returns(Task.FromResult(client));

            var actual = _target.Delete(client.UniqueId);

            var okResult = actual as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);

        }


        [TestMethod]
        public void DeleteClient_Not_Found()
        {
            var id = 1;
            Client client = null;

            _clientService.Setup(c => c.GetClient(id)).Returns(client);

            var actual = _target.Delete(id);

            var notFoundResult = actual as NotFoundObjectResult;

            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);

        }


    }
}
