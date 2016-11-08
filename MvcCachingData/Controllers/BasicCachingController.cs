using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MvcState.Controllers
{
    public class BasicCachingController : Controller
    {
        public ActionResult Index()
        {
            return View((long?)HttpContext.Cache["pageLength"]);
        }

        [HttpPost]
        public async Task<ActionResult> PopulateCache() 
        {
            HttpResponseMessage result = await new HttpClient().GetAsync("http://apress.com");
            HttpContext.Cache["pageLength"] = result.Content.Headers.ContentLength;
            return RedirectToAction("Index");
        }
    }
}