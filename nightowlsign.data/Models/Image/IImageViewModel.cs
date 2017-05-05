using System.Collections.Generic;
using System.Web;
using ImageStorage;

namespace nightowlsign.data.Models.Image
{
    public interface IImageViewModel
    {
        bool Selected { get; set; }
        List<ImagesAndSign> Images { get; set; }
        ImagesAndSign SearchEntity { get; set; }
        data.Image Entity { get; set; }
        HttpPostedFileBase File { get; set; }
        bool LastImage { get; set; }
        UploadedImage ImageToUpload { get; set; }
        string CommandString { get; set; }
        string Message { get; set; }
        int? SearchSignId { get; set; }
        IList<SelectListItem> SignList { get; }
        string Mode { get; set; }
        string EventCommand { get; set; }
        string EventArgument { get; set; }
        bool IsDetailVisible { get; set; }
        bool IsSearchVisible { get; set; }
        bool IsListAreaVisible { get; set; }
        bool IsValid { get; set; }
        List<KeyValuePair<string, string>> ValidationErrors { get; set; }
        void HandleRequest();
    }
}