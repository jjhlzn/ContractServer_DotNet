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
using Newtonsoft.Json;

public partial class sendNotify : System.Web.UI.Page
{
    private ILog Logger = LogManager.GetLogger(typeof (sendNotify));
    private String serviceUrl = "openapi.xg.qq.com/v2/push/single_device";


    protected void Page_Load(object sender, EventArgs e)
    {
        String platform = Request.Params["platform"];

        NotificationService notificationService = new NotificationService();
        Approval approval = new Approval();
        approval.id = "111";
        approval.keyword = "keyword";
        approval.type = "type";
        approval.reporter = "reporter";
        approval.approvalObject = "审批对象";
        approval.amount = 100;
        approval.status = "待批";
        approval.reportDate = "2016-05-01";
        approval.approvalResult = "";

        string responseString = "";

        if (platform == "android")
        {
            responseString = notificationService.Send("7e720d4e7b2ffadd46ad85f23237568a75b64c1c", 5,
                "您有一条来自于金军航的审批", approval, "android");
        }
        else
        {
            responseString = notificationService.Send("35ac9e03445d666ad62e750e2b6b13281ccc9efcad5e3e395bfccaaf2b1369bf", 1,
                "您有一条来自于金军航的审批", approval, "ios");
        }

        /*
        var url = "http://" + serviceUrl;

        Hashtable parameters = new Hashtable();
        parameters["access_id"] = "2200199196";
        parameters["timestamp"] = ConvertDateTimeInt(DateTime.Now).ToString();


        var message = "{\"aps\":{\"alert\":\"您有一条来自于金军航的审批\", \"sound\":\"default\", \"badge\": 3, \"approval\": " + JsonConvert.SerializeObject(approval)  + "  }}";
        Logger.Debug(message);
        //var message = "{}";
        parameters["message"] = message;
        parameters["message_type"] = "0";
        parameters["environment"] = "2";
        parameters["device_token"] = "005fb11b6d182ec3aa156f27a20c2f65f158def3430ac71425a0d8cac4393c31";

        parameters["sign"] = createSign(serviceUrl, parameters);

        var postData = GetPostData(parameters);

        var responseString = OpenReadWithHttps(url, postData); */

        Response.Write(responseString);

        
    }

    
     
}