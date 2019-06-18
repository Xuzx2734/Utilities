using Newtonsoft.Json;
using Pom.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
//using System.Web.Caching;
using System.Web;

namespace Utilities
{
    //企业微信接口帮助类 xzx add  201808
    //public static class WeChatWorkHelper {

    //    private const string sendmsgUrl = "https://qyapi.weixin.qq.com/cgi-bin/message/send";
    //    private const string method = "POST";        
    //    //private static string corpid = Const.CORPID;
    //    //private static string corpsecret = Const.CORPSECRET;
    //    private static string corpid = string.Empty;
    //    private static string corpsecret = string.Empty;
    //    private const string tokenUrl = "https://qyapi.weixin.qq.com/cgi-bin/gettoken";
    //    private const string getuserinfoUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo";
    //    private const string getuserdetailUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/getuserdetail";
    //    private const string long2shortUrl = "https://api.weixin.qq.com/cgi-bin/shorturl";
    //    private const string wechattokenUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential";
    //    //private static string appid = Const.APPID;
    //    //private static string secret = Const.APPSECRET;

    //    /// <summary>
    //    /// 发送企业微信消息
    //    /// </summary>
    //    /// <param name="postmsg"></param>
    //    public static Tuple<int, string> SendMessage(string postmsg) {
    //        var token = GetVaildToken();
    //        string query = $"?access_token={token}";
    //        string url = sendmsgUrl + query;
    //        HttpWebRequest httpRequest = null;
    //        HttpWebResponse httpResponse = null;

    //        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
    //        httpRequest = (HttpWebRequest) WebRequest.CreateDefault(new Uri(url));
    //        httpRequest.Method = method;
    //        httpRequest.ContentType = "application/json; charset=UTF-8";
    //        if (0 < postmsg.Length) {
    //            byte[] data = Encoding.UTF8.GetBytes(postmsg);
    //            using (Stream stream = httpRequest.GetRequestStream()) {
    //                stream.Write(data, 0, data.Length);
    //            }
    //        }

    //        try {
    //            httpResponse = (HttpWebResponse) httpRequest.GetResponse();
    //        } catch (WebException ex) {
    //            httpResponse = (HttpWebResponse) ex.Response;
    //        }

    //        using (Stream st = httpResponse.GetResponseStream()) {
    //            using (StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"))) {
    //                var res = reader.ReadToEnd();
    //                var result = JsonConvert.DeserializeObject<sendResult>(res);

    //                return Tuple.Create<int, string>(result.errcode, result.errmsg + string.Empty + result.invaliduser + string.Empty + result.invalidparty + string.Empty + result.invalidtag);
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// 根据code获取成员信息
    //    /// </summary>
    //    /// <param name="code"></param>
    //    /// <returns>errcode ,UserId, DeviceId, user_ticket</returns>
    //    public static userInfoResult Getuserinfo(string code) {
    //        var token = GetVaildToken();
    //        string query = $"?access_token={token}&code={code}";
    //        string url = getuserinfoUrl + query;
    //        HttpWebRequest httpRequest = null;
    //        HttpWebResponse httpResponse = null;
    //        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
    //        httpRequest = (HttpWebRequest) WebRequest.CreateDefault(new Uri(url));
    //        httpRequest.Method = "GET";
    //        httpRequest.ContentType = "application/json; charset=UTF-8";

    //        try {
    //            httpResponse = (HttpWebResponse) httpRequest.GetResponse();
    //        } catch (WebException ex) {
    //            httpResponse = (HttpWebResponse) ex.Response;
    //        }

