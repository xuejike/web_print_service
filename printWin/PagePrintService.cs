using System;
using System.Collections.Generic;
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
            DocPrinter.PrintPage += DocPrinter_PrintPage;
        }

        private void DocPrinter_PrintPage(object sender, PrintPageEventArgs e)
        {
//            throw new NotImplementedException();

        }
    }

    class PageData
    {
        public List<String> Page { get; set; }
        public float MarginTop { get; set; }
        public float MarginLeft { get; set; }
        public float MarginRight { get; set; }
        public float MarginBottom { get; set; }
        public String PrintName { get; set; }


    }
}
