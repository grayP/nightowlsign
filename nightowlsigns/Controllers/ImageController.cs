using ImageStorage;
using nightowlsign.data.Models.Image;
using nightowlsign.Models;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Web;
using System.Web.Mvc;

namespace nightowlsign.Controllers
{
        
    public class ImageController : Controller
    {
        private readonly IImageViewModel _imageViewModel;

        public ImageController(IImageViewModel imageViewModel)
        {
            _imageViewModel = imageViewModel;
        }

        [AllowAnonymous]
        public ActionResult Index(int? SignId)
        {

            _imageViewModel.SearchSignId = Helper.GetSetSignSize(SignId);
            _imageViewModel.HandleRequest();
            _imageViewModel.ImageToUpload.Status = false;

            return View(_imageViewModel);
        }
        [AllowAnonymous]
        public ActionResult Show(ImageViewModel imageViewModel, int? SignId)
        {
            _imageViewModel.SearchSignId = Helper.GetSetSignSize(SignId); 
            _imageViewModel.HandleRequest();
            return View("Index", _imageViewModel);
        }


        [HttpPost]
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult Index(FormCollection formCollection, ImageViewModel iVm, int? signId)
        {
            iVm.SearchSignId = Helper.GetSetSignSize(signId); 
            iVm.IsValid = ModelState.IsValid;
            if (Request != null)
            {
                var files = HttpContext.Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    iVm.File = files[i];
                    if (i == files.Count-1){iVm.LastImage = true;}
                    iVm.HandleRequest();
                }
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