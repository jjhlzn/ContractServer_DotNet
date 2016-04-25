using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Microsoft.Ajax.Utilities;

/// <summary>
/// Order 的摘要说明
/// </summary>
public class Order : BaseObject
{
	
	public string businessPerson { 
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

    public string moneyType { get; set; }

    public override string[] GetNeedResetProperties()
    {
        return new string[]{"businessPerson", "contractNo", "orderNo", "guestName", "moneyType"};
    }
}