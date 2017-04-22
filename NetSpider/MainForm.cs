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
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
                NetDownLoaderUtils.downloaderHtml(htmlUrl, new System.Net.DownloadStringCompletedEventHandler(onHtmlDownLoaderComplete));
            }
            else
            {
                MessageBox.Show("路径不合法！");
                return;
            }
        }

        /// <summary>
        /// html页面下载完毕的回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onHtmlDownLoaderComplete(object sender, DownloadStringCompletedEventArgs e)
        {
            //开始TreeView的展示
            Lexer lexer = new Lexer(e.Result);
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

        private void NodeItemSelectListener(object sender, EventArgs e)
        {
            //情况之前选择的信息
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
    }
        #endregion
}
