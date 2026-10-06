using System.Collections.Generic;
using MovieApi.DTOs.DTOs.AdminSeriesDTOs;
using MovieApi.DTOs.DTOs.CastDTOs;
using MovieApi.DTOs.DTOs.EpisodeDTOs;
using MovieApi.DTOs.DTOs.SeasonDTOs;
using MovieApi.DTOs.DTOs.TagDTOs;

namespace Movie.Api.UI.Models
{
    public class SeriesDetailViewModel
    {
        public AdminResultSeriesDTO Series { get; set; } = new AdminResultSeriesDTO();
        public string CategoryName { get; set; } = "";
        public List<ResultSeasonDto> Seasons { get; set; } = new List<ResultSeasonDto>();
        public Dictionary<int, List<ResultEpisodeDto>> EpisodesBySeason { get; set; } = new Dictionary<int, List<ResultEpisodeDto>>();
        public List<ResultCastDto> Casts { get; set; } = new List<ResultCastDto>();
        public List<ResultTagDto> Tags { get; set; } = new List<ResultTagDto>();
        public bool IsFavorited { get; set; }
    }
}
