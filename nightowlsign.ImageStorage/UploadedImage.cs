using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System;
using System.Web;

namespace ImageStorage
{
    public class UploadedImage
    {
        public UploadedImage()
        {
            // hard-coded to a single thumbnail at 200 x 300 for now
            Thumbnails = new List<Thumbnail>();
        }
        public int Id { get; set; }
        public bool Status { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public string Url { get; set; }
        public List<Thumbnail> Thumbnails { get; set; }
        public DateTime DateTaken { get; set; }
        public int SignId { get; set; }
        public int SignHeight { get; set; }
        public int SignWidth { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public bool AspectRatiosMatch => TheAspectRatiosMatch(SignWidth, SignHeight, ImageWidth, ImageHeight);


        public bool TheAspectRatiosMatch(int x1, int y1, int x2, int y2)
        {
            if (y1 < 1 || y2 < 1) return false;
            var comparedAspectRatios = Math.Round(((double)y1 / x1 * x2 / y2), 2);
            return .9 < comparedAspectRatios && comparedAspectRatios < 1.1;
        }
    }
    public class Thumbnail
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Url { get; set; }
        public Bitmap Bitmap { get; set; }
    }
}
