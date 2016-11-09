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
        public ActionResult Index(int scheduleId, string scheduleName, string phase)
        {
            SendToSignViewModel ssvm = new SendToSignViewModel();
            ssvm.Schedule.Id = scheduleId;
            ssvm.Schedule.Name = scheduleName;
            ssvm.loadData();

            if (phase == "resend")
            {
                SendCommunicator sendCommunicator = new SendCommunicator();
                ssvm.DisplayMessage += sendCommunicator.SendFiletoSign(ssvm.StoresForSchedule);
            }

            return View(ssvm);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Index(SendToSignViewModel svm)
        {
            svm.IsValid = ModelState.IsValid;
            try
            {
                CreateFilesToSend createFilesToSend = new CreateFilesToSend(svm.SignsForSchedule, svm.AllImagesInSchedule);
                createFilesToSend.DeleteOldImages();
                createFilesToSend.WriteImagesToDisk();
                createFilesToSend.GeneratetheProgramFiles(svm.Schedule.Name);
                createFilesToSend.GeneratethePlayBillFile(svm.Schedule.Name);

                SendCommunicator sendCommunicator = new SendCommunicator(createFilesToSend.PlaybillFileName);
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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Resend(SendToSignViewModel svm)
        {
            svm.IsValid = ModelState.IsValid;
            try
            {
                CreateFilesToSend createFilesToSend = new CreateFilesToSend(svm.SignsForSchedule, svm.AllImagesInSchedule);
                createFilesToSend.GeneratethePlayBillFile(svm.Schedule.Name);

                SendCommunicator sendCommunicator = new SendCommunicator(createFilesToSend.PlaybillFileName);
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
    }
}