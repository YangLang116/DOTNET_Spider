using NetSpider.Bean;
using NetSpider.Manager;
using NetSpider.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Lex;
using Winista.Text.HtmlParser.Util;

namespace NetSpider
{
    public partial class MainForm : Form
    {

        private String htmlContent;

        public MainForm()
        {
            InitializeComponent();
        }

        private void HtmlAnalysisBtn_Click(object sender, EventArgs e)
        {
            String htmlUrl = HtmlInputView.Text.Trim();
            if( htmlUrl == null || htmlUrl == "")
            {
                return;
            }
            Console.WriteLine("当前输入的是网址是：" + htmlUrl);
            if(htmlUrl.StartsWith("http://") || htmlUrl.StartsWith("https://"))
            {
                webBrowser.Url = new Uri(htmlUrl);
            }
            else
            {
                MessageBox.Show("路径不合法！");
                return;
            }
        }

        /// <summary>
        /// html页面加载完毕的回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            htmlContent = webBrowser.DocumentText;
            //开始TreeView的展示
            Lexer lexer = new Lexer(htmlContent);
            Parser parser = new Parser(lexer);
            NodeList htmlNodes = parser.Parse(null);
            this.HtmlTreeView.Nodes.Clear();
            this.HtmlTreeView.Nodes.Add("-----Content----");
            TreeNode treeRoot = this.HtmlTreeView.Nodes[0];
            for (int i = 0; i < htmlNodes.Count; i++)
            {
                this.RecursionHtmlNode(treeRoot, htmlNodes[i], false);
            }
        }

        #region 将Html内容绑定到TreeView上
        private void RecursionHtmlNode(TreeNode treeNode, INode htmlNode, bool siblingRequired)
        {
            if (htmlNode == null || treeNode == null) return;

            TreeNode current = treeNode;
            TreeNode content;
            if (htmlNode is ITag)
            {
                ITag tag = (htmlNode as ITag);
                if (!tag.IsEndTag())
                {
                    string nodeString = "<" + tag.TagName + ">  ";
                    if (tag.Attributes != null && tag.Attributes.Count > 0)
                    {
                        if (tag.Attributes["ID"] != null)
                        {
                            nodeString = nodeString + " id=\"" + tag.Attributes["ID"].ToString() + "\"";
                        }
                        if (tag.Attributes["CLASS"] != null)
                        {
                            nodeString = nodeString + " class=\"" + tag.Attributes["CLASS"].ToString() + "\"";
                        }
                        if (tag.Attributes["NAME"] != null)
                        {
                            nodeString = nodeString + " name=\"" + tag.Attributes["NAME"].ToString() + "\"";
                        }
                        if (tag.Attributes["HREF"] != null)
                        {
                            nodeString = nodeString + " href=\"" + tag.Attributes["HREF"].ToString() + "\"";
                        }
                    }

                    current = new TreeNode(nodeString);
                    treeNode.Nodes.Add(current);
                }
            }

            //获取节点间的内容
            if (htmlNode.Children != null && htmlNode.Children.Count > 0)
            {
                this.RecursionHtmlNode(current, htmlNode.FirstChild, true);
                content = new TreeNode(htmlNode.FirstChild.GetText());
                treeNode.Nodes.Add(content);
            }

            //the sibling nodes
            if (siblingRequired)
            {
                INode sibling = htmlNode.NextSibling;
                while (sibling != null)
                {
                    this.RecursionHtmlNode(treeNode, sibling, false);
                    sibling = sibling.NextSibling;
                }
            }
        }
        #endregion

        private void NodeItemSelectListener(object sender, EventArgs e)
        {
            //清空选择节点过滤器的信息
            filter_id.Checked = false;
            filter_class.Checked = false;
            filter_name.Checked = false;
            filter_id_input.Text = "";
            filter_name_input.Text = "";
            filter_class_input.Text = "";
           
            String selectNodeText = HtmlTreeView.SelectedNode.Text;
            if(selectNodeText == null || selectNodeText == "")
            {
                return;
            }
            //结点内容解析
            String[] splitResult = Regex.Split(selectNodeText, "   ");
            if(splitResult != null && splitResult.Length > 1)
            {
                String attrsStr = splitResult[1].Trim();
                String[] contentAttrPairs = Regex.Split(attrsStr, " ");
                int pairLength = contentAttrPairs.Length;
                for(int i = 0; i < pairLength; i++)
                {
                    String contentAttrPair = contentAttrPairs[i];
                    String[] keyValue = Regex.Split(contentAttrPair, "=");
                    String key = keyValue[0].Trim();
                    String value = keyValue[1].Substring(1, keyValue[1].Length - 2);
                    //绑定显示
                    switch (key)
                    {
                        case "id":
                            filter_id.Checked = true;
                            filter_id_input.Text = value;
                            break;

                        case "class":
                            filter_class.Checked = true;
                            filter_class_input.Text = value;
                            break;

                        case "name":
                            filter_name.Checked = true;
                            filter_name_input.Text = value;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 当添加添加时添加一个提取规则
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addFilterBtn_click(object sender, EventArgs e)
        {
            if(htmlContent == null || htmlContent == "")
            {
                return;
            }
            Dictionary<String, String> rules = null;
            if (filter_id.Checked && filter_id_input.Text != "")
            {
                if(rules == null)
                {
                    rules = new Dictionary<string, string>();
                }
                rules.Add("id", filter_id_input.Text);
            }
            if (filter_class.Checked && filter_class_input.Text != "")
            {
                if (rules == null)
                {
                    rules = new Dictionary<string, string>();
                }
                rules.Add("class", filter_class_input.Text);
            }
            if (filter_name.Checked && filter_name_input.Text != "")
            {
                if (rules == null)
                {
                    rules = new Dictionary<string, string>();
                }
                rules.Add("name", filter_name_input.Text);
            }
            if(rules == null)
            {
                return;
            }
            String needInfo = null;
            if (result_text.Checked)
            {
                needInfo = "text";
            }
            else
            {
                needInfo = "href";
            }
            //开始配置爬虫
            SpiderNodeFilter spiderNodeFilter = new SpiderNodeFilter(rules);
            SpiderManager spiderManager = new SpiderManager(htmlContent, spiderNodeFilter);
            List<String> results = spiderManager.start(needInfo);
            foreach(String content in results)
            {
                Console.WriteLine(content);
            }
        }

       
    }
       
}
