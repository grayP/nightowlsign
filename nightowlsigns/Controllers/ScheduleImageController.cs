using nightowlsign.data.Models.Image;
using nightowlsign.data.Models.Schedule;
using nightowlsign.data.Models.ScheduleImage;
using System.Collections.Generic;
using System.Web.Mvc;
using nightowlsign.data.Models.UpLoadLog;

namespace nightowlsign.Controllers
{
    public class ScheduleImageController : Controller
    {
        private readonly IScheduleImageManager _scheduleImageManager ;
        private readonly IScheduleManager _scheduleManager;
        private readonly IUpLoadLoggingManager _upLoadLoggingManager;

        public ScheduleImageController(IScheduleImageManager scheduleImageManager, IScheduleManager scheduleManager, IUpLoadLoggingManager upLoadLoggingManager)
        {
            _scheduleImageManager = scheduleImageManager;
            _scheduleManager = scheduleManager;
            _upLoadLoggingManager = upLoadLoggingManager;
        }

        // GET: 
        [Authorize(Roles = "Admin,SuperUser")]
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

        [Authorize(Roles = "Admin,SuperUser")]
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
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult Index(ScheduleImageViewModel model)
        {
            List<ImageSelect> imageSelects = model.AllImages;
            //    List<ImageSelect> 
            foreach (var imageSelect in imageSelects)
            {
                _scheduleImageManager.UpdateImageList(imageSelect, model.Schedule);
            }
            _scheduleManager.UpdateDate(model.Schedule.Id);
           _upLoadLoggingManager.Delete(model.Schedule.Id);

            return RedirectToAction("Index", "Schedules", new {SignId=model.SignId});
        }
    }
}