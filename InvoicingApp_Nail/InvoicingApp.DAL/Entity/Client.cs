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
    public class Client
    {
        //Int primary key with identity
        [Key]
        public int Id { get; set; }

        //Setting Index for the unique valdidation on server side
        [Required(ErrorMessage = "Company Name required")]
        [DisplayName("Company Name")]
        [StringLength(450)]
        [Index(IsUnique = true)]
        public string CompanyName { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Contract { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email required")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Wrong email format")]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("Contact Person")]
        public string ContactPerson { get; set; }

        [DisplayName("Contract Date Start")]
        [DataType(DataType.Date)]
        public DateTime? ContractDateStart { get; set; }

        [DisplayName("Contract Date End")]
        [DataType(DataType.Date)]
        public DateTime? ContractDateEnd { get; set; }

        public bool Status { get; set; }

        //Member of the one to many relationship
        public ICollection<Invoice> Invoices { get; set; }

    }
}
