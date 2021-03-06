﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Approval 的摘要说明
/// </summary>
public class Approval : BaseObject
{
	public Approval()
	{
	}

    public string id { get; set; }
    public string keyword { get; set; }
    public string type { get; set; }
    public string reporter { get; set; }
    public string approvalObject { get; set; }
    public decimal amount { get; set; }
    public string status { get; set; }
    public string reportDate { get; set; }
    public string approvalResult { get; set; }

    public override string[] GetNeedResetProperties()
    {
        return new string[] { "id", "keyword", "type", "reporter", "approvalObject", "status", "reportDate" };
    }
}