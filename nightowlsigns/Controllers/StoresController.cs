using System.Web.Mvc;
using nightowlsign.data.Models.Stores;

namespace nightowlsign.Controllers
{
    public class StoresController : Controller
    {
        // GET: 
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var svm = new StoreViewModel();
            svm.HandleRequest();

            return View(svm);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult show()
        {
            var svm = new StoreViewModel();
            svm.HandleRequest();

            return View(svm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Index(StoreViewModel svm)
        {
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