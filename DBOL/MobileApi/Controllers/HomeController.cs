using System.Web.Mvc;

namespace BanCaMobileApi.Controllers
{
    public class HomeController : Controller
    {
        [TanTamApi.JWT.Middleware.Authorize]
        public ActionResult Index()
        {
            return Content("{\"Result\":1003,\"Code\":0,\"Message\":\"Unexisted api\",\"Data\":null}");
        }


        public ActionResult Index2()
        {
            return Content("{\"Result\":1003,\"Code\":0,\"Message\":\"Unexisted api\",\"Data\":null}");
        }
    }
}
