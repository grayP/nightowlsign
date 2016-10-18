using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Configuration;
using ImageProcessor.CP5200;
using ImageProcessor.Enums;
using nightowlsign.data;
using nightowlsign.data.Models;
using nightowlsign.data.Models.Signs;
using System.Web;

namespace ImageProcessor.Services

{
    public class CreatePlayBill
    {
        private readonly List<SignDto> _SignSizesForSchedule;
        private readonly List<ImageSelect> _imagesToSend;
        private readonly SignManager sm = new SignManager();



        public CreatePlayBill(List<SignDto> signsForSchedule, List<ImageSelect> imagesToSend)
        {
            _SignSizesForSchedule = signsForSchedule;
            _imagesToSend = imagesToSend;
        }

        public void PopulateSignList()
        {
            foreach (var signDto in _SignSizesForSchedule)
            {
                sm.Find(signDto.Id);
            }
        }
        public void GeneratethePlayBillFile(string scheduleName)
        {
            int PlayItemNo = -1;

            ushort PeriodToShowImage = 6; //Seconds
            byte colourMode = 0x77;
            foreach (var signSize in _SignSizesForSchedule)
            {
              //  ushort screenWidth = (ushort)(signSize.Width);
              //  ushort screenHeight = (ushort)(signSize.Height);
                ushort screenWidth = (ushort)(64);
                ushort screenHeight = (ushort)(64);

                PlayBillFiles cp5200 = new PlayBillFiles(screenWidth, screenHeight, PeriodToShowImage, colourMode);

                if (cp5200.Program_Create())
                {
                   cp5200.AddPlayWindow();
                     
               }

                foreach (var image in _imagesToSend)
                {
                    PlayItemNo = cp5200.Program_AddPicture(image.ImageUrl, (int)RenderMode.Stretch_to_fit_the_window, 0, 0, PeriodToShowImage, 0);
                }
                var programFileName =
                    HttpContext.Current.Server.MapPath(string.Concat("/playBillFiles/", strip(scheduleName), ".lpp"));
               cp5200.Program_SaveFile(programFileName);

               //Now Create the playBillFile
                if (cp5200.playBill_Create())
                {
                    cp5200.Playbill_SetProperty(0, 1);
                }
                var playBillNumber = cp5200.Playbill_AddFile(programFileName);
                //And now save it
                var playbillFileName =
                    HttpContext.Current.Server.MapPath(string.Concat("/playBillFiles/", strip(scheduleName), ".lpl"));
                cp5200.Playbill_SaveToFile(playbillFileName);


            }

        }

        private object strip(string scheduleName)
        {
            scheduleName = scheduleName.Replace(" ", "");
            scheduleName = scheduleName.Replace("*", "");
            scheduleName = scheduleName.Replace(".", "");
            scheduleName = scheduleName.Replace("/", "");
            scheduleName = scheduleName.Replace("$", "");
            return scheduleName;

        }
    }
}



