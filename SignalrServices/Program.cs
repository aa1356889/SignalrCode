using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNet.SignalR;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]//启动log4.net
namespace SignalrServices
{
    static class Program
    {
        public static ServicesForm from;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            from = new ServicesForm();
            Application.Run(from);
        }
    }
}
