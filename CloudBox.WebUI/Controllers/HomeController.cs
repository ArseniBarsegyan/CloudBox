using System.Web.Mvc;

namespace CloudBox.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Manage", "Account");
            }
            return View();
        }
    }
}
