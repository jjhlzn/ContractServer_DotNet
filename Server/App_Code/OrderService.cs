using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;
using Newtonsoft.Json;

/// <summary>
/// OrderService 的摘要说明
/// </summary>
/// 
public class OrderService
{
	public OrderService()
	{
	}

    public SearchOrderResponse Search(string keyword, string startDate, string endDate, 
        int pageNo, int pageSize)
    {
        using (IDbConnection conn = ConnectionFactory.GetInstance())
        {
            var result = conn.Query<Order>("SELECT top 10 seller as businessPerson, seller_cname as guestName, wxhth as contractNo, khbm as orderNo, zje as amount FROM yw_contract");
            var response = new SearchOrderResponse();
            response.orders = new List<Order>(result);
            return response;
        }
    }


    public GetOrderBasicInfoResponse GetBasicInfo(string orderId)
    {
        GetOrderBasicInfoResponse response = new GetOrderBasicInfoResponse();

        return response;
    }

    public GetOrderPurchaseInfoResponse GetPurchaseInfo(string orderId)
    {
        GetOrderPurchaseInfoResponse resp = new GetOrderPurchaseInfoResponse();
        return resp;
    }

    public GetOrderChuyunInfoResponse GetChuyunInfo(string orderId)
    {
        GetOrderChuyunInfoResponse resp = new GetOrderChuyunInfoResponse();
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
