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

public partial class reportPrice : BasePage
{
    private PriceReportService service = new PriceReportService();
    private static ILog Logger = LogManager.GetLogger(typeof(reportPrice));

   

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
                resp = service.Search(GetRequestParameter("userid"), GetRequestParameter("keyword"), GetRequestParameter("startDate"),  GetRequestParameter("endDate"),  GetIntRequestParameter("index"), GetIntRequestParameter("pagesize"));
                break;
            case "getPriceReport":
                resp = service.GetPriceReport(GetRequestParameter("reportId"), GetIntRequestParameter("index"), GetIntRequestParameter("pageSize"));
                break;
            case "searchProducts":
                resp = service.SearchProducts(GetRequestParameter("userid"), GetRequestParameter("codes"));
                break;
            case "submit":
                resp = service.CreatePriceReport(GetRequestParameter("userid"), GetRequestParameter("codes"));
                break;
        }
        return resp;
    }

    
}