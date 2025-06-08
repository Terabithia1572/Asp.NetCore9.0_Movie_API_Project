using Microsoft.AspNetCore.Mvc;

namespace Movie.Api.UI.ViewComponents.MovieDetailViewComponent
{
    public class _MovieDetailOverviewComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
