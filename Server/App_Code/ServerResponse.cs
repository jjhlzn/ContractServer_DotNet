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

public class GetOrderBasicInfoResponse : ServerResponse
{
    public OrderBasicInfo basicInfo { get; set; }

    public GetOrderBasicInfoResponse()
    {
        basicInfo = new OrderBasicInfo();
    }
}

public class OrderBasicInfo
{
    public string timeLimit { get; set; }
    public string startPort { get; set; }
    public string destPort { get; set; }
    public string getMoneyType { get; set; }
    public string priceRules { get; set; }
}

public class GetOrderPurchaseInfoResponse : ServerResponse
{
    public List<OrderPurchaseInfoItem> purchaseInfo { get; set; }

    public GetOrderPurchaseInfoResponse()
    {
        purchaseInfo = new List<OrderPurchaseInfoItem>();
    }
}

public class OrderPurchaseInfoItem
{
    public string contract { get; set; }
    public string date { get; set; }
    public string factory { get; set; }
    public decimal amount { get; set; }
}

public class GetOrderChuyunInfoResponse : ServerResponse
{
    public OrderChuyunInfo chuyunInfo { get; set; }

    public GetOrderChuyunInfoResponse()
    {
        chuyunInfo = new OrderChuyunInfo();
    }
}

public class OrderChuyunInfo
{
    public string detailNo { get; set; }
    public string date { get; set; }
    public decimal amount { get; set; }
}

public class GetOrderFukuangInfoResponse : ServerResponse
{
    public OrderFukuangInfo fukuangInfo { get; set; }

    public GetOrderFukuangInfoResponse()
    {
        fukuangInfo = new OrderFukuangInfo();
    }
}

public class OrderFukuangInfo
{
    public List<OrderFukuangInfo> items { get; set; }

    public OrderFukuangInfo()
    {
        items = new List<OrderFukuangInfo>();
    }
}

public class OrderFukuangInfoItem
{
    public string contract { get; set; }
    public string date { get; set; }
    public string factory { get; set; }
    public decimal amount { get; set; }
}

public class GetOrderShouhuiInfoResponse : ServerResponse
{
    public OrderShouhuiInfo shouhuiInfo { get; set; }

    public GetOrderShouhuiInfoResponse()
    {
        shouhuiInfo = new OrderShouhuiInfo();
    }
}

public class OrderShouhuiInfo
{
    public string date { get; set; }
    public decimal amount { get; set; }
}

public class SearchApprovalResponse : ServerResponse
{
    public List<Approval> approvals { get; set; }

    public SearchApprovalResponse()
    {
        approvals = new List<Approval>();
    }
}

public class AuditApprovalResponse : ServerResponse
{
    public AuditResult auditResult { get; set; }

    public AuditApprovalResponse()
    {
        auditResult = new AuditResult();
    }
}

public class AuditResult
{
    public bool result { get; set; }
    public string message { get; set; }
}

public class LoginResponse : ServerResponse
{
    public LoginResult result { get; set; }
    public LoginResponse()
    {
        result = new LoginResult();
    }
}

public class LoginResult
{
    public bool success { get; set; }
    public string errorMessage { get; set; }
    public string name { get; set; }
    public string department { get; set; }
}


