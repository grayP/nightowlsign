using System;
using System.Collections.Generic;

using ImageProcessor.CP5200;
using ImageProcessor.Enums;
using nightowlsign.data;
using nightowlsign.data.Models;
using nightowlsign.data.Models.Signs;
using System.Web;
using System.Linq;

namespace ImageProcessor.Services

{
    public class CreatePlayBill
    {
        private readonly List<SignDto> _signSizesForSchedule;
        private readonly List<ImageSelect> _imagesToSend;
        private readonly SignManager sm = new SignManager();



        public CreatePlayBill(List<SignDto> signsForSchedule, List<ImageSelect> imagesToSend)
        {
            _signSizesForSchedule = signsForSchedule;
            _imagesToSend = imagesToSend;
        }

        public void PopulateSignList()
        {
            foreach (var signDto in _signSizesForSchedule)
            {
                sm.Find(signDto.Id);
            }
        }
        public void GeneratethePlayBillFile(string scheduleName)
        {
            int PlayItemNo = -1;

            int PeriodToShowImage = 0xA; //Seconds
            byte colourMode = 0x77;
            foreach (var signSize in _signSizesForSchedule)
            {
                //  ushort screenWidth = (ushort)(signSize.Width);
                //  ushort screenHeight = (ushort)(signSize.Height);
                int screenWidth = 64;
                int screenHeight = 64;

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
                if (cp5200.Program_SaveFile(programFileName) > 1)
                {
                    cp5200.DestroyProgram();
                };

                //Now Create the playBillFile
                if (cp5200.playBill_Create())
                {
                    cp5200.Playbill_SetProperty(0, 1);
                }
                if (cp5200.Playbill_AddFile(programFileName) >= 0)
                {
                    var playbillFileName =
                        HttpContext.Current.Server.MapPath(string.Concat("/playBillFiles/", strip(scheduleName), ".lpl"));
                    cp5200.Playbill_SaveToFile(playbillFileName);

                    //And now save it
                    cp5200.SendFiletoSign(programFileName, playbillFileName);
                };
            }

        }

        private object strip(string scheduleName)
        {
            var invalidChars = " *./$";

            string invalidCharsRemoved = new string(scheduleName
              .Where(x => !invalidChars.Contains(x))
              .ToArray());

            return invalidCharsRemoved;

        }
    }
}



