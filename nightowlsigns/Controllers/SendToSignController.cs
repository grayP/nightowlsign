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
                CreateFilesToSend createFilesToSend = new CreateFilesToSend(svm.SignsForSchedule, svm.AllImagesInSchedule);
                createFilesToSend.GeneratethePlayBillFile(svm.Schedule.Name);

                SendCommunicator sendCommunicator = new SendCommunicator(createFilesToSend.ProgramFiles, createFilesToSend.PlaybillFileName);
                svm.DisplayMessage += sendCommunicator.SendFiletoSign(svm.StoresForSchedule);
            }
            catch (Exception ex)
            {
                svm.DisplayMessage = ex.InnerException.ToString();
            }
             svm.loadData();
         
            return View(svm);
        }
    }
}