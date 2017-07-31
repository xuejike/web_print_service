using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;

namespace printWin
{
    class PagePrintService
    {
        private PrintDocument DocPrinter=new PrintDocument();

        private int PageNo = 0;

        private PageData PrintData;

        public PagePrintService(PageData printData)
        {
            PrintData = printData;

            //纸张大小 大于0
            if (printData.PaperHeight>0&&printData.PaperWidth>0)
            {
                DocPrinter.DefaultPageSettings.PaperSize=new PaperSize("自定义",get100InByMm(printData.PaperWidth),get100InByMm(printData.PaperHeight));
            }

           
            if (!string.IsNullOrEmpty(printData.PrintName))
            {
                DocPrinter.PrinterSettings.PrinterName = printData.PrintName;
            }

            DocPrinter.PrintPage += DocPrinter_PrintPage;


        }

        public void print()
        {
            DocPrinter.Print();
            
        }

        private int get100InByMm(float num)
        {
            return (int) Math.Ceiling(num / 25.4 * 100);
        }

        private void DocPrinter_PrintPage(object sender, PrintPageEventArgs e)
        {
            string pageImage = PrintData.Page[PageNo];
//            throw new NotImplementedException();
            e.Graphics.DrawImage(CommonTool.getImageByString(pageImage),new Point(0,0));
            PageNo++;
            if (PageNo < PrintData.Page.Count)
            {
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
            }

        }
    }

    class PageData
    {
        public List<String> Page { get; set; }
     

        public String PrintName { get; set; }

        public float PaperHeight { get; set; }
        public float PaperWidth { get; set; }
        

    }
}
