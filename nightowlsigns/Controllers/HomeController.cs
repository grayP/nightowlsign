using System.Web.Mvc;

namespace nightowlsign.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult Index()
        {
            return RedirectToAction("index", "Stores");
        }
    }
}