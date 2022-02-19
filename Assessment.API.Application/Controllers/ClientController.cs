using Assessment.API.Application.BussinessLogicLayer.Interfaces;
using Assessment.API.Application.BussinessLogicLayer.Services;
using Assessment.API.Application.DataAccessLayer.Data;
using Assessment.API.Application.DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment.API.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IValidationService _validationService;
        private readonly ILogger _log;

        public ClientController(IClientService clientService, 
                                IValidationService validationService, 
                                ILogger<ClientController> log)
        {
            _clientService = clientService;
            _validationService = validationService;
            _log = log;
        }


        // GET: api/client
        [HttpGet]
        public IActionResult Get()
        {
            _log.LogInformation("Calling get method");

            var  clinets = _clientService.GetAllClients();
            return Ok(clinets);
        }


        // GET: api/client/2
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            Client client = _clientService.GetClient(id);
            if (client == null)
            {
                return NotFound("The client couldn't be found.");
            }
            return Ok(client);
        }


        // POST: api/client
        [HttpPost]
        public IActionResult Post([FromBody] Client client)
        {
            if (client == null)
            {
                return BadRequest("Client is null.");
            }

            if(!_validationService.ValidateEmailAddress(client.Email))
            {
                return BadRequest("Email is not in a valid format.");
            }

            _clientService.CreateClient(client);

            return Ok(client);
                 
        }


        // PUT: api/client/2
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Client client)
        {
            if (_clientService.GetClient(id) == null)
                return NotFound("Client is not found");

            if (!_validationService.ValidateEmailAddress(client.Email))
            {
                return BadRequest("Email is not in a valid format.");
            }


            _clientService.UpdateClient(id, client);

            return Ok("Client is updated");
        }

        // DELETE: api/client/2
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            if (_clientService.GetClient(id) == null)
                return NotFound("Client is not found");

            _clientService.DeleteClient(id);
           
            return Ok("Client is deleted.");
        }


    }
}
