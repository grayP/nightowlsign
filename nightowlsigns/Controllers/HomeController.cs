using System.Web.Mvc;
using nightowlsign.data.Models.Stores;

namespace nightowlsign.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
           // var svm = new StoreViewModel();
          //  svm.HandleRequest();
            return RedirectToAction("index","Stores");
           // return View(svm);
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}