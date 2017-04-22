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
        private String htmlContent;
        private SpiderNodeFilter nodefilter;

        public SpiderManager(String htmlContent, SpiderNodeFilter nodefilter)
        {
            this.htmlContent = htmlContent;
            this.nodefilter = nodefilter;
        }

        public List<String> start(String needbykey)
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
                    if(needbykey == "href")
                    {
                        results.Add(tag.GetAttribute(needbykey));
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
