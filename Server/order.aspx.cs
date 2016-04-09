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

public partial class order : BasePage
{
    private OrderService service = new OrderService();
    private static ILog Logger = LogManager.GetLogger(typeof (order));

   

    public override ServerResponse GetServerResponse()
    {
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
                resp = service.Search(GetRequestParameter("keyword"), GetRequestParameter("startdate"), GetRequestParameter("enddate"), GetIntRequestParameter("index"), GetIntRequestParameter("pagesize"));
                break;
            case "getBasicInfo":
                resp = service.GetBasicInfo(GetRequestParameter("orderId"));
                break;
            case "getPurchaseInfo":
                resp = service.GetPurchaseInfo(GetRequestParameter("orderId"));
                break;
            case "getChuyunInfo":
                resp = service.GetChuyunInfo(GetRequestParameter("orderId"));
                break;
            case "getFukuangInfo":
                resp = service.GetFukuangInfo(GetRequestParameter("orderId"));
                break;
            case "getShouhuiInfo":
                resp = service.GetShouhuiInfo(GetRequestParameter("orderId"));
                break;
        }
        return resp;
    }

    
}