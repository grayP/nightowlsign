using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nightowlsign.data.Models;
using ImageProcessor.Services;

namespace nightowlsign.Controllers
{
    public class SendToSignController : Controller
    {
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int scheduleId, string scheduleName)
        {
            SendToSignViewModel ssvm = new SendToSignViewModel();
            ssvm.Schedule.Id = scheduleId;
            ssvm.Schedule.Name = scheduleName;
            ssvm.loadData();
            return View(ssvm);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Index(SendToSignViewModel svm)
        {
            svm.IsValid = ModelState.IsValid;
            try
            {
                CreateFilesToSend createFilesToSend = new CreateFilesToSend(svm.SignsForSchedule, svm.AllImagesInSchedule, svm.Schedule.Name);
                var playBillFileName = createFilesToSend.PlaybillFileName;
                SendCommunicator sendCommunicator = new SendCommunicator();

                svm.DisplayMessage += sendCommunicator.SendFiletoSign(svm.StoresForSchedule);
                svm.DebugMessage = createFilesToSend.DebugString;
            }
            catch (Exception ex)
            {
                svm.DisplayMessage = ex.ToString();
            }
            svm.loadData();

            return View(svm);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Resend(int scheduleId, string scheduleName)
        {
            SendToSignViewModel ssvm = new SendToSignViewModel();
            ssvm.Schedule.Id = scheduleId;
            ssvm.Schedule.Name = scheduleName;
            ssvm.loadData();
            try
            {
                SendCommunicator sendCommunicator = new SendCommunicator();
                ssvm.DisplayMessage += sendCommunicator.SendFiletoSign(ssvm.StoresForSchedule);
            }
            catch (Exception ex)
            {
                ssvm.DisplayMessage = ex.ToString();
            }
 
            return View("Index",ssvm);
        }
    }
}