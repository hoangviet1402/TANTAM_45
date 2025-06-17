using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TanTamApi.Controllers
{
    public class ApiHomeController : ApiController
    {
        [HttpPost]
        [HttpGet]
        public HttpResponseMessage RIndex()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}