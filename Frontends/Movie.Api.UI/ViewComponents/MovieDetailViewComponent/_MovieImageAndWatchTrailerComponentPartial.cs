using Microsoft.AspNetCore.Mvc;

namespace Movie.Api.UI.ViewComponents.MovieDetailViewComponent
{
    public class _MovieImageAndWatchTrailerComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
