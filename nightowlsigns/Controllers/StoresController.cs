using System.Web.Mvc;
using nightowlsign.data;
using nightowlsign.data.Interfaces;
using nightowlsign.data.Models.Stores;

namespace nightowlsign.Controllers
{
    public class StoresController : Controller
    {
        private readonly StoreViewModel _storeViewModel;

        public StoresController(IDbContext context)
        {
            _storeViewModel =new StoreViewModel(context);
        }
       
        // GET: 
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            _storeViewModel.HandleRequest();
            return View(_storeViewModel);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult show()
        {
            _storeViewModel.HandleRequest();
            return View(_storeViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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