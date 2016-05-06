using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using Antlr.Runtime.Tree;
using Dapper;
using log4net;
using log4net.Repository.Hierarchy;

/// <summary>
/// ApprovalService 的摘要说明
/// </summary>
public class ApprovalService
{
    private static ILog logger = LogManager.GetLogger(typeof(ApprovalService));
	public ApprovalService()
	{
	}

    public SearchApprovalResponse Search(string userId, string keyword, bool containPass, bool containUnpass,
        string startDate, string endDate, int pageNo, int pageSize)
    {

        SearchApprovalResponse resp = new SearchApprovalResponse();
        int skipCount = pageNo*pageSize;

        if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
        {
            logger.Debug("startdate or enddate is empty");
            return resp;
        }
        
        String whereClause = string.Format(@" t_spjl.spry = @userId 
                                            AND  t_spjl.enabled = '1' AND bssj >= '{0}' AND bssj <= '{1}'", startDate, endDate);
        

        if (containPass && containUnpass)
        {
            //empty 
        }
        else if (!containPass && containUnpass)
        {
            whereClause += " and t_spjl.status = '待批' ";
        }
        else if (containPass && !containUnpass)
        {
            whereClause += " and t_spjl.status != '待批' ";
        }
        else
        {
            return resp;
        }

        if (!string.IsNullOrEmpty(keyword))
        {
            whereClause += " and shgjz like '%" + keyword + "%' ";
        }

        String sql = @"SELECT top " +  pageSize + @"
                                                    xh as id, 
                                                    shgjz as keyword, 
                                                    bssj as reportDate, 
                                                    (SELECT name FROM rs_employee where rs_employee.e_no = bsry) as reporter, 
                                                    shdx as approvalObject, 
                                                    spjg as approvalResult,
                                                    je as amount, [status], 
                                                    shlx as type  
                                                        FROM [dbo].[t_spjl] where " + whereClause + @" and xh not in ( select top " + skipCount +
                         " xh from [dbo].[t_spjl] where"
                         + whereClause + " order by bssj desc ) order by bssj desc ";
        logger.Debug(sql);
        using (IDbConnection conn = ConnectionFactory.GetInstance())
        {
            var result = conn.Query<Approval>(sql, new {userId});
           
            foreach (var item in result)
            {
                if (item.reportDate != null)
                {
                    item.reportDate = item.reportDate.Substring(0, item.reportDate.IndexOf(' ')).Replace('/','-');
                    item.amount = decimal.Parse(item.amount.ToString("f2"));
                }
            }
            resp.approvals = new List<Approval>(result);
            var count = conn.Query<int>("select COUNT(*) from [dbo].[t_spjl] where " + whereClause, new { userId }).First();

            resp.totalNumber = count;
            return resp;
        } 
    }

    public AuditApprovalResponse Audit(string approvalId, string userId, string result)
    {
        AuditApprovalResponse resp = new AuditApprovalResponse();
        String baseDir = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
        ;
        Process myProcess = new Process();

        string fileName = string.Format(@"{0}bin\shenpi\shenpi.exe", baseDir);
        logger.Debug("fileName = " + fileName);


        string yijian = "";
        if (result == "0")
        {
            yijian = "tongyi";
        }
        else
        {
            yijian = "butongyi";
        }
        string para = string.Format(@"{0} {1} {2} {3}", approvalId, result, yijian, userId);

        ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(fileName, para);
        myProcessStartInfo.WorkingDirectory = string.Format(@"{0}bin\shenpi\", baseDir);

        myProcess.StartInfo = myProcessStartInfo;

        myProcess.Start();

        while (!myProcess.HasExited)
        {
            myProcess.WaitForExit();
        }

        int returnValue = myProcess.ExitCode;
        logger.Debug("returnValue = " + returnValue);
        resp.auditResult.result = true;
        return resp;
    }
}