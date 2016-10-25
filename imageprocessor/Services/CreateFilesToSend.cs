using System;
using System.Collections.Generic;

using ImageProcessor.CP5200;
using ImageProcessor.Enums;
using nightowlsign.data;
using nightowlsign.data.Models;
using nightowlsign.data.Models.Signs;
using System.Web;
using System.Linq;
using System.Net;

namespace ImageProcessor.Services

{
    public class CreateFilesToSend
    {
        public List<string> ProgramFiles { get; set; }
        public string PlaybillFileName { get; set; }

        private readonly List<SignDto> _signSizesForSchedule;
        private readonly List<ImageSelect> _imagesToSend;
        private readonly SignManager sm = new SignManager();

        public CreateFilesToSend(List<SignDto> signsForSchedule, List<ImageSelect> imagesToSend)
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
            PlaybillFileName = GeneratePlayBillFileName(scheduleName);
            var PlayItemNo = -1;
            var PeriodToShowImage = 0xA; //Seconds
            byte colourMode = 0x77;
            ProgramFiles = new List<string>();
            foreach (var signSize in _signSizesForSchedule)
            {
                //  ushort screenWidth = (ushort)(signSize.Width);
                //  ushort screenHeight = (ushort)(signSize.Height);
                var screenWidth = 64;
                var screenHeight = 64;

                PlayBillFiles cp5200 = new PlayBillFiles(screenWidth, screenHeight, PeriodToShowImage, colourMode);
            
                if (cp5200.playBill_Create())
                {
                    cp5200.Playbill_SetProperty(0, 1);
                    var counter = 1;
                   
                    foreach (var image in _imagesToSend)
                    {
                        var sCounter = string.Format("{0:0000}0000", counter);
                        if (cp5200.Program_Create())
                        {
                            cp5200.AddPlayWindow();

                            var tempFileName = GenerateImageFileName(sCounter, image);
                            PlayItemNo = cp5200.Program_AddPicture(tempFileName, (int)RenderMode.Stretch_to_fit_the_window, 0, 0, PeriodToShowImage, 0);
                            var programFileName = GenerateProgramFileName(sCounter);
                            ProgramFiles.Add(programFileName);
                            if (cp5200.Program_SaveFile(programFileName) > 1)
                            {
                                cp5200.DestroyProgram();
                            };
                            cp5200.Playbill_AddFile(programFileName);
                        }
                        counter += 1;
                        //Now Create the playBillFile
                    }
                    cp5200.Playbill_SaveToFile(PlaybillFileName);
                }
            }
        }

       
        private string GenerateImageFileName(string sCounter, ImageSelect image)
        {
            string tempFileName = HttpContext.Current.Server.MapPath(string.Concat("/playBillFiles/images/", sCounter, ".jpg"));
            System.IO.File.Delete(tempFileName);
            WebClient webClient = new WebClient();
            webClient.DownloadFile(image.ImageUrl, tempFileName);
            return tempFileName;
        }

        private string GeneratePlayBillFileName(string scheduleName)
        {
            return HttpContext.Current.Server.MapPath(string.Concat("/playBillFiles/", strip(scheduleName), ".lpp"));
        }

        private string GenerateProgramFileName(string sCounter)
        {
            var fileAndPath = HttpContext.Current.Server.MapPath(string.Concat("/playBillFiles/", sCounter, ".lpb"));
            System.IO.File.Delete(fileAndPath);
            return fileAndPath;
        }

        private string strip(string scheduleName)
        {
            var invalidChars = " *./\\$";
            string invalidCharsRemoved = new string(scheduleName
              .Where(x => !invalidChars.Contains(x))
              .ToArray());
            return invalidCharsRemoved;
        }
    }
}



