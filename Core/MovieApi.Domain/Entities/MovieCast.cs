namespace MovieApi.Domain.Entities
{
    public class MovieCast
    {
        public int MovieCastID { get; set; }
        public int MovieID { get; set; }
        public Movie Movie { get; set; } = null!;
        public int CastID { get; set; }
        public Cast Cast { get; set; } = null!;
        public string? CharacterName { get; set; }
    }
}
