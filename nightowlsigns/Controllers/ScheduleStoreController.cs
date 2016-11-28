using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nightowlsign.data.Models;

namespace nightowlsign.Controllers
{
    public class ScheduleStoreController : Controller
    {
        // GET: RegattaCrew
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int scheduleId, string scheduleName)
        {
            ScheduleStoreViewModel ssvm = new ScheduleStoreViewModel();
            ssvm.Schedule.Id = scheduleId;
            ssvm.Schedule.Name = scheduleName;
            ssvm.loadData();
            return View(ssvm);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Index(ScheduleStoreViewModel model)
        {
            ScheduleStoreManager ssm = new ScheduleStoreManager();
            List<StoreSelect> storeSlects = model.AllStores;

            foreach (StoreSelect storeSelect in storeSlects)
            {
                ssm.UpdateStoreList(storeSelect, model.Schedule);
            }
            return RedirectToAction("Index", "Schedules");

        }


    }

}