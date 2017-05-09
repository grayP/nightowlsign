using System.Web.Mvc;
using nightowlsign.data.Models.Stores;

namespace nightowlsign.Controllers
{
    public class StoresController : Controller
    {
        private readonly IStoreViewModel _storeViewModel;

        public StoresController(IStoreViewModel storeViewModel)
        {
            _storeViewModel = storeViewModel;
        }

        // GET: 
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult Index()
        {
            _storeViewModel.HandleRequest();
            return View(_storeViewModel);
        }

        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult show()
        {
            _storeViewModel.HandleRequest();
            return View(_storeViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult Index(StoreViewModel storeViewModel)
        {
            storeViewModel.IsValid = ModelState.IsValid;
            storeViewModel.HandleRequest();

            if (storeViewModel.IsValid)
            {
                ModelState.Clear();
            }
            else
            {
                foreach (var item in storeViewModel.ValidationErrors)
                {
                    ModelState.AddModelError(item.Key, item.Value);
                }
            }
            return View(storeViewModel);
        }
    }
}