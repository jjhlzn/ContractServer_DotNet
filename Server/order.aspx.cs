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
    protected void Page_Load(object sender, EventArgs e)
    {
        using (IDbConnection conn = ConnectionFactory.GetInstance())
        {
            var result = conn.Query<Order>("SELECT top 40 seller as businessPerson, seller_cname as guestName, wxhth as contractNo, khbm as orderNo, zje as amount FROM yw_contract");
            var response = new SearchOrderResponse();
            response.orders = new List<Order>(result);
            Response.Write(JsonConvert.SerializeObject(response));
            Response.End();
            
        }
    }
}