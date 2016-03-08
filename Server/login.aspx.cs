using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Newtonsoft.Json;

public partial class login : System.Web.UI.Page
{
    private LoginService service = new LoginService();
    private static ILog Logger = LogManager.GetLogger(typeof(login));

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
            case "login":
                resp = service.Login("", "");
                break;

        }

        if (resp != null)
            Response.Write(JsonConvert.SerializeObject(resp));
        Response.End();
    }
}