using System.Web.Mvc;

namespace nightowlsign.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return RedirectToAction("index", "Stores");
        }
    }
}