using ImageStorage;
using System;
using System.Collections.Generic;
using System.Web;


using System.Threading.Tasks;
using System.Linq;

namespace nightowlsign.data.Models.Images
{
    public class ImageViewModel : BaseModel.ViewModelBase
    {

        public ImageViewModel() : base()
        {
           

        }

        //Properties--------------
        public List<Image> Images { get; set; }
        public Image SearchEntity { get; set; }
        public Image Entity { get; set; }
        public HttpPostedFileBase file { get; set; }

        public UploadedImage imageToUpload { get; set; }

        public string CommandString { get; set; }
        public string Message { get; set; }
        public int? searchRegattaID { get; set; }
        //public IList<RegattaSelectItem> RegattaList
        //{
        //    get
        //    {
        //        using (nightowlsign_Entities db = new nightowlsign_Entities())
        //        {
        //            var SelectList = (from item in
        //                              db.Store.OrderBy(x => x.id)
        //                              select new RegattaSelectItem()
        //                              {
        //                                  RegattaID = item.Id,
        //                                  Regatta = item.RegattaName
        //                              }).ToList();

        //            return SelectList;
        //        }

        //    }


        //    set { }
        //}




        //---------------------------------------------------------------
        protected override void Init()
        {
            Images = new List<Image>();
            SearchEntity = new Image();
            Entity = new Image();
            Entity.DateTaken = DateTime.Now;

            
            imageToUpload = new UploadedImage();
           // imageToUpload.RegattaID = 0;
            imageToUpload.DateTaken = DateTime.Now;
            base.Init();
        }

        public override void HandleRequest()
        {
            switch (EventCommand.ToLower())
            {
                case "edit":
                case "save":
                    CommandString = "save";
                    break;

                case "add":
                   CommandString = "insert";
                    break;
                default:
                    CommandString = "";
                    break;
            }
            if (EventCommand.ToLower() == "insert")
            {
                var task = Task.Run(async () => { await Insert(); });
                task.Wait();
            }
            else
            {
                base.HandleRequest();
            }
        }

        protected override void ResetSearch()
        {
            SearchEntity = new Image();
        }

        protected override void Get()
        {
            ImageManager cmm = new ImageManager();
            SearchEntity.Caption = SearchEntity.Caption ;

            Images = cmm.Get(SearchEntity);
        }

        protected override void Edit()
        {
            ImageManager imm = new ImageManager();
            Entity = imm.Find(Convert.ToInt32(EventArgument));

            imageToUpload.Caption = Entity.Caption;
            imageToUpload.Id = Entity.Id;
            imageToUpload.Url = Entity.ImageURL;
            imageToUpload.DateTaken = Entity.DateTaken ?? DateTime.Now;

            base.Edit();
        }

        protected override void Add()
        {
            IsValid = true;
            //Entity = new Image();
            //Entity.Caption = "";
            imageToUpload = new UploadedImage();
            //imageToUpload.RegattaID = Convert.ToInt32(EventArgument ?? "0");
            base.Add();
        }

        protected override void Save()
        {
            ImageManager imm = new ImageManager();
            if (imm.Update(imageToUpload))
            {
                Mode = "List";
                Message = "Image successfully updated";
            }

            ValidationErrors = imm.ValidationErrors;

            base.Save();

        }


        protected async Task<Boolean> Insert()
        {
            bool success = false;
            ImageManager imm = new ImageManager();
            if (Mode == "Add")
            {
                ImageService _imageService = new ImageService();

                imageToUpload = await _imageService.CreateUploadedImage(file, imageToUpload);
                await _imageService.AddImageToBlobStorageAsync(imageToUpload);

                success = await imm.Insert(imageToUpload);
                if (success)
                {
                    Mode = "List";
                    Message = "Image successfully added";
                }
                else
                {
                    Message = "Error uploading image";
                };
            }
          
            ValidationErrors = imm.ValidationErrors;
            return success;
           
        }

        protected override void Delete()
        {
            ImageManager cmm = new ImageManager();
            Entity = cmm.Find(Convert.ToInt32(EventArgument));
            cmm.Delete(Entity);
            Get();
            base.Delete();

        }


    }
}
