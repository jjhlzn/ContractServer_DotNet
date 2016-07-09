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
    public string validDays { get; set; }

    public List<PriceReportProduct> products = new List<PriceReportProduct>();

    public override string[] GetNeedResetProperties()
    {
        return new string[] { "id", "date", "status", "detailInfo" };
    }
}

public class PriceReportProduct : BaseObject
{
    private String _huohao;
    private String _xinghao;
    private String _unit;

    public string huohuo
    {
        get
        {
            if (string.IsNullOrEmpty(_huohao))
            {
                return "--";
            }
            return _huohao;
        }
        set { _huohao = value; }
    }

    public string specification
    {
        get
        {
            if (string.IsNullOrEmpty(_xinghao))
            {
                return "--";
            }
            return _xinghao;
        }
        set { _xinghao = value; }
    }

    public string id { get; set; } //商品编码
    public decimal price { get; set; }
    public string moneyType { get; set; }
    public string name { get; set; }
    public string englishName { get; set; }
    public string barCode { get; set; }
    public string chineseDesc { get; set; }
    public string englishDesc { get; set; }
    

    public string unit
    {
        get
        {
            if (string.IsNullOrEmpty(_unit))
            {
                return "--";
            }
            return _unit;
        }
        set { _unit = value; }
    }

    public override string[] GetNeedResetProperties()
    {
        return new string[] { "id", "specification", "moneyType", "englishName" };
    }
}