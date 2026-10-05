namespace MovieApi.DTOs.DTOs.CastDTOs
{
    public class ResultCastDto
    {
        public int CastID { get; set; }
        public string? CastTitle { get; set; }
        public string? CastName { get; set; }
        public string? CastSurname { get; set; }
        public string? CastImageURL { get; set; }
        public string? CastOverview { get; set; }
        public string? CastBiography { get; set; }

        public string CastFullName => !string.IsNullOrEmpty(CastName) || !string.IsNullOrEmpty(CastSurname)
            ? $"{CastName} {CastSurname}".Trim()
            : CastTitle ?? "";

        public string FullName => CastFullName;
        public string ImageUrl => CastImageURL ?? "";
        public string Overview => CastOverview ?? "";
        public string Biography => CastBiography ?? "";
    }
}
