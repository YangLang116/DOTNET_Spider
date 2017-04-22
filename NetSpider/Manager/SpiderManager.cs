using NetSpider.Bean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Util;

namespace NetSpider.Manager
{
    class SpiderManager
    {
        private String needValue;
        private NodeFilter nodefilter;

        public SpiderManager(NodeFilter nodefilter,String needValue)
        {
            this.nodefilter = nodefilter;
            this.needValue = needValue;
        }

        public List<String> start(String htmlContent)
        {
            Parser parser = new Parser();
            parser.InputHTML = htmlContent;
            NodeList nodelist =  parser.Parse(nodefilter);
            int size = nodelist.Size();
            List<String> results = new List<String>();
            for(int i = 0; i < size; i++)
            {
                INode node = nodelist.ElementAt(i);
                if(node is INode)
                {
                    ITag tag = node as ITag;
                    if(needValue == "href")
                    {
                        results.Add(tag.GetAttribute(needValue));
                    }
                    else
                    {
                        results.Add(tag.FirstChild.GetText());
                    }
                        
                }
            }
            return results;
        }
    }
}