    //        using (Stream st = httpResponse.GetResponseStream()) {
    //            using (StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"))) {
    //                var res = reader.ReadToEnd();
    //                var result = JsonConvert.DeserializeObject<userInfoResult>(res);
    //                return result;
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// 使用user_ticket获取成员详情
    //    /// </summary>
    //    /// <param name="user_ticket">"user_ticket": "USER_TICKET"</param>
    //    /// <returns></returns>
    //    public static userdetailResult getuserdetail(string user_ticket) {
    //        var token = GetVaildToken();
    //        string query = $"?access_token={token}";
    //        string url = getuserdetailUrl + query;
    //        HttpWebRequest httpRequest = null;
    //        HttpWebResponse httpResponse = null;
    //        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
    //        httpRequest = (HttpWebRequest) WebRequest.CreateDefault(new Uri(url));
    //        httpRequest.Method = method;
    //        httpRequest.ContentType = "application/json; charset=UTF-8";
    //        if (0 < user_ticket.Length) {
    //            byte[] data = Encoding.UTF8.GetBytes(user_ticket);
    //            using (Stream stream = httpRequest.GetRequestStream()) {
    //                stream.Write(data, 0, data.Length);
    //            }
    //        }

    //        try {
    //            httpResponse = (HttpWebResponse) httpRequest.GetResponse();
    //        } catch (WebException ex) {
    //            httpResponse = (HttpWebResponse) ex.Response;
    //        }

    //        using (Stream st = httpResponse.GetResponseStream()) {
    //            using (StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"))) {
    //                var res = reader.ReadToEnd();
    //                var result = JsonConvert.DeserializeObject<userdetailResult>(res);
    //                return result;
    //            }
    //        }
    //    }

    //    public static object SendMessage(object p)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) {
    //        return true;
    //    }

    //    /// <summary>
    //    /// 获取企业微信token
    //    /// </summary>
    //    public static Access_Token GetAccesssToken() {
    //        //var currentPath = AppDomain.CurrentDomain.BaseDirectory;
    //        //var logPath = Path.Combine(currentPath, "log/wxtoken/");
    //        //if (!Directory.Exists(logPath)) {
    //        //    Directory.CreateDirectory(logPath);
    //        //}
    //        string query = $"?corpid={corpid}&corpsecret={corpsecret}";
    //        string url = tokenUrl + query;
    //        HttpWebRequest httpRequest = null;
    //        HttpWebResponse httpResponse = null;

    //        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
    //        httpRequest = (HttpWebRequest) WebRequest.CreateDefault(new Uri(url));
    //        httpRequest.Method = "GET";
    //        try {
    //            httpResponse = (HttpWebResponse) httpRequest.GetResponse();
    //        } catch (WebException ex) {
    //            httpResponse = (HttpWebResponse) ex.Response;
    //        }
    //        using (Stream st = httpResponse.GetResponseStream()) {
    //            using (StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"))) {
    //                var res = reader.ReadToEnd();
    //                //LogHelper.Instance.AddQueue($"{DateTime.Now}: 企业微信token更新： {Environment.NewLine}{res}");
    //                var model = JsonConvert.DeserializeObject<Access_Token>(res);
    //                if(model.errcode != 0) {
    //                    throw new Exception($"企业微信接口请求出错,错误码{model.errcode},详情:{model.errmsg},错误码对应信息:https://work.weixin.qq.com/api/doc#10649");
    //                }
    //                model.expires_time = DateTime.Now.AddSeconds(model.expires_in);

    //                return model;
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// 获取有效token 并缓存
    //    /// </summary>
    //    /// <returns></returns>
    //    public static string GetVaildToken() {
    //        string access_token = string.Empty;
    //        var cache = HttpRuntime.Cache;
    //        if(cache == null) throw new Exception($"{DateTime.Now}: cache为空");
    //        else {
    //            var cachetoken = cache.Get("access_token");
    //            if(cachetoken == null) {
    //                var token = GetAccesssToken();
    //                cache.Insert("access_token", token, null, DateTime.Now.AddSeconds(token.expires_in), TimeSpan.Zero, CacheItemPriority.High, null);
    //                access_token = token.access_token;
    //            }else {
    //                var at = cachetoken as Access_Token;
    //                if(at != null && at.expires_time < DateTime.Now) {
    //                    var a = GetAccesssToken();
    //                    cache.Insert("access_token", a, null, DateTime.Now.AddSeconds(a.expires_in), TimeSpan.Zero, CacheItemPriority.High, null);
    //                    access_token = a.access_token;
    //                } else {
    //                    access_token = at.access_token;
    //                }
    //            }
    //        }
    //        return access_token;
    //    }

