using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using log4net;
using Newtonsoft.Json;

/// <summary>
/// NotificationService 的摘要说明
/// </summary>
public class NotificationService
{
    private static String Dev_Env = "2";
    private static String Prod_Env = "1";
    private ILog Logger = LogManager.GetLogger(typeof(NotificationService));
    private String serviceUrl = "openapi.xg.qq.com/v2/push/single_device";
    private String AccessId = "2200199196";
    private String SecretKey = "3ba8903b6c3ae3b74cf2eebedfcc6188";
    private static String Env = Dev_Env;


    public String Send(string deviceToken, int badge, String message, Approval approval, String platform)
    {
        var url = "http://" + serviceUrl;

        Hashtable parameters = new Hashtable();
        parameters["access_id"] = AccessId;
        parameters["timestamp"] = ConvertDateTimeInt(DateTime.Now).ToString();

        parameters["environment"] = Env;
        parameters["device_token"] = deviceToken;

        if (platform == "ios")
        {
            setParameter4iOS(deviceToken, badge, message, approval, parameters);
        }
        else
        {
            setParameter4Android(deviceToken, badge, message, approval, parameters);
        }
        parameters["sign"] = createSign(serviceUrl, parameters);

        var postData = GetPostData(parameters);

        var responseString = OpenReadWithHttps(url, postData);

        Logger.Debug("response = " + responseString);

        return responseString;

    }

    private Hashtable setParameter4iOS(string deviceToken, int badge, String message, Approval approval, Hashtable parameters)
    {
        var messageJSON = "{\"aps\":{\"alert\":\"您有一条来自于金军航的审批\", \"sound\":\"default\", \"badge\": 3, \"approval\": " + JsonConvert.SerializeObject(approval)  + "  }}";
        //var message = "{}";
        parameters["message"] = messageJSON;
        parameters["message_type"] = "0";

        return parameters;
    }

    public Hashtable setParameter4Android(string deviceToken, int badge, String message, Approval approval, Hashtable parameters)
    {

       // var messageJSON = "{\"aps\":{\"alert\":\"" + message + "\", \"sound\":\"default\", \"badge\": " + badge + "}}";

        var messageJSON = "{\"content\":\"您有一条来自于金军航的审批\",\"title\":\"新审批\", \"vibrate\":1, \"approval\": " + JsonConvert.SerializeObject(approval) + " }";

        parameters["message"] = messageJSON;
        parameters["message_type"] = "1";

        return parameters;
    }

    public string OpenReadWithHttps(string URL, string strPostdata)
    {
        Encoding encoding = Encoding.UTF8;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
        request.Method = "post";
        request.Accept = "text/html, application/xhtml+xml, */*";
        request.ContentType = "application/x-www-form-urlencoded";
        byte[] buffer = encoding.GetBytes(strPostdata);
        request.ContentLength = buffer.Length;
        request.GetRequestStream().Write(buffer, 0, buffer.Length);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8))
        {
            return reader.ReadToEnd();
        }
    }

    public int ConvertDateTimeInt(System.DateTime time)
    {
        double intResult = 0;
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        intResult = (time - startTime).TotalSeconds;
        return (int)intResult;
    }

    public String createSign(String url, Hashtable parameters)
    {
        StringBuilder sb = new StringBuilder("POST" + url);

        ArrayList akeys = new ArrayList(parameters.Keys);
        akeys.Sort();

        foreach (string k in akeys)
        {
            string v = (string)parameters[k];
            if (null != v && "".CompareTo(v) != 0
                && "sign".CompareTo(k) != 0 && "key".CompareTo(k) != 0)
            {
                sb.Append(k + "=" + v);
            }
        }

        sb.Append(this.getKey());
        Logger.Debug(sb.ToString());
        string sign = MD5Util.GetMD5(sb.ToString(), "UTF-8");
        Logger.Debug("sign=  " + sign);
        return sign;
    }

    public String GetPostData(Hashtable parameters)
    {
        StringBuilder sb = new StringBuilder();

        ArrayList akeys = new ArrayList(parameters.Keys);
        akeys.Sort();

        foreach (string k in akeys)
        {
            string v = (string)parameters[k];

            sb.Append(k + "=" + v + "&");

        }
        var result = sb.ToString().Substring(0, sb.Length - 1);
        Logger.Debug(result);
        return result;
    }

    private String getKey()
    {
        return SecretKey;

    }


}