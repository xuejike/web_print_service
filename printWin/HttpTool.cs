using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;


namespace printWin
{
    class HttpTool
    {
        public static HttpListener Listen(string hostPort, HttpCallBack iHttpCallback)
        {
            HttpListener httpListener = new HttpListener();

            httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            httpListener.Prefixes.Add(hostPort);
            httpListener.Start();
            
            new Thread(new ThreadStart(delegate
            {
                while (true)
                {
                    HttpListenerContext httpListenerContext = httpListener.GetContext();

                    Stream reqStream = httpListenerContext.Request.InputStream;
                    NameValueCollection queryString = httpListenerContext.Request.QueryString;
                    string body = new StreamReader(reqStream).ReadToEnd();

                    object result = iHttpCallback(queryString, body);
                    httpListenerContext.Response.StatusCode = 200;
                    httpListenerContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    httpListenerContext.Response.Headers.Add("Content-Type", "text/json; charset=utf-8");
                    using (StreamWriter writer = new StreamWriter(httpListenerContext.Response.OutputStream))
                    {
//                        string json = JsonConvert.SerializeObject(result);
                        writer.WriteLine(result);
                    }
                }
            })).Start();
            return httpListener;
        }
    }

    delegate Object HttpCallBack(NameValueCollection queryString, String body);
}
