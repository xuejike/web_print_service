using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace printWin
{
    class NetService
    {
        public static bool startService()
        {
            try
            {
                HttpTool.Listen("http://127.0.0.1:9999/", (query, postBody) =>
                {
                    try
                    {
                        string paperWidth = query["paperWidth"];
                        string paperHeight = query["paperHeight"];

                        PageData pageData = new PageData();
                        pageData.PaperWidth = Convert.ToSingle(paperWidth);
                        pageData.PaperHeight = Convert.ToSingle(paperHeight);
                        string[] split = postBody.Split(new char[]{ '#' },StringSplitOptions.RemoveEmptyEntries);
//                        var deserializeObject = (List<String>)JsonConvert.DeserializeObject(postBody, typeof(List<String>));
                        pageData.Page = new List<string>(split);
                        PagePrintService printService = new PagePrintService(pageData);
                        printService.print();
                        return "success";
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
//                throw;
                return false;
            }
            
        }
    }
}
