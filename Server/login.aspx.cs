using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Newtonsoft.Json;

public partial class login : BasePage
{
    private LoginService service = new LoginService();
    private static ILog Logger = LogManager.GetLogger(typeof(login));

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
            case "login":
                resp = service.Login(GetRequestParameter("x"), GetRequestParameter("y"));
                break;
            case "registerdevice":
                resp = service.RegisterDevice(GetRequestParameter("username"), GetRequestParameter("platform"),
                    GetRequestParameter("devicetoken"));
                break;
            case "resetbadge":
                resp = service.ResetBadge(GetRequestParameter("username"));
                break;

        }
        return resp;
    }
}