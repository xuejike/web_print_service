using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;


namespace printWin
{
    class NetService
    {
        private static HttpListener httpListener;
        public static bool startService()
        {
            try
            {
                httpListener = HttpTool.Listen("http://127.0.0.1:19999/", (query, postBody) =>
                {
                    try
                    {
                        string paperWidth = query["paperWidth"];
                        string paperHeight = query["paperHeight"];
                        string printerName = query["printerName"];

                        PageData pageData = new PageData();
                        pageData.PaperWidth = Convert.ToSingle(paperWidth);
                        pageData.PaperHeight = Convert.ToSingle(paperHeight);
                        pageData.PrintName = printerName;
                        string[] split = postBody.Split(new char[]{ '#' },StringSplitOptions.RemoveEmptyEntries);
//                        var deserializeObject = (List<String>)JsonConvert.DeserializeObject(postBody, typeof(List<String>));
                        pageData.Page = new List<string>(split);
                        PagePrintService printService = new PagePrintService(pageData);
                        printService.print();
                        return "success:打印发送成功";
                    }
                    catch (Exception e)
                    {
                        CommonTool.Log(e.StackTrace);
                        return "error:"+e.Message;
                    }
                    
                    
                });
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                CommonTool.Log(e.StackTrace);
                httpListener = null;
//                throw;
                return false;
            }
            
        }

        public static void stopService()
        {
            if (httpListener != null)
            {
                httpListener.Stop();
            }
        }
    }
}
