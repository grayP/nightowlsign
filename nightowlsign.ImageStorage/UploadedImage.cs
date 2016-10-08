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

     
    }
    public class Thumbnail
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Url { get; set; }
        public Bitmap bitmap { get; set; }
    }




}
