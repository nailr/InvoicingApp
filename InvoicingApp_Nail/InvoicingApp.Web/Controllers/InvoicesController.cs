using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InvoicingApp.DAL.DataConnection;
using InvoicingApp.DAL.Entity;
using PagedList;
using Microsoft.Reporting.WebForms;
using System.IO;
using InvoicingApp.Web.Reports;
using System.Data.SqlClient;

namespace InvoicingApp.Web.Controllers
{
    public class InvoicesController : Controller
    {
        private DataContext db = new DataContext();
        private InvoicingApp_NailEntities1 rptdb = new InvoicingApp_NailEntities1();

        // GET: Invoices
        public ViewResult Index(string sortOrder, string searchParameter, string currentFilter, int? page)
        {
            List<Invoice> invoices = new List<Invoice>();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NumberSortParm = String.IsNullOrEmpty(sortOrder) ? "number_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";

            //Setting the pager on 1 on initial load
            if (searchParameter != null)
            {
                page = 1;
            }
            else
            {
                searchParameter = currentFilter;
            }

            ViewBag.CurrentFilter = searchParameter;

            //Make filtered db request to minimize data flow
            if (!String.IsNullOrEmpty(searchParameter))
                invoices = db.Invoices.Where(i => (i.InvoiceNumber.Contains(searchParameter) || i.Client.CompanyName.Contains(searchParameter))).ToList();
            else
                invoices = db.Invoices.ToList();

            //Populating data for referent tables
            foreach (Invoice i in invoices)
            {
                i.Client = db.Clients.Find(i.ClientId);
                i.Charge = db.Charges.Find(i.ChargeId);
            }

            //Main sorting logic
            switch (sortOrder)
            {
                case "number_desc":
                    invoices = invoices.OrderByDescending(i => i.InvoiceNumber).ToList();
                    break;
                case "name_desc":
                    invoices = invoices.OrderByDescending(i => i.Client.CompanyName).ToList();
                    break;
                case "Name":
                    invoices = invoices.OrderBy(i => i.Client.CompanyName).ToList();
                    break;
                default:
                    invoices = invoices.OrderBy(i => i.InvoiceNumber).ToList();
                    break;
            }

            //Paging configuration
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(invoices.ToPagedList(pageNumber, pageSize));
        }

