# FacebookAuth #

本範例為部落格文章「[ASP.Net MVC 實作使用 oAuth 2.0 連接 Google API](http://coding.anyun.tw/2012/03/14/asp-net-mvc-using-oauth-2-0-connect-google-api/)」的實作範例，更多詳細程式碼說明請參閱部落格文章。

在執行專案前請先開啟 HomeController.cs 檔案修改 API 相關參數：

```C#
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
```

開發環境：Visual Studio 2013

P.S. 因有開啟套件還原功能，請參閱 Demo 寫的「[NuGet套件還原步驟使用Visual Studio 2012 為例](http://demo.tc/Post/763)」說明來還原有用到的套件。

**by AnYun - [http://coding.anyun.idv.tw](http://coding.anyun.idv.tw)**