using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Text;
using System.IO;
using GoogleAuth.Models;
using Newtonsoft.Json;

namespace GoogleAuth.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 你申請的 client_id
        /// </summary>
        private const string client_id = "{client_id}";
        /// <summary>
        /// 你申請的 client_secret
        /// </summary>
        private const string client_secret = "{client_id}";
        /// <summary>
        /// 申請時候設定的回傳網址
        /// </summary>
        private const string redirect_uri = "{redirect_uri}";

        public ActionResult Index()
        {
            // 可在額外加入 approval_prompt=force 參數，
            // 就不會授權過還會在出現授權畫面
            string Url = "https://accounts.google.com/o/oauth2/auth?scope={0}&redirect_uri={1}&response_type={2}&client_id={3}&state={4}";
            // https://www.googleapis.com/auth/calendar 
            // https://www.googleapis.com/auth/calendar.readonly
            // 這兩個是存取 Google Calendar 的 scope 中間用空白做分隔
            // UrlEncode 之後再額外用 + 取代 %20
            string scope = Utitity.UrlEncode("https://www.googleapis.com/auth/calendar https://www.googleapis.com/auth/calendar.readonly").Replace("%20", "+");
            string redirect_uri_encode = Utitity.UrlEncode(redirect_uri);
            string response_type = "code";
            string state = "";

            Response.Redirect(string.Format(Url, scope, redirect_uri_encode, response_type, client_id, state));

            return null;
        }

        public ActionResult CallBack(string Code)
        {
            // 沒有接收到參數
            if (string.IsNullOrEmpty(Code))
                return Content("沒有收到 Code");

            string Url = "https://accounts.google.com/o/oauth2/token";
            string grant_type = "authorization_code";
            string redirect_uri_encode = Utitity.UrlEncode(redirect_uri);
            string data = "code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type={4}";

            HttpWebRequest request = HttpWebRequest.Create(Url) as HttpWebRequest;
            string result = null;
            request.Method = "POST";    // 方法
            request.KeepAlive = true; //是否保持連線
            request.ContentType = "application/x-www-form-urlencoded";

            string param = string.Format(data, Code, client_id, client_secret, redirect_uri_encode, grant_type);
            byte[] bs = Encoding.ASCII.GetBytes(param);

            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }

            using (WebResponse response = request.GetResponse())
            {
                StreamReader sr = new StreamReader(response.GetResponseStream());
                result = sr.ReadToEnd();
                sr.Close();
            }

            TokenData tokenData = JsonConvert.DeserializeObject<TokenData>(result);
            Session["token"] = tokenData.access_token;

            // 這邊不建議直接把 Token 當做參數傳給 CallAPI 可以避免 Token 洩漏
            return RedirectToAction("CallAPI");
        }

        public ActionResult CallAPI()
        {
            if (Session["token"] == null)
                return Content("請先取得授權！");

            string token = Session["token"] as string;
            // 取得日曆列表的 API 網址
            string Url = "https://www.googleapis.com/calendar/v3/users/me/calendarList?access_token=" + token;
            HttpWebRequest request = HttpWebRequest.Create(Url) as HttpWebRequest;
            string result = null;
            request.Method = "GET";    // 方法
            request.KeepAlive = true; //是否保持連線
            // 不使用接參數的方式可以在 Head 加上 Authorization，如下一行程式碼
            //request.Headers.Add("Authorization", "Bearer " + token);

            using (WebResponse response = request.GetResponse())
            {
                StreamReader sr = new StreamReader(response.GetResponseStream());
                result = sr.ReadToEnd();
                sr.Close();
            }

            Response.Write(result);

            return null;
        }

    }
}