    //    ///// <summary>
    //    ///// 微信公众号token 方圆会
    //    ///// </summary>
    //    ///// <returns></returns>
    //    //private static Access_Token GetWechatToken()
    //    //{
    //    //    string query = $"&appid={appid}&secret={secret}";
    //    //    string url = wechattokenUrl + query;
    //    //    HttpWebRequest httpRequest = null;
    //    //    HttpWebResponse httpResponse = null;

    //    //    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
    //    //    httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
    //    //    httpRequest.Method = "GET";
    //    //    try
    //    //    {
    //    //        httpResponse = (HttpWebResponse)httpRequest.GetResponse();
    //    //    }
    //    //    catch (WebException ex)
    //    //    {
    //    //        httpResponse = (HttpWebResponse)ex.Response;
    //    //    }
    //    //    using (Stream st = httpResponse.GetResponseStream())
    //    //    {
    //    //        using (StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8")))
    //    //        {
    //    //            var res = reader.ReadToEnd();
    //    //            LogHelper.Instance.AddQueue($"{DateTime.Now}: 微信token更新： {Environment.NewLine}{res}");
    //    //            var model = JsonConvert.DeserializeObject<Access_Token>(res);
    //    //            if (model.errcode != 0)
    //    //            {
    //    //                throw new Exception($"微信公众号接口请求出错,错误码{model.errcode},详情:{model.errmsg},错误码对应信息:https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421140183");
    //    //            }
    //    //            model.expires_time = DateTime.Now.AddSeconds(model.expires_in);

    //    //            return model;
    //    //        }
    //    //    }
    //    //}

    //    ///// <summary>
    //    ///// 获取微信公众号token   方圆会appid
    //    ///// </summary>
    //    ///// <returns></returns>
    //    //public static string GetVaild_WechatToken(bool isNeedFresh = false)
    //    //{
    //    //    string access_token = string.Empty;
    //    //    var cache = HttpRuntime.Cache;
    //    //    if (cache == null) LogHelper.Instance.AddQueue($"{DateTime.Now}: cache为空");
    //    //    else
    //    //    {
    //    //        var cachetoken = cache.Get("wechat_token");
    //    //        if (cachetoken == null || isNeedFresh)
    //    //        {
    //    //            var token = GetWechatToken();
    //    //            cache.Insert("wechat_token", token, null, DateTime.Now.AddSeconds(token.expires_in), TimeSpan.Zero, CacheItemPriority.High, null);
    //    //            access_token = token.access_token;
    //    //        }
    //    //        else
    //    //        {
    //    //            var at = cachetoken as Access_Token;
    //    //            if (at != null && at.expires_time < DateTime.Now)
    //    //            {
    //    //                var a = GetWechatToken();
    //    //                cache.Insert("wechat_token", a, null, DateTime.Now.AddSeconds(a.expires_in), TimeSpan.Zero, CacheItemPriority.High, null);
    //    //                access_token = a.access_token;
    //    //            }
    //    //            else
    //    //            {
    //    //                access_token = at.access_token;
    //    //            }
    //    //        }
    //    //    }
    //    //    return access_token;
    //    //}

    //    ///// <summary>
    //    ///// 长链接转短链接
    //    ///// </summary>
    //    ///// <param name="longUrl"></param>
    //    ///// <returns></returns>
    //    //public static shortUrl_Result wechat_longUrl2short(string longUrl)
    //    //{
    //    //    var token = GetVaild_WechatToken();
    //    //    string query = $"?access_token={token}";
    //    //    string url = long2shortUrl + query;
    //    //    HttpWebRequest httpRequest = null;
    //    //    HttpWebResponse httpResponse = null;
    //    //    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
    //    //    httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
    //    //    httpRequest.Method = method;
    //    //    httpRequest.ContentType = "application/json; charset=UTF-8";
    //    //    if (!string.IsNullOrEmpty(longUrl))
    //    //    {

