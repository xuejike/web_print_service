using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace printWin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private int page = 0;
        private void button1_Click(object sender, EventArgs e)
        {
//            printDoc.PrinterSettings
            printDoc.Print();
//            printDoc.PrintPage += PrintDoc_PrintPage;

        }

        private void PrintDoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //            throw new NotImplementedException();
            Image file = Image.FromFile("C:\\Users\\xuejike\\Pictures\\工作证------薛纪克.jpg");
            e.Graphics.DrawImage(file, new Point(0, 0));
            Font f = new Font("宋体", 12);
            e.Graphics.DrawString("sss11", f, Brushes.Black, 0, 0);
        }

        private void printDoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //            printDoc.
            //            e.Graphics.DrawImage();

            Image file = Image.FromFile("C:\\Users\\xuejike\\Pictures\\工作证------薛纪克.jpg");
            e.Graphics.DrawImage(file,new Point(0,0));
            Font f = new Font("宋体", 12);
            e.Graphics.DrawString("sss"+page,f,Brushes.Black,0,0  );
            page++;
            if (page < 5)
            {
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
            }
            

        }
    }
}
