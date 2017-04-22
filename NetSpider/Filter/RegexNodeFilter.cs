using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winista.Text.HtmlParser;

namespace NetSpider.Filter
{
    class RegexNodeFilter:NodeFilter
    {
        private String key;
        private String value;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="rules">节点过滤的规则</param>
        public RegexNodeFilter(String key, String value)
        {
            this.key = key;
            this.value = value;
        }

        public bool Accept(INode node)
        {
            if(node is ITag)
            {
                ITag tag = node as ITag;
                if (tag.GetAttribute(key).StartsWith(value))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
