using System.Collections.Generic;
using MovieApi.DTOs.DTOs.CastDTOs;
using MovieApi.DTOs.DTOs.MovieDTO;
using MovieApi.DTOs.DTOs.TagDTOs;

namespace Movie.Api.UI.Models
{
    public class MovieDetailOverviewViewModel
    {
        public ResultMovieDTO? Movie { get; set; }
        public List<ResultCastDto> Casts { get; set; } = new();
        public List<ResultTagDto> Tags { get; set; } = new();
    }
}
