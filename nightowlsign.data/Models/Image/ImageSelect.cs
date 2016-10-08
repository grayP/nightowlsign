namespace nightowlsign.data.Models
{
    public class ImageSelect
    {
        public bool Selected { get; set; }
        public int Id { get; set; }
        public int ScheduleId { get; set; }
        public int ImageId { get; set; }
        public string Name { get; set; }
        public byte[] ThumbNail { get; set; }
        public int SignSize { get; set; }
        public string ImageUrl { get; set; }
    }

    
}
