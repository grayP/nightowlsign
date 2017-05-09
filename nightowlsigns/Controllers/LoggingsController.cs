using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using nightowlsign.data;

using nightowlsign.data.Interfaces;
using nightowlsign.data.Models.Logging;

namespace nightowlsign.Controllers
{
    public class LoggingsController : Controller
    {
        private readonly Inightowlsign_Entities _context;
        private readonly ILoggingManager _loggingManager;

        public LoggingsController(Inightowlsign_Entities context, ILoggingManager loggingManager)
        {
            _context = context;
            _loggingManager = loggingManager;
        }
        // GET: Loggings
        public ActionResult Index(string filter="")
        {
            
            return View(_loggingManager.GetLatest(filter));
        }

        // GET: Loggings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Logging logging =_context.Loggings.Find(id);
            if (logging == null)
            {
                return HttpNotFound();
            }
            return View(logging);
        }

        // GET: Loggings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Loggings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Subject,Description,DateStamp")] Logging logging)
        {
            if (ModelState.IsValid)
            {
                _context.Loggings.Add(logging);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(logging);
        }

        // GET: Loggings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Logging logging = _context.Loggings.Find(id);
            if (logging == null)
            {
                return HttpNotFound();
            }
            return View(logging);
        }

        // POST: Loggings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Subject,Description,DateStamp")] Logging logging)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(logging).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(logging);
        }

        // GET: Loggings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Logging logging = _context.Loggings.Find(id);
            if (logging == null)
            {
                return HttpNotFound();
            }
            return View(logging);
        }

        // POST: Loggings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Logging logging = _context.Loggings.Find(id);
            _context.Loggings.Remove(logging);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
