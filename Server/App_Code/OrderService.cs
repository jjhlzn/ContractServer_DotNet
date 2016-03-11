using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;
using Newtonsoft.Json;
using log4net;
using WebGrease.Css.Visitor;

/// <summary>
/// OrderService 的摘要说明
/// </summary>
/// 
public class OrderService
{
    private static ILog Logger = LogManager.GetLogger(typeof (OrderService));
	public OrderService()
	{
	}

    public SearchOrderResponse Search(string keyword, string startDate, string endDate, 
        int pageNo, int pageSize)
    {
        using (IDbConnection conn = ConnectionFactory.GetInstance())
        {
            int skipCount = pageNo*pageSize;
            string whereClause =
                string.Format(
                    @" yw_contract.ywy=rs_employee.e_no and yw_contract.khbm=yw_wldw.yw_khbm and yw_contract.bb_flag='Y' 
                        and qyrq >= @startDate and qyrq <= @endDate and (yw_contract.wxhth like '%{0}%' or po_no like '%{1}%') ", keyword, keyword);
            string sql = @"select top " + pageSize + @" name as businessPerson,wxhth as contractNo,po_no as orderNo,zje as amount,yw_contract.khmc as guestName, qyrq
                                            from yw_contract,rs_employee,yw_wldw
                                            where " + whereClause + @" and wxhth not in ( select top " + skipCount +
                         " wxhth from yw_contract,rs_employee,yw_wldw where"
                         + whereClause + " order by qyrq desc) order by qyrq desc";
            Logger.DebugFormat(sql);

            
            var result = conn.Query<Order>(sql , new { startDate, endDate });

            sql = "select COUNT(*) from " + whereClause;
            Logger.Debug(sql);
            var count = conn.Query<int>("select COUNT(*) from yw_contract,rs_employee,yw_wldw where " + whereClause, new { startDate, endDate }).First();
            var response = new SearchOrderResponse();
            response.orders = new List<Order>(result);
            response.totalNumber = count;
            return response;


        }
    }

    public GetOrderBasicInfoResponse GetBasicInfo(string orderId)
    {
        GetOrderBasicInfoResponse resp = new GetOrderBasicInfoResponse();
        string sql = @"SELECT  yw_contract.zyqx as timeLimit, 
                              yw_contract.ckka as startPort ,       
                              yw_contract.mdka as destPort,        
                              yw_contract.shfs as getMoneyType,       
                              yw_contract.jgtk as priceRules
                            FROM yw_contract      
                            WHERE ( yw_contract.wxhth = @orderId ) and ( yw_contract.bb_flag = 'Y' )";
        Logger.Debug(sql);
        using (var conn = ConnectionFactory.GetInstance())
        {
            var basicInfo = conn.Query<OrderBasicInfo>(sql, new {orderId}).FirstOrDefault();
            if (basicInfo != null && basicInfo.timeLimit != null)
            {
                basicInfo.timeLimit = basicInfo.timeLimit.Substring(0, basicInfo.timeLimit.IndexOf(' '));
            }
            resp.basicInfo = basicInfo;
        }
        return resp;
    }

    public GetOrderPurchaseInfoResponse GetPurchaseInfo(string orderId)
    {
        GetOrderPurchaseInfoResponse resp = new GetOrderPurchaseInfoResponse();
        string sql = @"select sghth as contract,
                              gfmc as factory,
                              qyrq as date,
                              zje as amount
                        from yw_bcontract
                        where wxhth=@orderId and
                              bb_flag='Y'";
        Logger.Debug(sql);
        Logger.DebugFormat("loggerId = {0}", orderId);
        using (var conn = ConnectionFactory.GetInstance())
        {
            var result = conn.Query<OrderPurchaseInfoItem>(sql, new {orderId});
            foreach (var item in result)
            {
                if (item.date != null)
                {
                    item.date = item.date.Substring(0, item.date.IndexOf(' '));
                }
            }
            resp.purchaseInfo = new List<OrderPurchaseInfoItem>(result);
        }
        return resp;
    }

    public GetOrderChuyunInfoResponse GetChuyunInfo(string orderId)
    {
        GetOrderChuyunInfoResponse resp = new GetOrderChuyunInfoResponse();
        string sql = @"SELECT mxdbh = yw_mxd.mxdbh,
                              amount =Sum(yw_mxd_cmd.wxzj),
                              fprq = yw_mxd.fprq 
                           FROM yw_mxd_cmd,yw_mxd  
                           WHERE yw_mxd_cmd.wxhth = @as_wxhth and
                        yw_mxd_cmd.mxdbh = yw_mxd.mxdbh and 
                        yw_mxd_cmd.bbh = yw_mxd.bbh and 
                        yw_mxd.bb_flag = 'Y'";
        using (var conn = ConnectionFactory.GetInstance())
        {
            
        }
        return resp;
    }

    public GetOrderFukuangInfoResponse GetFukuangInfo(string orderId)
    {
        GetOrderFukuangInfoResponse resp = new GetOrderFukuangInfoResponse();
        return resp;
    }

    public GetOrderShouhuiInfoResponse GetShouhuiInfo(string orderId)
    {
        GetOrderShouhuiInfoResponse resp = new GetOrderShouhuiInfoResponse();
        return resp;
    }
}
