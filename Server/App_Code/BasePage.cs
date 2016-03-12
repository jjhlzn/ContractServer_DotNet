using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

/// <summary>
/// BasePage 的摘要说明
/// </summary>
public abstract class BasePage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //string action = Request["action"];

        ServerResponse resp = GetServerResponse();

        if (resp != null)
        {
            resp.ResetNullProperteis();
            Response.Write(JsonConvert.SerializeObject(resp));
        }
        Response.End();
    }

    public abstract ServerResponse GetServerResponse();

    protected string GetRequestParameter(string param)
    {
        string value = Request[param];
        if (value == null)
        {
            return "";
        }
        return value;
    }
}