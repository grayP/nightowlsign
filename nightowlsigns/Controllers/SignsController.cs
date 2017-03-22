using nightowlsign.data.Models.Signs;
using System.Collections.Generic;
using System.Web.Mvc;

namespace nightowlsign.Controllers
{
    public class SignsController : Controller
    {
        private readonly SignViewModel _signViewModel = new SignViewModel();
        // GET: 
        [Authorize(Roles="Admin")]
        public ActionResult Index()
        {
            _signViewModel.HandleRequest();

            return View(_signViewModel);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Show(int Id)
        {
            _signViewModel.HandleRequest();
       return View(_signViewModel);
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
