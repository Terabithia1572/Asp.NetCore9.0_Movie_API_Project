using Microsoft.AspNetCore.Mvc;

namespace Movie.Api.UI.ViewComponents.UserLayoutWebUIViewComponent
{
    public class _UserLayoutWebUIHeroComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
