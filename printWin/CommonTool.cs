using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace printWin
{
    class CommonTool
    {
        public static void Log(String msg)
        {
            string currentDirectory = Environment.CurrentDirectory;
            string path = currentDirectory + "/log.txt";
            if (System.IO.File.Exists(path))
            {
                using (StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8))
                {
                    sw.WriteLine(new DateTime().ToString("yyyy-MM-dd HH:mm:ss -")+msg);
                    sw.Flush();
                }
            }
        }

        /// <summary>
        /// base64字符串转图片
        /// </summary>
        /// <param name="base64Str"></param>
        /// <returns></returns>
        public static Image getImageByString(String base64Str)

        {
            if (base64Str!=null)
            {
                int headIndex = base64Str.IndexOf("base64", StringComparison.Ordinal);
                if (headIndex >= 0)
                {
                    base64Str = base64Str.Substring(headIndex + 7);
                }

                MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(base64Str));
                Image fromStream = Image.FromStream(memoryStream);
                return fromStream;
            }
            return null;
        }
    }
}
