using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// ApprovalService 的摘要说明
/// </summary>
public class ApprovalService
{
	public ApprovalService()
	{
	}

    public SearchApprovalResponse Search(string keyword, bool containPass, bool containUnpass,
        string startDate, string endDate, int pageNo, int pageSize)
    {
        SearchApprovalResponse resp = new SearchApprovalResponse();
        return resp;
    }

    public AuditApprovalResponse Audit(string approvalId, string result)
    {
        AuditApprovalResponse resp = new AuditApprovalResponse();
        return resp;
    }
}