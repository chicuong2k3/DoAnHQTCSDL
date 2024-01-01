using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
    public class Credit
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(customer))]
        public string CustomerId { get; set; }
        public Customer customer { get; set; }
        public decimal Balance { set; get; }
    }
}