    //    //        string postParams = "{\"action\":\"long2short\",\"long_url\":\""+ longUrl + "\"}";
    //    //        byte[] data = Encoding.UTF8.GetBytes(postParams);
    //    //        using (Stream stream = httpRequest.GetRequestStream())
    //    //        {
    //    //            stream.Write(data, 0, data.Length);
    //    //        }
    //    //    }

    //    //    try
    //    //    {
    //    //        httpResponse = (HttpWebResponse)httpRequest.GetResponse();
    //    //    }
    //    //    catch (WebException ex)
    //    //    {
    //    //        httpResponse = (HttpWebResponse)ex.Response;
    //    //    }

    //    //    using (Stream st = httpResponse.GetResponseStream())
    //    //    {
    //    //        using (StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8")))
    //    //        {
    //    //            var res = reader.ReadToEnd();
    //    //            var result = JsonConvert.DeserializeObject<shortUrl_Result>(res);
    //    //            //多个地方使用方圆会token，导致token失效，强制刷新
    //    //            if(result != null && result.errcode == 40001)
    //    //            {
    //    //                GetVaild_WechatToken(true);
    //    //            }
    //    //            return result;
    //    //        }
    //    //    }
    //    //}

    //}

    /// <summary>
    /// 微信返回格式
    /// </summary>
    public class WeChatBaseResult {
        public string errmsg { get; set; }
        public int errcode { get; set; }
    }

    /// <summary>
    /// token返回格式
    /// </summary>
    public class Access_Token : WeChatBaseResult {

        public string access_token { get; set; }
        public int expires_in { get; set; }
        public DateTime expires_time { get; set; }
    }    

    /// <summary>
    /// 微信发送消息返回格式
    /// </summary>
    public class sendResult : WeChatBaseResult {
        
        public string invaliduser { get; set; }
        public string invalidparty { get; set; }
        public string invalidtag { get; set; }

    }

    /// <summary>
    /// 获取用户信息返回格式
    /// </summary>
    public class userInfoResult : WeChatBaseResult {

        /// <summary>
        /// 成员UserID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 手机设备号(由企业微信在安装时随机生成，删除重装会改变，升级不受影响)
        /// </summary>
        public string DeviceId { get; set; }
        /// <summary>
        /// 成员票据，最大为512字节。
        ///scope为snsapi_userinfo或snsapi_privateinfo，且用户在应用可见范围之内时返回此参数。
        ///后续利用该参数可以获取用户信息或敏感信息。
        /// </summary>
        public string user_ticket { get; set; }
        /// <summary>
        /// 	user_ticket的有效时间（秒），随user_ticket一起返回
        /// </summary>
        public int expires_in { get; set; }

    }

    /// <summary>
    /// 获取用户明细返回格式
    /// </summary>
    public class userdetailResult : WeChatBaseResult {

        /// <summary>
        /// 成员UserID
        /// </summary>
        public string userid { get; set; }
        /// <summary>
        /// 成员姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 成员手机号，仅在用户同意snsapi_privateinfo授权时返回
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 性别。0表示未定义，1表示男性，2表示女性
        /// </summary>
        public string gender { get; set; }
        /// <summary>
        /// 成员邮箱，仅在用户同意snsapi_privateinfo授权时返回
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 头像url。注：如果要获取小图将url最后的”/0”改成”/100”即可。仅在用户同意snsapi_privateinfo授权时返回
        /// </summary>
        public string avatar { get; set; }
        /// <summary>
        /// 员工个人二维码（扫描可添加为外部联系人），仅在用户同意snsapi_privateinfo授权时返回
        /// </summary>
        public string qr_code { get; set; }

    }

    /// <summary>
    /// 长链接转短链接返回实体
    /// </summary>
    public class shortUrl_Result : WeChatBaseResult
    {
        public string short_url { get; set; }
    }

}