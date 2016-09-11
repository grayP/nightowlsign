using ImageStorage;
using nightowlsign.data.Models.Images;

using System.Collections.Generic;
using System.Web.Mvc;



namespace nightowlsign.Controllers
{

    public class ImageController : Controller
    {

        private readonly IImageService _imageService = new ImageService();
        private ImageViewModel imv = new ImageViewModel();
        // GET: Image

        [AllowAnonymous]
        public ActionResult Index(int? scheduleId)
        {

            imv.searchRegattaID = scheduleId ?? 0;
            imv.HandleRequest();
            imv.imageToUpload.Status = false;

            return View(imv);
        }
        [AllowAnonymous]
        public ActionResult show(int? RegattaID)
        {

            imv.searchRegattaID = RegattaID ?? 0;
            imv.HandleRequest();
            imv.imageToUpload.Status = false;

            return View(imv);
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