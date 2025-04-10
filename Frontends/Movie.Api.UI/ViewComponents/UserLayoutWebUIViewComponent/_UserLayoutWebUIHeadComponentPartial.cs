using Microsoft.AspNetCore.Mvc;

namespace Movie.Api.UI.ViewComponents.UserLayoutWebUIViewComponent
{
    public class _UserLayoutWebUIHeadComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
