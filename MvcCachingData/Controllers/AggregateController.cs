using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.Mvc;
using MvcState.Infrastructure;

namespace MvcState.Controllers
{
    public class AggregateController : Controller
    {
        public ActionResult Index()
        {
            SelfExpiringData<long?> seData =
                (SelfExpiringData<long?>) HttpContext.Cache["pageLength"];
            return View(seData == null ? null : seData.Value);
        }

        [HttpPost]
        public async Task<ActionResult> PopulateCache()
        {
            HttpResponseMessage result = await new HttpClient().GetAsync("http://apress.com");
            long? data = result.Content.Headers.ContentLength;

            SelfExpiringData<long?> seData = new SelfExpiringData<long?>(data, 3);
            CacheDependency fileDep = new CacheDependency(Request.MapPath("~/data.txt"));
            AggregateCacheDependency aggDep = new AggregateCacheDependency();
            aggDep.Add(seData, fileDep);
            HttpContext.Cache.Insert("pageLength", seData, aggDep);

            return RedirectToAction("Index");
        }
    }
}