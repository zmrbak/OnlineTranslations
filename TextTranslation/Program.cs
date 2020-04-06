using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TextTranslation
{
    class Program
    {
        public static void Main()
        {
            YouDaoTextTranslation youDaoTranslation = new YouDaoTextTranslation();
            youDaoTranslation.InputText = "我是赵庆明老师";
            youDaoTranslation.AppId = "-----------------";
            youDaoTranslation.SecretKey = "-----------------";
            string result = youDaoTranslation.Translation();

            JObject jObject = JObject.Parse(result);
            Console.WriteLine(jObject["query"].ToString());
            Console.WriteLine(jObject["translation"].ToString());

            Console.ReadLine();
        }
    }
}
