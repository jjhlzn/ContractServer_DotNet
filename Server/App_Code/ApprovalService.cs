using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;

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
        using (IDbConnection conn = ConnectionFactory.GetInstance())
        {
            var result = conn.Query<Approval>("SELECT top 10 xh as id, '' as keyword, bssj as reportDate, sprymc as reporter, shdx as appovalObject, je as amount, [status], shlx as type  FROM [nbhz].[dbo].[t_spjl] ");
            resp.approvals = new List<Approval>(result);
            return resp;
        } 
    }

    public AuditApprovalResponse Audit(string approvalId, string result)
    {
        AuditApprovalResponse resp = new AuditApprovalResponse();
        return resp;
    }
}