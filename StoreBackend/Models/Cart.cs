using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StoreBackend.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }

        public List<CartItem> CartItems { get; set; }
    }
}
