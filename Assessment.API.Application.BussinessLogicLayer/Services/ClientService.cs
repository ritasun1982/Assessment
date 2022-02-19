using Assessment.API.Application.BussinessLogicLayer.Interfaces;
using Assessment.API.Application.DataAccessLayer.Data;
using Assessment.API.Application.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.API.Application.BussinessLogicLayer.Services
{
    public class ClientService : IClientService
    {
        private readonly ApplicationDBContext _dbContext;
        public ClientService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Client> CreateClient(Client client)
        {

            client.CreatedDateTime = DateTime.Now;

            var _client = await _dbContext.Clients.AddAsync(client);

            _dbContext.SaveChanges();

            return _client.Entity;

        }

        public Client GetClient(int id)
        {

            var client = _dbContext.Clients.Where(c => c.UniqueId == id).FirstOrDefault();
            return client;

        }

        public IEnumerable<Client> GetClientsByName(string name)
        {
            var clients = _dbContext.Clients.Where(c => c.Name == name);
            return clients;
        }


        public IEnumerable<Client> GetAllClients()
        {
            var clients = _dbContext.Clients;
            return clients;
        }


        public async Task UpdateClient(int id, Client client)
        {
            var clientToUpdate = _dbContext.Clients.Where(c => c.UniqueId == id).FirstOrDefault();

            if (clientToUpdate != null)
            {
                clientToUpdate.Email = client.Email;
                clientToUpdate.Name = client.Name;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteClient(int id)
        {
            var client = _dbContext.Clients.Where(c => c.UniqueId == id).FirstOrDefault();
            if (client != null)
            {
                _dbContext.Clients.Remove(client);
                await _dbContext.SaveChangesAsync();
            }

        }
    }
}
