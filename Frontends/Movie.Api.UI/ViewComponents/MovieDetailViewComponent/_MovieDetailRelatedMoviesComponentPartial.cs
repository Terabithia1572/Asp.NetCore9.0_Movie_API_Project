using Microsoft.AspNetCore.Mvc;

namespace Movie.Api.UI.ViewComponents.MovieDetailViewComponent
{
    public class _MovieDetailRelatedMoviesComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
