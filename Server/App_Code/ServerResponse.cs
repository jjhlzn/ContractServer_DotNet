using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.Messaging;

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

    public virtual void ResetNullProperteis()
    {
        
    }
}



public class SearchOrderResponse : ServerResponse
{
    public int totalNumber { get; set; }
    public List<Order> orders { get; set; }

    public SearchOrderResponse()
    {
        orders = new List<Order>();
    }
    public override void ResetNullProperteis()
    {
        foreach (var order in orders)
        {
            order.resetNullProperties();
        }
    }
}

public class GetOrderBasicInfoResponse : ServerResponse
{
    public OrderBasicInfo basicInfo { get; set; }

    public GetOrderBasicInfoResponse()
    {
        basicInfo = new OrderBasicInfo();
    }
    public override void ResetNullProperteis()
    {
        basicInfo.resetNullProperties();
    }
}

public class OrderBasicInfo : BaseObject
{
    public string timeLimit { get; set; }
    public string startPort { get; set; }
    public string destPort { get; set; }
    public string getMoneyType { get; set; }
    public string priceRules { get; set; }

    public override string[] GetNeedResetProperties()
    {
        return new string[] { "timeLimit", "startPort", "destPort", "getMoneyType", "priceRules" };
    }
}

public class GetOrderPurchaseInfoResponse : ServerResponse
{
    public List<OrderPurchaseInfoItem> purchaseInfo { get; set; }

    public GetOrderPurchaseInfoResponse()
    {
        purchaseInfo = new List<OrderPurchaseInfoItem>();
    }
    public override void ResetNullProperteis()
    {
        foreach (var item  in purchaseInfo)
        {
            item.resetNullProperties();
        }
        
    }
}

public class OrderPurchaseInfoItem : BaseObject
{
    public string contract { get; set; }
    public string date { get; set; }
    public string factory { get; set; }
    public decimal amount { get; set; }
    public override string[] GetNeedResetProperties()
    {
        return new string[] { "contract", "date", "factory" };
    }
}

public class GetOrderChuyunInfoResponse : ServerResponse
{
    public OrderChuyunInfo chuyunInfo { get; set; }

    public GetOrderChuyunInfoResponse()
    {
        chuyunInfo = new OrderChuyunInfo();
    }
    public override void ResetNullProperteis()
    {
        chuyunInfo.resetNullProperties();
    }
}

public class OrderChuyunInfo : BaseObject
{
    public string detailNo { get; set; }
    public string date { get; set; }
    public decimal amount { get; set; }
    public override string[] GetNeedResetProperties()
    {
        return new string[] { "detailNo", "date" };
    }
}

public class GetOrderFukuangInfoResponse : ServerResponse
{
    public List<OrderFukuangInfoItem> fukuangInfo { get; set; }

    public GetOrderFukuangInfoResponse()
    {
        fukuangInfo = new List<OrderFukuangInfoItem>();
    }
    public override void ResetNullProperteis()
    {
        foreach (var item in fukuangInfo)
        {
            item.resetNullProperties();
        }
        
    }
}

public class OrderFukuangInfoItem : BaseObject
{
    public string contract { get; set; }
    public string date { get; set; }
    public string factory { get; set; }
    public decimal amount { get; set; }
    public override string[] GetNeedResetProperties()
    {
        return new string[] { "contract", "date", "factory" };
    }
}

public class GetOrderShouhuiInfoResponse : ServerResponse
{
    public OrderShouhuiInfo shouhuiInfo { get; set; }

    public GetOrderShouhuiInfoResponse()
    {
        shouhuiInfo = new OrderShouhuiInfo();
    }
    public override void ResetNullProperteis()
    {
        shouhuiInfo.resetNullProperties();
    }
}

public class OrderShouhuiInfo : BaseObject
{
    public string date { get; set; }
    public decimal amount { get; set; }
    public override string[] GetNeedResetProperties()
    {
        return new string[] {"date" };
    }
}

public class SearchApprovalResponse : ServerResponse
{
    public int totalNumber { get; set; }
    public List<Approval> approvals { get; set; }
    

    public SearchApprovalResponse()
    {
        approvals = new List<Approval>();
    }
    public override void ResetNullProperteis()
    {
        foreach (var item  in approvals)
        {
            item.resetNullProperties();
        }
        
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

public class GetProductResponse : ServerResponse
{
    public bool isExist { get; set; }
    public Product product { get; set; }
}


