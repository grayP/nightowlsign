using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nightowlsign.data.Models.Schedule;

namespace nightowlsign.Controllers
{
    public class SchedulesController : Controller
    {
        // GET: 
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var svm = new ScheduleViewModel();
            svm.HandleRequest();
            return View(svm);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult show()
        {
            var svm = new ScheduleViewModel();
            svm.HandleRequest();
            return View(svm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Index(ScheduleViewModel svm)
        {
            svm.IsValid = ModelState.IsValid;
            svm.HandleRequest();

            if (svm.IsValid)
            {
                ModelState.Clear();
            }
            else
            {
                foreach (var item in svm.ValidationErrors)
                {
                    ModelState.AddModelError(item.Key, item.Value);
                }
            }
            return View(svm);
        }
    }

}