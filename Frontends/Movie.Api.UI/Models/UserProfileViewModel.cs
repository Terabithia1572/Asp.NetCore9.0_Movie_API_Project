using System.Collections.Generic;
using MovieApi.DTOs.DTOs.UserFavoriteDTOs;

namespace Movie.Api.UI.Models
{
    public class UserProfileViewModel
    {
        public string UserId { get; set; } = "";
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        
        public int FavoriteMoviesCount { get; set; }
        public int FavoriteSeriesCount { get; set; }
        public int ReviewsCount { get; set; }

        public List<ResultFavoriteMovieDto> FavoriteMovies { get; set; } = new List<ResultFavoriteMovieDto>();
        public List<ResultFavoriteSeriesDto> FavoriteSeries { get; set; } = new List<ResultFavoriteSeriesDto>();

        public UpdateProfileInputModel UpdateModel { get; set; } = new UpdateProfileInputModel();
        public ChangePasswordInputModel PasswordModel { get; set; } = new ChangePasswordInputModel();
    }

    public class UpdateProfileInputModel
    {
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
    }

    public class ChangePasswordInputModel
    {
        public string CurrentPassword { get; set; } = "";
        public string NewPassword { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
    }
}
