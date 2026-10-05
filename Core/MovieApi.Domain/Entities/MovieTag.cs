namespace MovieApi.Domain.Entities
{
    public class MovieTag
    {
        public int MovieTagID { get; set; }
        public int MovieID { get; set; }
        public Movie Movie { get; set; } = null!;
        public int TagID { get; set; }
        public Tag Tag { get; set; } = null!;
    }
}
