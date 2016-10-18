﻿using System;
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
           
            CreatePlayBill ip = new CreatePlayBill(svm.signsForSchedule,svm.AllImagesInSchedule);
            ip.GeneratethePlayBillFile(svm.Schedule.Name);
            svm.loadData();

         
            return View(svm);
        }
    }
}