using nightowlsign.data.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using nightowlsign.data.Models.ScheduleStore;

namespace nightowlsign.Controllers
{

    public class ScheduleStoreController : Controller
    {
       private readonly ScheduleStoreViewModel _scheduleStoreViewModel = new ScheduleStoreViewModel();
       private readonly ScheduleStoreManager _scheduleStoreManager = new ScheduleStoreManager();

        // GET: 
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int signId, int scheduleId, string scheduleName)
        {
            _scheduleStoreViewModel.Schedule.Id = scheduleId;
            _scheduleStoreViewModel.Schedule.Name = scheduleName;
            _scheduleStoreViewModel.LoadData(signId);
            return View(_scheduleStoreViewModel);
        }
 
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Index(ScheduleStoreViewModel model)
        {
            var currentSignSize = 0;
            List<StoreSelect> storeSelects = model.AllStores;

            foreach (var storeSelect in storeSelects)
            {
                _scheduleStoreManager.UpdateStoreList(storeSelect, model.Schedule);
                currentSignSize = storeSelect.SignId;
            }
            return RedirectToAction("Index", "Schedules", new {SignId=currentSignSize});
        }
    }
}