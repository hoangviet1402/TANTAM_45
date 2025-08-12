using System.Web.Mvc;

namespace TanTamApi.Controllers
{
    /// <summary>
    /// MVC Controller cho ESP32-CAM streaming interface
    /// </summary>
    public class ESP32MvcController : Controller
    {
        /// <summary>
        /// Hiển thị ESP32 streaming test page
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }
        
        /// <summary>
        /// Hiển thị streaming dashboard
        /// </summary>
        public ActionResult Dashboard()
        {
            return View();
        }
        
        /// <summary>
        /// Hiển thị stream management
        /// </summary>
        public ActionResult Management()
        {
            return View();
        }
    }
} 