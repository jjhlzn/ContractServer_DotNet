
using log4net;


public partial class product : BasePage
{
    private ProductService service = new ProductService();
    private static ILog Logger = LogManager.GetLogger(typeof(product));

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
                resp = service.GetProductById(GetRequestParameter("productid"));
                break;
           
        }
        return resp;
    }

   
}