using nightowlsign.data.Models.Image;
using nightowlsign.data.Models.Schedule;
using nightowlsign.data.Models.ScheduleImage;
using System.Collections.Generic;
using System.Web.Mvc;

namespace nightowlsign.Controllers
{
    public class ScheduleImageController : Controller
    {
        private readonly IScheduleImageManager _scheduleImageManager ;
        private readonly IScheduleManager _scheduleManager;

        public ScheduleImageController(IScheduleImageManager scheduleImageManager, IScheduleManager scheduleManager)
        {
            _scheduleImageManager = scheduleImageManager;
            _scheduleManager = scheduleManager;
        }

        // GET: 
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int signId, int scheduleId, string scheduleName)
        {
            var ssvm = new ScheduleImageViewModel
            {
                SignId = signId,
                Schedule =
                {
                    Id = scheduleId,
                    Name = scheduleName
                }
            };
            ssvm.LoadData();
            return View(ssvm);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Display(int scheduleId, string scheduleName)
        {
            var ssvm = new ScheduleImageViewModel
            {
                Schedule =
                {
                    Id = scheduleId,
                    Name = scheduleName
                }
            };
            ssvm.GetImagesForThisSchedule(scheduleId);
            return View(ssvm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Index(ScheduleImageViewModel model)
        {
            List<ImageSelect> imageSelects = model.AllImages;
            //    List<ImageSelect> 
            foreach (var imageSelect in imageSelects)
            {
                _scheduleImageManager.UpdateImageList(imageSelect, model.Schedule);
            }
            _scheduleManager.UpdateDate(model.Schedule.Id);

            return RedirectToAction("Index", "Schedules", new {SignId=model.SignId});
        }
    }
}