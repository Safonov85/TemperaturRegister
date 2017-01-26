using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.Net;
using System.Text.RegularExpressions;

namespace TemptraturRegister
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //using (TextWriter tw = new StreamWriter("TempInfo//SavedList.txt"))
            //{
            //    foreach (String s in IList.verbList)
            //    {
            //        tw.WriteLine(s);

            //    }
            //}
        }

        void GenerateNumbers()
        {
            int num = 0;
            List<int> list = new List<int>();

            while (num < 10)
            {
                list.Add(num);
                //listViewItem.add
                //this.Text = num.ToString();
                num++;
            }
            foreach (int number in list)
            {
                //listView.Items.Add(number.ToString());
                listBox.Items.Add(number.ToString());
            }

            //listBox.Show();
            //listView.Show();
        }

        public void GetTableInfo(string html)
        {
            //Console.WriteLine("Downloading and Loading data...");

            string htmlCode = "";
            using (WebClient client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.UserAgent, "AvoidError");
                htmlCode = client.DownloadString(html);
            }
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();

            document.LoadHtml(htmlCode);

            foreach (HtmlNode node in document.DocumentNode.SelectNodes("//td[@class='weather current']"))
            {
                string currentTempValue;
                //if (node.Id == "current-conditions-handle")
                //{
                //    this.Text = "found it";
                //}
                //listBox.Items.Add(textBox.Text);
                if (node.InnerText.Contains("º"))
                {
                    currentTempValue = node.InnerText;
                    string removeTabs = currentTempValue.Replace("\t", "");
                    string removeSpaces = removeTabs.Replace("\n", "");
                    string getTemp = removeSpaces.Substring(0, 2);
                    //currentTempValue.Replace(" ", string.Empty);
                    //string getTempNumber = new String(currentTempValue.TakeWhile(Char.IsDigit).ToArray());
                    textBox.Text = getTemp;
                    this.Text = getTemp;
                    break;
                }

                //foreach (HtmlNode node2 in node.SelectNodes("//div"))
                //{
                //    if (node.InnerText.Contains("º"))
                //    {
                //        textBox.Text = node2.InnerText;
                //    }
                //}

                //foreach (HtmlNode div in document.DocumentNode.SelectNodes("//div"))
                //{
                //    textBox.Text = div.InnerText;
                //}

                //if (table.Id == "sendungsHistorieOne")
                //{
                //    string innerText = table.InnerText.Replace("&nbsp;", "");

                //    Console.WriteLine(innerText);
                //}
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            GenerateNumbers();
            GetTableInfo("http://www.klart.se/väder-björsäter.html");
        }
    }
}
