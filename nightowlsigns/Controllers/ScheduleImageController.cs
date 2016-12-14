using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nightowlsign.data.Models;

namespace nightowlsign.Controllers
{
    public class ScheduleImageController : Controller
    {
        // GET: 
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int SignId, int scheduleId, string scheduleName)
        {
            ScheduleImageViewModel ssvm = new ScheduleImageViewModel
            {
                SignId = SignId
            };
            ssvm.Schedule.Id = scheduleId;
            ssvm.Schedule.Name = scheduleName;
            ssvm.loadData();
            return View(ssvm);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Index(ScheduleImageViewModel model)
        {
            ScheduleImageManager sim = new ScheduleImageManager();
            List<ImageSelect> imageSelects = model.AllImages;

            foreach (ImageSelect imageSelect in imageSelects)
            {
                sim.UpdateImageList(imageSelect, model.Schedule);

            }

            return RedirectToAction("Index", "Schedules", new {SignId=model.SignId});

        }
    }

}