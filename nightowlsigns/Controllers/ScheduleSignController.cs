using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nightowlsign.data.Models;

namespace nightowlsign.Controllers
{
    public class ScheduleSignController : Controller
    {
       
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int scheduleId, string scheduleName)
        {
            ScheduleSignViewModel ssvm = new ScheduleSignViewModel();
            ssvm.schedule.Id = scheduleId;
            ssvm.schedule.Name = scheduleName;
            ssvm.loadData();
            return View(ssvm);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Index(ScheduleSignViewModel model)
        {
            ScheduleSignManager ssm = new ScheduleSignManager();
            List<data.Models.SelectListItem> signSelects = model.AllSigns;

            foreach (data.Models.SelectListItem signSelect in signSelects)
            {
                ssm.UpdateSignList(signSelect, model.schedule);
            }
            return RedirectToAction("Index", "Schedules");

        }


    }

}