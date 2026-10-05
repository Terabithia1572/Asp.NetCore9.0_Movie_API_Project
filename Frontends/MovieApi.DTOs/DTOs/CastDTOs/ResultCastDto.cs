namespace MovieApi.DTOs.DTOs.CastDTOs
{
    public class ResultCastDto
    {
        public int CastID { get; set; }
        public string CastTitle { get; set; } = null!;
        public string CastName { get; set; } = null!;
        public string CastSurname { get; set; } = null!;
        public string CastImageURL { get; set; } = null!;
        public string FullName => $"{CastName} {CastSurname}".Trim();
    }
}
