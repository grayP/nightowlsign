using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nightowlsign.data.Models.Schedule;
using nightowlsign.Models;

namespace nightowlsign.Controllers
{
    public class SchedulesController : Controller
    {
        private readonly IScheduleViewModel _scheduleViewModel;
         public SchedulesController(IScheduleViewModel scheduleViewModel)
        {
            _scheduleViewModel = scheduleViewModel;
                 }
        // GET: 
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult Index(int signId)
        {
            signId = Helper.GetSetSignSize(signId);

            _scheduleViewModel.SearchEntity.SignId = signId;
            _scheduleViewModel.HandleRequest();
            return View(_scheduleViewModel);
        }

        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult Show()
        {
             _scheduleViewModel.HandleRequest();
            return View(_scheduleViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult Index(ScheduleViewModel svm)
        {
            svm.SearchEntity.SignId = Helper.GetSetSignSize(svm.SearchEntity.SignId);

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