using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winista.Text.HtmlParser;

namespace NetSpider.Bean
{
    class SpiderNodeFilter : NodeFilter
    {
        private Dictionary<String, String> mRules;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="rules">节点过滤的规则</param>
        public SpiderNodeFilter(Dictionary<String,String> rules)
        {
            mRules = rules;
        }

        /// <summary>
        /// 设置过滤条件
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool Accept(INode node)
        {
          Boolean isAccept = false;
          if(node is ITag)
            {
                ITag tagNode = (ITag)node;
                if(tagNode.Attributes != null && tagNode.Attributes.Count > 0)
                {
                    int totalNum = mRules.Count;
                    int currentNum = 0;
                    foreach (var item in mRules)
                    {
                        String getValue = tagNode.GetAttribute(item.Key);
                        if(getValue == item.Value)
                        {
                            currentNum++;
                        }
                    }
                    if(currentNum == totalNum)
                    {
                        isAccept = true;
                    }
                }
            }
            return isAccept;
        }
    }
}
