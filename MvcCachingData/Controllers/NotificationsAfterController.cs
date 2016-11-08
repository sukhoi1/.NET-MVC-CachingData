using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.Mvc;
using MvcState.Infrastructure;

namespace MvcState.Controllers
{
    public class NotificationsAfterController : Controller
    {
        public ActionResult Index()
        {
            SelfExpiringData<long?> seData =
                (SelfExpiringData<long?>)HttpContext.Cache["pageLength"];
            return View(seData == null ? null : seData.Value);
        }

        [HttpPost]
        public async Task<ActionResult> PopulateCache()
        {
            HttpResponseMessage result = await new HttpClient().GetAsync("http://apress.com");
            long? data = result.Content.Headers.ContentLength;

            SelfExpiringData<long?> seData = new SelfExpiringData<long?>(data, 3);
            HttpContext.Cache.Insert("pageLength", seData, seData, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration,
                CacheItemPriority.Normal, HandleNotification);

            return RedirectToAction("Index");
        }

        private void HandleNotification(string key, object data, CacheItemRemovedReason reason)
        {
            System.Diagnostics.Debug.WriteLine("Item {0} removed. ({1})", key, Enum.GetName(typeof(CacheItemRemovedReason), reason));
        }
    }
}