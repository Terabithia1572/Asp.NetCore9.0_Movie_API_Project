using Microsoft.AspNetCore.Mvc;

namespace Movie.Api.UI.ViewComponents.UserLayoutWebUIViewComponent
{
    public class _UserLayoutWebUIPreloaderComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
