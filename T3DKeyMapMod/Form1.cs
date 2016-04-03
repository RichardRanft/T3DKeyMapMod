using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using BasicLogging;

namespace T3DKeyMapMod
{
    public partial class Form1 : Form
    {
        private String m_keyMapPath;
        private Dictionary<String, String> m_keyMap;
        private AboutBox1 m_about;
        private DocForm m_doc;
        private CLog m_log;

        public Form1()
        {
            InitializeComponent();
            m_log = new CLog();
            String logfile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Roostertail Games";
            if(!Directory.Exists(logfile))
            {
                try
                {
                    Directory.CreateDirectory(logfile);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Unable to create log folder at " + logfile + " : " + ex.Message + "\n" + ex.StackTrace, "Error - can't create log folder.");
                    Application.Exit();
                }
            }
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            if (attributes.Length == 0)
            {
                MessageBox.Show("Unable to access assembly information.  Can't create application log folder.", "Error - can't create log folder.");
                Application.Exit();
            }
            logfile += "\\" + ((AssemblyProductAttribute)attributes[0]).Product;
            if (!Directory.Exists(logfile))
            {
                try
                {
                    Directory.CreateDirectory(logfile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to create log folder at " + logfile + " : " + ex.Message + "\n" + ex.StackTrace, "Error - can't create log folder.");
                    Application.Exit();
                }
            }
            m_log.Filename = logfile + "\\T3DKeyMapMod.log";
            m_log.WriteLine("Application started successfully.");
            m_about = new AboutBox1();
            m_doc = new DocForm(m_log);
            m_keyMapPath = "";
            m_keyMap = new Dictionary<String, String>();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(fbdOpenProject.ShowDialog() == System.Windows.Forms.DialogResult.OK) // fbd is a FolderBrowserDialog
            {
                // use path to extrapolate the location of the keymap.cs file
                m_keyMapPath = fbdOpenProject.SelectedPath + @"\game\scripts\gui\keymap.cs";
                if(!File.Exists(m_keyMapPath))
                {
                    createKeymapScript(fbdOpenProject.SelectedPath);
                }
                loadKeyMap();
            }
        }

        private void createKeymapScript(String basepath)
        {
            String optDlgPath = basepath + @"\game\scripts\gui\optionsDlg.cs";
            String keyMapPath = basepath + @"\game\scripts\gui\keyMap.cs";
            List<String> optDlgFile = new List<string>();
            List<String> keyMapFile = new List<string>();
            using (StreamReader sr = new StreamReader(optDlgPath))
            {
                String line = "";
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    if (line.StartsWith("$RemapCount") || line.StartsWith("$RemapName") || line.StartsWith("$RemapCmd"))
                        keyMapFile.Add(line);
                    else
                        optDlgFile.Add(line);
                }
            }

            using (StreamWriter sw = new StreamWriter(optDlgPath))
            {
                bool execAdded = false;
                foreach (String line in optDlgFile)
                {
                    if (line.Trim().Length < 1 && !execAdded)
                    {
                        sw.WriteLine("exec(\"./keyMap.cs\");");
                        execAdded = true;
                    }
                    else
                        sw.WriteLine(line);
                }
            }

            using(StreamWriter sw = new StreamWriter(keyMapPath))
            {
                foreach (String line in keyMapFile)
                    sw.WriteLine(line);
            }
        }

        private void loadKeyMap()
        {
            using (StreamReader sr = new StreamReader(m_keyMapPath))
            {
                String line = "";
                String key = "";
                String method = "";
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    if (line.Equals("$RemapCount = 0;") || line.Equals("$RemapCount++;"))
                        continue;
                    if (line.Contains("$RemapName"))
                    {
                        key = line.Replace("$RemapName[$RemapCount] = \"", "");
                        key = key.Replace("\";", "");
                    }
                    if(line.Contains("$RemapCmd"))
                    {
                        method = line.Replace("$RemapCmd[$RemapCount] = \"", "");
                        method = method.Replace("\";", "");

                        m_keyMap.Add(key, method);
                        key = "";
                        method = "";
                    }
                }
            }
            dgvKeyMap.Rows.Clear(); // dgv is a DataGridView
            foreach(String key in m_keyMap.Keys)
            {
                dgvKeyMap.Rows.Add(new object[]{key, m_keyMap[key]});
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(m_keyMapPath != "")
            {
                label1.Focus();
                m_keyMap.Clear();
                foreach (DataGridViewRow row in dgvKeyMap.Rows)
                {
                    if (row.Cells[0].Value == null)
                        continue;
                    m_keyMap.Add(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString());
                }
                using (StreamWriter sr = new StreamWriter(m_keyMapPath))
                {
                    sr.WriteLine("$RemapCount = 0;");
                    foreach(KeyValuePair<String, String> entry in m_keyMap)
                    {
                        sr.WriteLine("$RemapName[$RemapCount] = \"" + entry.Key + "\";");
                        sr.WriteLine("$RemapCmd[$RemapCount] = \"" + entry.Value + "\";");
                        sr.WriteLine("$RemapCount++;");
                    }
                }
            }
        }

        private void dgvKeyMap_Leave(object sender, EventArgs e)
        {
            dgvKeyMap.EndEdit();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dgvKeyMap_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewRow row = new DataGridViewRow();
            DataGridView obj = (DataGridView)sender;
            int index = obj.SelectedRows[0].Index + 1;
            if (index == dgvKeyMap.Rows.Count)
                dgvKeyMap.Rows.Add(row);
            else
                dgvKeyMap.Rows.Insert(index, row);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_about.ShowDialog();
        }

        private void documentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_doc.ShowDialog();
        }
    }
}
