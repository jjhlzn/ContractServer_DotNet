using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// PriceReport 的摘要说明
/// </summary>
public class PriceReport : BaseObject
{

    public string id { get; set; }
    public string version { get; set; }
    public string date { get; set; }
    public string status { get; set; }
    public string reporter { get; set; }
    public string detailInfo { get; set; }

    public List<PriceReportProduct> products = new List<PriceReportProduct>();

    public override string[] GetNeedResetProperties()
    {
        return new string[] { "id", "date", "status", "detailInfo" };
    }
}

public class PriceReportProduct : BaseObject
{
    public string id { get; set; } //商品编码
    public string huohuo { get; set; }
    public string specification { get; set; }
    public decimal price { get; set; }
    public string moneyType { get; set; }
    public string name { get; set; }
    public string englishName { get; set; }
    public string barCode { get; set; }
    public string chineseDesc { get; set; }
    public string englishDesc { get; set; }

public override string[] GetNeedResetProperties()
    {
        return new string[] { "id", "specification", "moneyType", "englishName" };
    }
}