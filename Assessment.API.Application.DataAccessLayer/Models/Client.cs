using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.API.Application.DataAccessLayer.Models
{
    public class Client
    {

        [Key]
        public int UniqueId { get; set; }
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public DateTime CreatedDateTime { get; set; }

    }
}
