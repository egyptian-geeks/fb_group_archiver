using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Facebook;
using System.Threading;
using System.IO;

namespace GroupArchiver
{
    public partial class Groups : Form
    {
        FacebookClient client;
        public Groups()
        {
            InitializeComponent();
        }
        void BindGroupList()
        {
            listBox1.DataSource = GetGroups();
            listBox1.DisplayMember = "name";
            listBox1.ValueMember = "id";
        }
        class group
        {
            public string name { get; set; }
            public string id { get; set; }
            public group(string name,string id)
            {
                this.name = name;
                this.id = id;
            }
        }
        List<group> GetGroups()
        {
            dynamic groups = client.Get("/me/groups");
            List<group> result = new List<group>();
            foreach (var item in groups.data)
            {
                result.Add(new group(item.name, item.id));
            }
            return result;
        }
       
        private void Groups_Load(object sender, EventArgs e)
        {
            client = FB.GetClient();
            lbl_user.Text = FB.me.name;
            lbl_id.Text = FB.me.id;
            BindGroupList();
        }

      
         public delegate void ArchivingOp(string id);
         List<string> currentlyProcessing = new List<string>();
        private void button1_Click(object sender, EventArgs e)
        {
            string location = Data.GetGroupLastLocation(listBox1.SelectedValue.ToString());
            if (location!=null&&Directory.Exists(location))
            {
                folderBrowserDialog1.SelectedPath = location;
            }
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                btn_export.Enabled = false;
                currentlyProcessing.Add(listBox1.SelectedValue.ToString());
                ArchivingOp archive = new ArchivingOp(c => { Archiver.GetAllPosts(c, folderBrowserDialog1.SelectedPath,cbx_newpostsonly.Checked?Data.GetGroupLastUpdate(c):null);  });
                archive.BeginInvoke(listBox1.SelectedValue.ToString(), new AsyncCallback(c =>
                {
                    MessageBox.Show("Done Archiving " + ((group)c.AsyncState).name);
                    currentlyProcessing.Remove(((group)c.AsyncState).id);
                    if ((string)this.Invoke(new Func<string>(() => { return listBox1.SelectedValue.ToString(); })) == ((group)c.AsyncState).id)
                    {
                        this.Invoke(new Action(() => { btn_export.Enabled = true; }));
                    }
                })
            , listBox1.SelectedItem);
            }
        }

        private void lnk_logout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Browser b = new Browser(UrlType.Logout, new Browser(UrlType.Login, new Groups()));
            b.Show();
            this.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string groupId = listBox1.SelectedValue.ToString();
            if (currentlyProcessing.Contains(groupId))
            {
                btn_export.Enabled = false;
                
            }
            else
            {
                btn_export.Enabled = true;
            }
            if (Data.GetGroupLastUpdate(groupId) == null)
            {
                cbx_newpostsonly.Enabled = false;
            }
            else
            {
                cbx_newpostsonly.Enabled = true;
            }
        }

        private void Groups_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }
        public const int SC_CLOSE = 61536;
        public const int WM_SYSCOMMAND = 274;
        
        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == WM_SYSCOMMAND && msg.WParam.ToInt32() == SC_CLOSE)
            {
                Environment.Exit(0);
            }

            base.WndProc(ref msg);
        }
 
    }
}
