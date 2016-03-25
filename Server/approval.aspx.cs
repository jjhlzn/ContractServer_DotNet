using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Newtonsoft.Json;

public partial class approval : System.Web.UI.Page
{
    private ApprovalService service = new ApprovalService();
    private static ILog Logger = LogManager.GetLogger(typeof(approval));

    protected void Page_Load(object sender, EventArgs e)
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
                resp = service.Search("0004", "", true, true, "", "", 0, 10);
                break;
            case "audit":
                resp = service.Audit("", "0004", "pass");
                break;
        }

        if (resp != null)
            Response.Write(JsonConvert.SerializeObject(resp));
        Response.End();
    }
}