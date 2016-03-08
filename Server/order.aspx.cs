using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dapper;
using Newtonsoft.Json;
using log4net;

public partial class order : System.Web.UI.Page
{
    private OrderService service = new OrderService();
    private static ILog Logger = LogManager.GetLogger(typeof (order));

    protected void Page_Load(object sender, EventArgs e)
    {
        
        //string action = Request["action"];

        var actionValue = Page.RouteData.Values["action"];
        Logger.Debug(RouteData.Values);
        if (actionValue == null)
        {
            Logger.Info("no action value");
            Response.End();
        }
        string action = actionValue.ToString();

        ServerResponse resp = null;
        switch (action)
        {
            case "search":
                resp = service.Search("", "", "", 0, 10);
                break;
            case "getBasicInfo":
                resp = service.GetBasicInfo("");
                break;
            case "getPurchaseInfo":
                resp = service.GetPurchaseInfo("");
                break;
            case "getChuyunInfo":
                resp = service.GetChuyunInfo("");
                break;
            case "getFukuangInfo":
                resp = service.GetFukuangInfo("");
                break;
            case "getShouhuiInfo":
                resp = service.GetShouhuiInfo("");
                break;
            
            default:
                //resp = service.Search("", "", "", 0, 10);
                break;
        }
    
        if (resp != null)
            Response.Write(JsonConvert.SerializeObject(resp));
        Response.End();
    }
}