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
    public class Invoice
    {
        //Int primary key with identity
        [Key]
        public int Id { get; set; }

        //Setting Index for the unique valdidation on server side
        [Required(ErrorMessage = "Invoice Number required")]
        [DisplayName("Invoice Number")]
        [StringLength(400)]
        [Index("IX_InvoiceNumberAndClientId", 1, IsUnique = true)]
        public string InvoiceNumber { get; set; }

        //Setting Index for the unique valdidation on server side
        [Index("IX_InvoiceNumberAndClientId", 2, IsUnique = true)]
        [ForeignKey("Client")]
        public int ClientId { get; set; }

        public Client Client { get; set; }

        [DisplayName("Invoice Date")]
        [DataType(DataType.Date)]
        public DateTime? InvoiceDate { get; set; }

        [DisplayName("Strat Date")]
        [DataType(DataType.Date)]
        public DateTime? StratDate { get; set; }

        [DisplayName("End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [DisplayName("Charge")]
        [ForeignKey("Charge")]
        public int ChargeId { get; set; }

        public Charge Charge { get; set; }

        public decimal? Rate { get; set; }
        public int? Units { get; set; }

        public decimal? Amount { get; set; }

        public decimal? Tax { get; set; }

        public decimal? Total { get; set; }

    }
}
