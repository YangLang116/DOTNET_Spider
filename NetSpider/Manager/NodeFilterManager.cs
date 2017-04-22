using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Filters;

namespace NetSpider.Manager
{
    class NodeFilterManager
    {

        private List<NodeFilter> mNodeFilters; 

        private NodeFilterManager()
        {
            mNodeFilters = new List<NodeFilter>();
         }

        private static NodeFilterManager mNodeFilterManager;

        public static NodeFilterManager getInstance()
        {
            if(mNodeFilterManager == null)
            {
                mNodeFilterManager = new NodeFilterManager();
            }
            return mNodeFilterManager;
        }

        /// <summary>
        /// 添加一个节点过滤器
        /// </summary>
        /// <param name="filter">过滤器</param>
        public void addNodeFilter(NodeFilter filter)
        {
            mNodeFilters.Add(filter);
        }


        /// <summary>
        /// 将所有节点过滤器统一起来
        /// </summary>
        /// <returns>总过滤器</returns>
        public NodeFilter makeNodeFilter()
        {
            int totalNum = mNodeFilters.Count;
            NodeFilter[] nodefilters = new NodeFilter[totalNum];
            for(int i=0;i<totalNum;i++)
            {
                nodefilters[i] = mNodeFilters.ElementAt(i);
            }
            OrFilter orFilter =  new OrFilter();
            orFilter.Predicates = nodefilters;
            return orFilter;
        }

    }
}
