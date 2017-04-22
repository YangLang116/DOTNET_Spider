using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetSpider.Utils
{
    class NetDownLoaderUtils
    {
        /// <summary>
        /// 获取网页源代码
        /// </summary>
        /// <param name="url">网络路径</param>
        /// <param name="handler">监听回调</param>
        public static void downloaderHtml(String url,DownloadStringCompletedEventHandler handler)
        {
            if(url == null || url == "")
            {
                return;
            }
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            webClient.DownloadStringCompleted += handler;
            webClient.DownloadStringAsync(new Uri(url));
        }

        /// <summary>
        /// 异步下载文件
        /// </summary>
        /// <param name="url">下载文件的网络地址</param>
        /// <param name="localfileDir">保存文件的本地路径</param>
        /// <param name="handler">监听回调</param>
        public static void downloaderFile(String url, String localfileDir, AsyncCompletedEventHandler handler)
        {
            if(url == null || url== "")
            {
                return;
            }
            String fileName = url.Substring(url.LastIndexOf("/"), url.Length);
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += handler;
            webClient.DownloadFileAsync(new Uri(url), localfileDir + fileName);
        }

    }
}
