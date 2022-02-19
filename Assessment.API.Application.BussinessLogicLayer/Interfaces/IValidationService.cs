using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.API.Application.BussinessLogicLayer.Interfaces
{
    public interface IValidationService
    {
        bool ValidateEmailAddress(string email);
    }
}
