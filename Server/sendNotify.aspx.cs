using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

public partial class sendNotify : System.Web.UI.Page
{
    private ILog Logger = LogManager.GetLogger(typeof (sendNotify));
    private String serviceUrl = "openapi.xg.qq.com/v2/push/single_device";
     

    protected void Page_Load(object sender, EventArgs e)
    {
        var url = "http://" + serviceUrl;

        Hashtable parameters = new Hashtable();
        parameters["access_id"] = "2200199196";
        parameters["timestamp"] = ConvertDateTimeInt(DateTime.Now).ToString();
       

        var message = "{\"aps\":{\"alert\":\"gogogo\", \"sound\":\"default\", \"badge\": 3}}";
        //var message = "{}";
        parameters["message"] = message;
        parameters["message_type"] = "0";
        parameters["environment"] = "2";
        parameters["device_token"] = "005fb11b6d182ec3aa156f27a20c2f65f158def3430ac71425a0d8cac4393c31";

        parameters["sign"] = createSign(serviceUrl, parameters);

        var postData = GetPostData(parameters);

        var responseString = OpenReadWithHttps(url, postData);

        Logger.Debug("response = " + responseString);
        Response.Write(responseString);

        
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
        StringBuilder sb = new StringBuilder("POST"+url);

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
        return "3ba8903b6c3ae3b74cf2eebedfcc6188";

    }
     
}