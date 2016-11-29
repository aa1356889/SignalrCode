using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Owin.Hosting;

namespace SignalrServices
{
    public partial class ServicesForm : Form
    {
        private ServicesManager _services;
        public ServicesForm()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            _services = new ServicesManager(ConfigHelper.GetServiceAddress);
            //开启服务
            _services.Star((isStar, message) =>
            {
                if (isStar)
                    MessageBox.Show("服务器启动成功");
                else
                    MessageBox.Show("服务器启动失败!失败信息：" + message);
            });
         
            InitializeComponent();
        }

    

        private void Form1_Load(object sender, EventArgs e)
        {
        
        }


        object lockobj = new object();
        public void AppedMessage(string text)
        {

            lock (lockobj)
            {
                if (this.txt_Message.Text.Length >= 1000)
                {

                    this.txt_Message.Text = string.Empty;
                }
                this.txt_Message.Text += text + "\r\n";
            }
        }

        void services_LoginCallBack(User obj)
        {
            AppedMessage(obj.UName + "已经上线," + DateTime.Now.ToString());
            AddItems(obj);
        }
        public void AddItems(User user)
        {

            ListViewItem item = new ListViewItem(new string[] { user.Uid, user.UName, DateTime.Now.ToString() });
            ; this.lv_Users.Items.Add(item);
        }

        public void RemoveItems(User user)
        {
            lock (lockobj)
            {
                for (int i = 0; i < this.lv_Users.Items.Count; i++)
                {
                    if (this.lv_Users.Items[i].Text == user.Uid)
                    {
                        this.lv_Users.Items.RemoveAt(i);
                    }
                }
            }
        }

        void services_CloseCallbCack(User obj)
        {
            AppedMessage(obj.UName + "已经离线，" + DateTime.Now.ToString());
            RemoveItems(obj);
        }

    }
}
