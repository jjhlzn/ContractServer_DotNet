using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Approval 的摘要说明
/// </summary>
public class Approval
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

}