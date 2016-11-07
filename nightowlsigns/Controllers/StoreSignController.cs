using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nightowlsign.data.Models;

namespace nightowlsign.Controllers
{
    public class StoreSignController : Controller
    {
        // GET: RegattaCrew
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int storeId, string storeName)
        {
            StoreSignViewModel ssvm = new StoreSignViewModel();
            ssvm.store.id = storeId;
            ssvm.store.Name = storeName;
            ssvm.loadData();
            return View(ssvm);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Index(StoreSignViewModel model)
        {
            StoreSignManager rcm = new StoreSignManager();
            List<SignSelect> signselects = model.AllSigns;

            foreach (SignSelect signSelect in signselects)
            {
                rcm.UpdateSignList(signSelect, model.store);
            }
            return RedirectToAction("Index", "Stores");

        }


    }
}