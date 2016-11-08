using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.Mvc;

namespace MvcState.Controllers
{
    public class FileDependencyController : Controller
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
            CacheDependency dependency = new CacheDependency(Request.MapPath("~/data.txt"));
            HttpContext.Cache.Insert("pageLength", data, dependency);
            return RedirectToAction("Index");
        }
    }
}