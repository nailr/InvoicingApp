using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoicingApp.DAL.Entity;

namespace InvoicingApp.DAL.DataConnection
{
    public class DataContext : DbContext
    {
        public DataContext() : base("name=InvoicingAppDBContext") { }

        public DbSet<Client> Clients { get; set;}
        public DbSet<Charge> Charges { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
    }
}
