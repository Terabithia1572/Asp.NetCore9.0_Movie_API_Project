namespace MovieApi.Domain.Entities
{
    public class SeriesCast
    {
        public int SeriesCastID { get; set; }
        public int SeriesID { get; set; }
        public Series Series { get; set; } = null!;
        public int CastID { get; set; }
        public Cast Cast { get; set; } = null!;
        public string? CharacterName { get; set; }
    }
}
