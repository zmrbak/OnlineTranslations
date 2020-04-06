using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TextTranslation
{
    public class YouDaoTextTranslation
    {
        private string apiUrl = "https://openapi.youdao.com/api";
        private HttpHelper httpHelper = null;
        private string appId = "";
        private string secretKey = "";
        private string inputText = "";
        private string salt = "";
        private string curTime = "";

        public string AppId { get => appId; set => appId = value; }
        public string SecretKey { get => secretKey; set => secretKey = value; }
        public string InputText { get => inputText; set => inputText = value; }

        public string Translation()
        {
            if (this.httpHelper == null)
            {
                this.httpHelper = new HttpHelper();
            }

            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = apiUrl,  
                Method = "post",
                ContentType = "application/x-www-form-urlencoded",
                PostDataType = PostDataType.String,
                PostEncoding = System.Text.Encoding.UTF8,
            };

            curTime = Utility.GetDateTime();
            salt = DateTime.Now.Millisecond.ToString();

            Dictionary<string, string> param = new Dictionary<string, string>();
            //待翻译文本
            //必须是UTF-8编码
            param["q"] = inputText;
            //源语言
            //参考下方 支持语言 (可设置为auto)
            param["from"] = "zh-CHS";
            //目标语言
            //参考下方 支持语言(可设置为auto)
            param["to"] = "en";
            //应用ID
            //可在 应用管理 查看
            param["appKey"] = appId;
            //UUID
            param["salt"] = salt;
            //签名
            //sha256(应用ID+input+salt+curtime+应用密钥)
            //param["sign"] = "";
            //签名类型
            param["signType"] = "v3";
            //当前UTC时间戳(秒)
            //TimeStamp
            param["curtime"] = curTime;
            //翻译结果音频格式，支持mp3
            param["ext"] = "mp3";
            //翻译结果发音选择
            //0为女声，1为男声。默认为女声
            param["voice"] = "0";
            //是否严格按照指定from和to进行翻译：true/false
            //如果为false，则会自动中译英，英译中。默认为false
            param["strict"] = "false";

            //签名
            //sha256(应用ID+input+salt+curtime+应用密钥)
            string signStr = appId + Utility.Truncate(inputText) + salt + curTime + secretKey; ;
            string sign = Utility.ComputeHash(signStr, new SHA256CryptoServiceProvider());
            param["sign"] = sign;

            //发送请求
            item.Postdata = param.ToParamString();
            HttpResult result = httpHelper.GetHtml(item);

            //处理结果
            JObject jObject = JObject.Parse(result.Html);
            JObject obj = new JObject();
            obj["query"] = jObject["query"].ToString();
            obj["translation"] = jObject["translation"][0].ToString();
            return obj.ToString(Newtonsoft.Json.Formatting.None);
        }
    }
}
