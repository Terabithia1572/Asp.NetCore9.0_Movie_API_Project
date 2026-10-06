using System.Collections.Generic;
using MovieApi.DTOs.DTOs.AdminReviewDTOs;
using MovieApi.DTOs.DTOs.CastDTOs;
using MovieApi.DTOs.DTOs.MovieDTO;
using MovieApi.DTOs.DTOs.TagDTOs;

namespace Movie.Api.UI.Models
{
    public class MovieDetailOverviewViewModel
    {
        public ResultMovieDTO? Movie { get; set; }
        public string CategoryName { get; set; } = "Film";
        public List<ResultCastDto> Casts { get; set; } = new List<ResultCastDto>();
        public List<ResultTagDto> Tags { get; set; } = new List<ResultTagDto>();
        public List<ResultMovieDTO> RelatedMovies { get; set; } = new List<ResultMovieDTO>();
        public List<ResultAdminReviewDTO> Reviews { get; set; } = new List<ResultAdminReviewDTO>();
        public bool IsFavorited { get; set; }
    }
}
