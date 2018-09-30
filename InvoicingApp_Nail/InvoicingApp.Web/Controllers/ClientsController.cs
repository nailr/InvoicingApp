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

namespace InvoicingApp.Web.Controllers
{
    public class ClientsController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Clients
        public ViewResult Index(string sortOrder, string searchParameter, string currentFilter, int? page)
        {
            List<Client> clients = new List<Client>();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            
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
                clients = db.Clients.Where(c => c.CompanyName.Contains(searchParameter)).ToList();
            else
                clients = db.Clients.ToList();

            //Main sorting logic
            switch (sortOrder)
            {
                case "name_desc":
                    clients = clients.OrderByDescending(c => c.CompanyName).ToList();
                    break;
                default:
                    clients = clients.OrderBy(c => c.CompanyName).ToList();
                    break;
            }

            //Paging configuration
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(clients.ToPagedList(pageNumber, pageSize));
        }

        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CompanyName,City,Address,Contract,Phone,Email,ContactPerson,ContractDateStart,ContractDateEnd,Status")] Client client)
        {
            //Client side validation logic
            Client validation = new Client();
            validation = db.Clients.Where(x => x.CompanyName == client.CompanyName).FirstOrDefault();

            if (validation != null)
            {
                ModelState.AddModelError("CompanyName", "Company Name already exists.");
                return View(client);
            }

            if (ModelState.IsValid)
            {
                db.Clients.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(client);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Client client = db.Clients.Find(id);

            if (client == null)
            {
                return HttpNotFound();
            }

            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CompanyName,City,Address,Contract,Phone,Email,ContactPerson,ContractDateStart,ContractDateEnd,Status")] Client client)
        {
            //Client side validation logic
            Client validation = new Client();
            validation = db.Clients.Where(x => x.CompanyName == client.CompanyName && x.Id != client.Id).FirstOrDefault();

            if (validation != null)
            {
                ModelState.AddModelError("CompanyName", "Company Name already exists.");
                return View(client);
            }

            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
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
    }
}
