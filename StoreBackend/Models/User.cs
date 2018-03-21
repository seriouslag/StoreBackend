using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StoreBackend.Models
{
    public class User: BaseEntity
    {
        public int Id { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string displayName { get; set; }
        public string email { get; set; }
        public Boolean isVerified { get; set; }

        public Cart Cart { get; set; }
        public int CartId { get; set; }
    }
}

