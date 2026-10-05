namespace MovieApi.Domain.Entities
{
    public class SeriesTag
    {
        public int SeriesTagID { get; set; }
        public int SeriesID { get; set; }
        public Series Series { get; set; } = null!;
        public int TagID { get; set; }
        public Tag Tag { get; set; } = null!;
    }
}