        // GET: Invoices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // GET: Invoices/Create
        public ActionResult Create()
        {
            ViewBag.ChargeId = new SelectList(db.Charges, "Id", "ChargeSymbol");
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "CompanyName");
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,InvoiceNumber,ClientId,InvoiceDate,StratDate,EndDate,ChargeId,Rate,Units,Amount")] Invoice invoice)
        {
            //Calculating Total and Tax - Total = Units*Amount and Taxes are 17% of Total
            if (invoice.Amount != null && invoice.Units != null)
            {
                invoice.Total = invoice.Units * invoice.Amount;
                invoice.Tax = decimal.Parse((double.Parse(invoice.Total.ToString()) * .17).ToString());
            }

            //Client side validation logic - combination of InvocieNumber and ClientId
            Invoice validation = new Invoice();
            validation = db.Invoices.Where(x => (x.InvoiceNumber == invoice.InvoiceNumber && x.ClientId == invoice.ClientId)).FirstOrDefault();

            if (validation != null)
            {
                ModelState.AddModelError("ClientId", "Combination of this Company Name and Invoice Number already exists.");
                ModelState.AddModelError("InvoiceNumber", "Combination of this Company Name and Invoice Number already exists.");
                ViewBag.ChargeId = new SelectList(db.Charges, "Id", "ChargeSymbol", invoice.ChargeId);
                ViewBag.ClientId = new SelectList(db.Clients, "Id", "CompanyName", invoice.ClientId);
                return View(invoice);
            }

            if (ModelState.IsValid)
            {
                db.Invoices.Add(invoice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ChargeId = new SelectList(db.Charges, "Id", "ChargeSymbol", invoice.ChargeId);
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "CompanyName", invoice.ClientId);
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }

            ViewBag.ChargeId = new SelectList(db.Charges, "Id", "ChargeSymbol", invoice.ChargeId);
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "CompanyName", invoice.ClientId);
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,InvoiceNumber,ClientId,InvoiceDate,StratDate,EndDate,ChargeId,Rate,Units,Amount,Tax,Total")] Invoice invoice)
        {
            //Calculating Total and Tax - Total = Units*Amount and Taxes are 17% of Total
            if (invoice.Amount != null && invoice.Units != null)
            {
                invoice.Total = invoice.Units * invoice.Amount;
                invoice.Tax = decimal.Parse((double.Parse(invoice.Total.ToString()) * .17).ToString());
            }

            //Client side validation logic - combination of InvocieNumber and ClientId
            Invoice validation = new Invoice();
            validation = db.Invoices.Where(x => x.InvoiceNumber == invoice.InvoiceNumber && x.Id != invoice.Id && x.ClientId == invoice.ClientId).FirstOrDefault();

            if (validation != null)
            {
                ModelState.AddModelError("ClientId", "Combination of this Company Name and Invoice Number already exists.");
                ModelState.AddModelError("InvoiceNumber", "Combination of this Company Name and Invoice Number already exists.");
                ViewBag.ChargeId = new SelectList(db.Charges, "Id", "ChargeSymbol", invoice.ChargeId);
                ViewBag.ClientId = new SelectList(db.Clients, "Id", "CompanyName", invoice.ClientId);
                return View(invoice);
            }

            if (ModelState.IsValid)
            {
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ChargeId = new SelectList(db.Charges, "Id", "ChargeSymbol", invoice.ChargeId);
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "CompanyName", invoice.ClientId);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invoice invoice = db.Invoices.Find(id);
            db.Invoices.Remove(invoice);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult InvoicePrint(int id)
        {
            //Variables
            string mimeType;
            string encoding;
            string filenameExtension = "pdf";
            string[] streams;
            Warning[] warnings;
            byte[] renderedByte;

            //Setting up local Report
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/InvoicePrint.rdlc");

            //Setting the report parameter
            ReportParameter parameter = new ReportParameter("ClientId", id.ToString());
            localReport.SetParameters(new ReportParameter[] { parameter });
            
            //Setting report datasources
            ReportDataSource reportDataSource = new ReportDataSource();
            ReportDataSource reportDataSource2 = new ReportDataSource();
            reportDataSource.Name = "InvoicingDataSet";
            reportDataSource2.Name = "TaxAndTotal";
            reportDataSource.Value = GetDetailData(id);
            reportDataSource2.Value = GetTaxAndTotalData(id);

            localReport.DataSources.Add(reportDataSource);
            localReport.DataSources.Add(reportDataSource2);
            
            //Rendering local report
            renderedByte = localReport.Render("PDF","", out mimeType, out encoding, out filenameExtension, out streams, out warnings);

            return File(new MemoryStream(renderedByte), "Invoice.pdf");

        }

        private DataTable GetDetailData (int id)
        {
            DataTable data = new DataTable();
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["InvoicingAppDBContext"].ConnectionString;
            using (SqlConnection cn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("GetInvoceDetails", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ClientId", SqlDbType.Int).Value = id;

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(data);

            }
            return data;
        }
        private DataTable GetTaxAndTotalData(int id)
        {
            DataTable data = new DataTable();
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["InvoicingAppDBContext"].ConnectionString;
            using (SqlConnection cn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("GetTotalAndTax", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ClientId", SqlDbType.Int).Value = id;

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(data);

            }
            return data;
        }

        public ActionResult ShowChart(string from, string to)
        {   //Variables
            string mimeType;
            string encoding;
            string filenameExtension = "pdf";
            string[] streams;
            Warning[] warnings;
            byte[] renderedByte;

            //Setting up local Report
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/InvoiceChart.rdlc");

            //Setting report datasources
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "ChartDataSet";
            reportDataSource.Value = GetChartData(from,to);

            localReport.DataSources.Add(reportDataSource);

            //Rendering local report
            renderedByte = localReport.Render("PDF", "", out mimeType, out encoding, out filenameExtension, out streams, out warnings);

            return File(new MemoryStream(renderedByte),"application/pdf", "Chart.pdf");

        }
        private DataTable GetChartData(string from, string to)
        {
            DataTable data = new DataTable();
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["InvoicingAppDBContext"].ConnectionString;
            using (SqlConnection cn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("GetChartData", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@From", SqlDbType.Date).Value = DateTime.Parse(from);
                cmd.Parameters.Add("@To", SqlDbType.Date).Value = DateTime.Parse(to);

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(data);

            }
            return data;
        }
    }
}
