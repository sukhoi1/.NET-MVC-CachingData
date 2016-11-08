using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.Mvc;

namespace MvcState.Controllers
{
    public class NoSlidingCacheController : Controller
    {
        public ActionResult Index()
        {
            return View((long?)HttpContext.Cache["pageLength"]);
        }

        [HttpPost]
        public async Task<ActionResult> PopulateCache()
        {
            HttpResponseMessage result = await new HttpClient().GetAsync("http://apress.com");
            long? data = result.Content.Headers.ContentLength;
            DateTime expiryTime = DateTime.Now.AddSeconds(30);
            HttpContext.Cache.Insert("pageLength", data, null, expiryTime, Cache.NoSlidingExpiration);
            return RedirectToAction("Index");
        }
    }
}