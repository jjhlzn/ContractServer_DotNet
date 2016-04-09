
using log4net;


public partial class approval : BasePage
{
    private ApprovalService service = new ApprovalService();
    private static ILog Logger = LogManager.GetLogger(typeof(approval));

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
                resp = service.Search(GetRequestParameter("userid"),
                    GetRequestParameter("keyword"),
                    GetRequestParameter("containapproved") == "true", 
                    GetRequestParameter("containunapproved") == "true",
                    GetRequestParameter("startdate"), 
                    GetRequestParameter("enddate"), 
                    GetIntRequestParameter("index"), 
                    GetIntRequestParameter("pagesize"));
                break;
            case "audit":
                resp = service.Audit(GetRequestParameter("approvalid"), GetRequestParameter("userid"), GetRequestParameter("result"));
                break;
        }
        return resp;
    }

   
}