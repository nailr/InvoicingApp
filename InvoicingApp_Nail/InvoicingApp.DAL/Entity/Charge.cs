using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicingApp.DAL.Entity
{
    public class Charge
    {
        //Int primary key with identity
        [Key]
        public int Id { get; set; }

        //Setting Index for the unique valdidation on server side
        [StringLength(450)]
        [Index(IsUnique = true)]
        public string ChargeSymbol { get; set; }

        //Member of the one to many relationship
        public ICollection<Invoice> Invoices { get; set; }

    }
}
