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
        public string currentFileName;
        public string city = "http://www.klart.se/väder-stockholm.html";


        public Main()
        {
            InitializeComponent();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            GetTempInfo(city);
        }

        public void GetTempInfo(string html)
        {
            // using try - catch because internet may be down sometimes and throw the list an exception
            try
            {
                string htmlCode = "";
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add(HttpRequestHeader.UserAgent, "AvoidError");
                    
                    htmlCode = client.DownloadString(html);
                }
                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();

                document.LoadHtml(htmlCode);

                // gets the degrees of the weather (in celcius)
                foreach (HtmlNode node in document.DocumentNode.SelectNodes("//td[@class='weather current']"))
                {
                    string currentTempValue;
                    if (node.InnerText.Contains("º"))
                    {
                        currentTempValue = node.InnerText;
                        string removeTabs = currentTempValue.Replace("\t", ""); // removes tabs from text
                        string removeSpaces = removeTabs.Replace("\n", ""); // removes spaces from text
                        
                        // only saves degrees(celcius) to listBox
                        int indexCharacter = removeSpaces.IndexOf("Â");
                        string getTemp = removeSpaces.Substring(0, indexCharacter);
                        listBox.Items.Add(DateTime.Now.ToString() + ": " + getTemp);
                        break;
                    }
                }
                SaveListBox();
            }
            catch (Exception e)
            {
                listBox.Items.Add(DateTime.Now.ToString() + ": " + e.ToString());
                SaveListBox();
            }
            
        }

        // Saving all the items in the listbox to a .txt file
        public void SaveListBox()
        {
            using (TextWriter textWriter = new StreamWriter(currentFileName))
            {
                foreach (var item in listBox.Items)
                {
                    textWriter.WriteLine(item.ToString());
                }
                textWriter.Close();
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            notifyIcon.Visible = true;
            currentFileName = "SavedList_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            listBox.Items.Add("Vädret i Stockholm");
            GetTempInfo(city);
            timer.Start();
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon.Visible = true;
                this.Hide();
            }

            //else if (FormWindowState.Normal == this.WindowState)
            //{
            //    notifyIcon.Visible = false;
            //}
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
    }
}
