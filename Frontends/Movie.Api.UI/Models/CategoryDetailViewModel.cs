using System.Collections.Generic;
using MovieApi.DTOs.DTOs.AdminSeriesDTOs;
using MovieApi.DTOs.DTOs.MovieDTO;

namespace Movie.Api.UI.Models
{
    public class CategoryDetailViewModel
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = "";
        public List<ResultMovieDTO> Movies { get; set; } = new List<ResultMovieDTO>();
        public List<AdminResultSeriesDTO> Series { get; set; } = new List<AdminResultSeriesDTO>();
    }
}
