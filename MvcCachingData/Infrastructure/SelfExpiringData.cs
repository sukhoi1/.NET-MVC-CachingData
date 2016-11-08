using System;
using System.Web.Caching;

namespace MvcState.Infrastructure
{
    public class SelfExpiringData<T> : CacheDependency
    {
        private T dataValue;
        private int requestCount = 0;
        private int requestLimit;

        public T Value
        {
            get
            {
                if (this.requestCount++ >= this.requestLimit)
                {
                    NotifyDependencyChanged(this, EventArgs.Empty);
                }

                return this.dataValue;
            }
        }

        public SelfExpiringData(T data, int limit)
        {
            this.dataValue = data;
            this.requestLimit = limit;
        }
    }
}