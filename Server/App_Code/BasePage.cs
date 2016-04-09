using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using log4net;
using Newtonsoft.Json;

/// <summary>
/// BasePage 的摘要说明
/// </summary>
public abstract class BasePage : System.Web.UI.Page
{
    private ILog Logger = LogManager.GetLogger(typeof (BasePage));
    protected void Page_Load(object sender, EventArgs e)
    {

        var actionValue = Page.RouteData.Values["action"];

        ServerResponse resp = GetServerResponse();

        if (resp != null)
        {
            if (actionValue.ToString() == "getImage")
            {
                Logger.Debug("getImage");
                Response.ContentType = "image";
                Response.OutputStream.Write(((GetProductResponse)resp).product.image, 0, ((GetProductResponse)resp).product.image.Length);
            }
            else
            {
                resp.ResetNullProperteis();
                Response.Write(JsonConvert.SerializeObject(resp));
            }
            
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

    protected int GetIntRequestParameter(string param)
    {
        string value = Request[param];
        if (value == null)
        {
            return 0;
        }
        try
        {
            return int.Parse(value);

        }
        catch (Exception e)
        {
            return 0;
        }
  
    }
}