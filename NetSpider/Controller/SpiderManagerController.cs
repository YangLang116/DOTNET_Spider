using NetSpider.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSpider.Controller
{
    class SpiderManagerController
    {
        private List<SpiderManager> mManagers;
        private SpiderManager mNextManager;

        private SpiderManagerController() {
            mManagers = new List<SpiderManager>();
        }
        private static SpiderManagerController mSpiderManagerController;
        public static SpiderManagerController getInstance()
        {
            if(mSpiderManagerController == null)
            {
                mSpiderManagerController = new SpiderManagerController();
            }
            return mSpiderManagerController;
        }

        public void inputNextManager(SpiderManager manager)
        {
            mNextManager = manager;
        }

        public void inputManager(SpiderManager manager)
        {
            mManagers.Add(manager);
        }

        public void startAllManager(String currentPageUrl,ref List<List<String>> table)
        {
            List<List<String>> data = new List<List<String>>();
           foreach (SpiderManager manager in mManagers)
            {
               List<String> results =  manager.start(currentPageUrl);
                data.Add(results);
            }
            table = data;
            //更新请求库
            List<String> currentUrl = mNextManager.start(currentPageUrl);
            if(currentUrl != null && currentUrl.Count > 0)
            {
                UrlManager urlManager = UrlManager.getInstance();
                foreach (String url in currentUrl)
                {
                    urlManager.addNewUrl(url);
                }
            }
            
        }
    }
}
