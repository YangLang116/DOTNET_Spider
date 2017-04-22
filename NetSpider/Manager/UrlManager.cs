using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSpider.Manager
{
    class UrlManager
    {
        private Queue<String> newUrls;
        private Queue<String> oldUrls;

        private UrlManager()
        {
            newUrls = new Queue<string>();
            oldUrls = new Queue<string>();
        }

        private static UrlManager mUrlManager;
        public static UrlManager getInstance()
        {
            if(mUrlManager == null)
            {
                mUrlManager = new UrlManager();
            }
            return mUrlManager;
        }

        public String getNewUrl()
        {
            String currentUrl = newUrls.Dequeue();
            oldUrls.Enqueue(currentUrl);
            return currentUrl;
        }

        public void addNewUrl(String url)
        {
            if(!newUrls.Contains(url) && !oldUrls.Contains(url))
            {
                newUrls.Enqueue(url);
            }
        }
    }
}
