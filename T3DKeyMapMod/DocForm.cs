using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using BasicLogging;

namespace T3DKeyMapMod
{
    public partial class DocForm : Form
    {
        private Dictionary<String, String> m_docList;
        private CLog m_log;

        public CLog Log
        {
            set { m_log = value; }
        }

        public DocForm(CLog log)
        {
            m_log = log;
            InitializeComponent();
            InitDocList();
        }

        private void InitDocList()
        {
            m_log.WriteLine("Initializing document list...");
            m_docList = new Dictionary<string, string>();
            String exePath = Application.ExecutablePath;
            exePath = exePath.Remove(exePath.LastIndexOf('\\'));
            String path = Path.GetFullPath(exePath + "\\Help\\");
            m_log.WriteLine("Listing documents in " + path);
            try
            {
                var fileList = Directory.EnumerateFiles(path, "*.html");
                foreach (String file in fileList)
                {
                    m_log.WriteLine("Found " + file);
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    try
                    {
                        doc.Load(file);
                    }
                    catch (Exception ex)
                    {
                        m_log.WriteLine("Failed to open " + file + " : " + ex.Message + "\n" + ex.StackTrace);
                    }
                    HtmlAgilityPack.HtmlNode titleNode = doc.DocumentNode.ChildNodes.FindFirst("title");
                    m_log.WriteLine(" Loaded " + file);
                    m_docList.Add(titleNode.InnerText, file);
                }
                foreach (String key in m_docList.Keys)
                {
                    lbxDocPages.Items.Add(key);
                    m_log.WriteLine(" Added page " + key + " @ " + m_docList[key]);
                }
            }
            catch (Exception ex)
            {
                m_log.WriteLine("Error getting documentation files : " + ex.Message + "\n" + ex.StackTrace);
                Application.Exit();
            }
        }

        private void lbxDocPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            String pathString = Path.GetFullPath(m_docList[lbxDocPages.SelectedItem.ToString()]);
            wbzDocViewer.Navigate(pathString);
        }
    }
}
