using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using nightowlsign.data;
using nightowlsign.data.Models.Signs;
using nightowlsign.Models;

namespace nightowlsign.Controllers
{
    public class SignsController : Controller
    {
        // GET: 
        [Authorize(Roles="Admin")]
        public ActionResult Index()
        {
            SignViewModel svm = new SignViewModel();
            svm.HandleRequest();

            return View(svm);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult show(int Id)
        {
            SignViewModel svm = new SignViewModel();
            svm.HandleRequest();

            return View(svm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Index(SignViewModel svm)
        {
            svm.IsValid = ModelState.IsValid;
            svm.HandleRequest();

            if (svm.IsValid)
            {
                ModelState.Clear();
            }
            else
            {
                foreach (KeyValuePair<string, string> item in svm.ValidationErrors)
                {
                    ModelState.AddModelError(item.Key, item.Value);
                }

            }

            return View(svm);
        }

    }
}
