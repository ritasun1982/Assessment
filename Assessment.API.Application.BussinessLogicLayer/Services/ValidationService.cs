using Assessment.API.Application.BussinessLogicLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Assessment.API.Application.BussinessLogicLayer.Services
{
    public class ValidationService : IValidationService
    {
        public bool ValidateEmailAddress(string email)
        {
            var regex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            
            return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);

        }
    }
}
