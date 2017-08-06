using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace printWin
{
    partial class jkPrintService : ServiceBase
    {
        public jkPrintService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            NetService.startService();
        }

        protected override void OnStop()
        {
            NetService.stopService();
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
        }
    }
}
