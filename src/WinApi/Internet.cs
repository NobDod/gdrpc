using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GDRPC.WinApi
{
    class HttpResponse
    {
        public static string SendPost(string url, string data)
        {
            string obj = null;
            SendPostPrivate(url, data, out obj);
            return obj;
        }
        private static void SendPostPrivate(string url, string data, out string returnData)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.Timeout = 15000;
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(data);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            using (Stream dataStream = request.GetRequestStream())
                dataStream.Write(byteArray, 0, byteArray.Length);
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
                returnData = reader.ReadToEnd();
        }
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        public static bool IsConnectedToInternet() => InternetGetConnectedState(out int desc, 0);
    }
}
