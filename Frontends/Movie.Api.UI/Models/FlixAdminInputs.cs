using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MovieApi.DTOs.DTOs.AdminCategoryDTOs;
using MovieApi.DTOs.DTOs.CastDTOs;
using MovieApi.DTOs.DTOs.TagDTOs;

namespace Movie.Api.UI.Models;

public class FlixItemInput
{
    [Required, RegularExpression("movie|series")] public string Kind { get; set; } = "movie";
    [Required, StringLength(500)] public string Title { get; set; } = "";
    [StringLength(1000)] public string? ImageUrl { get; set; }
    [Required, StringLength(10000)] public string Description { get; set; } = "";
    [Range(0, 10)] public decimal Rating { get; set; }
    [Range(1, 10000)] public int Duration { get; set; } = 90;
    [Range(1880, 2200)] public int Year { get; set; } = DateTime.UtcNow.Year;
    [DataType(DataType.Date)] public DateTime ReleaseDate { get; set; } = DateTime.Today;
    [Range(1, int.MaxValue)] public int CategoryId { get; set; }
    public bool Published { get; set; }
    public List<int> CastIds { get; set; } = [];
    public List<int> TagIds { get; set; } = [];
    [ValidateNever] public List<AdminResultCategoryDTO> Categories { get; set; } = [];
    [ValidateNever] public List<ResultCastDto> Cast { get; set; } = [];
    [ValidateNever] public List<ResultTagDto> Tags { get; set; } = [];
}

public class FlixEditUserInput
{
    public bool IsLockedOut { get; set; }
    [Required] public string Id { get; set; } = "";
    public string Username { get; set; } = "";
    [Required, StringLength(100)] public string Name { get; set; } = "";
    [Required, StringLength(100)] public string Surname { get; set; } = "";
    [Required, EmailAddress] public string Email { get; set; } = "";
    [Phone] public string? PhoneNumber { get; set; }
    public List<MovieApi.DTOs.DTOs.AdminReviewDTOs.ResultAdminReviewDTO> Reviews { get; set; } = [];
}
