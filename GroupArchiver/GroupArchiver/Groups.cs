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
            BindGroupList();
        }

      
         public delegate void ArchivingOp(string id);
         List<int> currentlyProcessing = new List<int>();
        private void button1_Click(object sender, EventArgs e)
        {
            
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ArchivingOp archive = new ArchivingOp(c => Archiver.GetAllPosts(c,folderBrowserDialog1.SelectedPath));
                archive.BeginInvoke(listBox1.SelectedValue.ToString(), new AsyncCallback(c => MessageBox.Show("Done Archiving " + ((group)c.AsyncState).name)), listBox1.SelectedItem);
            }
        }
       
       
    }
}
