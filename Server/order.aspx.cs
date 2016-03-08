using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dapper;
using Newtonsoft.Json;

public partial class order : System.Web.UI.Page
{
    private OrderService service = new OrderService();

    protected void Page_Load(object sender, EventArgs e)
    {
        string action = Request["action"];
        ServerResponse resp = null;
        switch (action)
        {
            case "search":
                resp = service.Search("", "", "", 0, 10);
                break;
            case "getBasicInfo":

                break;
        }
    
        if (resp != null)
            Response.Write(JsonConvert.SerializeObject(resp));
        Response.End();
    }
}