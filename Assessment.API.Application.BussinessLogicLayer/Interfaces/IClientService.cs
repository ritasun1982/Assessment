using Assessment.API.Application.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.API.Application.BussinessLogicLayer.Interfaces
{
    public interface IClientService
    {
        Task<Client> CreateClient(Client client);
        Client GetClient(int id);
        IEnumerable<Client> GetAllClients();
        IEnumerable<Client> GetClientsByName(string name);
        Task UpdateClient(int id, Client client);
        Task DeleteClient(int id);

    }
}
