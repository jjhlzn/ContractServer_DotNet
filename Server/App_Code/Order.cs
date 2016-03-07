using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Order 的摘要说明
/// </summary>
public class Order
{
	
	public string businessPerson
	{
	    get;
	    set;
	}

    public string contractNo
    {
        get;
        set;
    }

    public string orderNo
    {
        get;
        set;
    }

    public decimal amount
    {
        get;
        set;
    }

    public string guestName
    {
        get;
        set;
    }
}