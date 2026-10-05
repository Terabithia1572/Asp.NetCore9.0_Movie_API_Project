using System.Collections.Generic;
using MovieApi.DTOs.DTOs.UserFavoriteDTOs;

namespace Movie.Api.UI.Models
{
    public class UserFavoriteViewModel
    {
        public List<ResultFavoriteMovieDto> FavoriteMovies { get; set; } = new();
        public List<ResultFavoriteSeriesDto> FavoriteSeries { get; set; } = new();
    }
}
