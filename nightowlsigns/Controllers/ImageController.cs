using ImageStorage;
using nightowlsign.data.Models.Image;

using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Web.Mvc;



namespace nightowlsign.Controllers
{

    public class ImageController : Controller
    {

        private ImageViewModel ImageViewModel = new ImageViewModel();
        // GET: Image

        [AllowAnonymous]
        public ActionResult Index(int? scheduleId)
        {
            ImageViewModel.SearchSignId = scheduleId ?? 0;
            ImageViewModel.HandleRequest();
            ImageViewModel.ImageToUpload.Status = false;

            return View(ImageViewModel);
        }
        [AllowAnonymous]
        public ActionResult Show(ImageViewModel imageViewModel)
        {
            ImageViewModel.SearchSignId = imageViewModel.SearchSignId ?? 0;
            ImageViewModel.HandleRequest();
          return View("Index", ImageViewModel);
        }

       
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Index(FormCollection formCollection, ImageViewModel iVm)
        {

            iVm.IsValid = ModelState.IsValid;
            if (Request != null)
            {
                iVm.file = Request.Files["file"];
             }

            iVm.HandleRequest();

            if (iVm.IsValid)
            {
                ModelState.Clear();
            }
            else
            {
                foreach (KeyValuePair<string, string> item in iVm.ValidationErrors)
                {
                    ModelState.AddModelError(item.Key, item.Value);
                }
            }
            return View(iVm);
        }
   }

}