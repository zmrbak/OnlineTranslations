using CsharpHttpHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TextTranslation
{
    public static class Utility
    {
        public static string ToParamString(this Dictionary<string, string> dictionary)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in dictionary.Keys)
            {
                stringBuilder.Append(string.Format("{0}={1}&", HttpUtility.UrlEncode(item.ToString()), HttpUtility.UrlEncode(dictionary[item].ToString())));
            }

            string result = stringBuilder.ToString();
            return result.Substring(0, result.Length - 1);
        }

        public static string ToUnixStyle(this DateTime dateTime)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (dateTime.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t.ToString();
        }

        public static string FileLength(this string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            return fileInfo.Length.ToString();
        }

        public static string ShortFileName(this string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            return fileInfo.Name;
        }

        public static string GetHttpUrl(string url)
        {
            HttpHelper httpHelper = new HttpHelper();
            HttpItem item = new HttpItem
            {
                URL = url,
                Method = "GET"
            };

            HttpResult result = httpHelper.GetHtml(item);

            //获取请请求的Html
            return result.Html;
        }

        public static string GetDateTime()
        {
            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            long millis = (long)ts.TotalMilliseconds;
            return Convert.ToString(millis / 1000);
        }

        public static string Truncate(string q)
        {
            if (q == null)
            {
                return null;
            }
            int len = q.Length;
            return len <= 20 ? q : (q.Substring(0, 10) + len + q.Substring(len - 10, 10));
        }

        public static string ComputeHash(string input, HashAlgorithm algorithm)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }


    }
}
