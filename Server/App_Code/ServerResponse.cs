using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Response 的摘要说明
/// </summary>
public class ServerResponse
{
    public int status {
        get;
        set;
    }

    public string errorMessage { get; set; }

    public ServerResponse()
    {
        status = 0;
        errorMessage = "";
    }
}



public class SearchOrderResponse : ServerResponse
{
    public int totalNumber { get; set; }
    public List<Order> orders { get; set; } 
}