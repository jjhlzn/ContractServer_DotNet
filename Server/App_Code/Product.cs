using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Product 的摘要说明
/// </summary>
public class Product : BaseObject
{
    public string chineseName { get; set; }
    public string englishName { get; set; }
    public string huohao { get; set; }
    public string xinghao { get; set; }
    public decimal chengbenPrice { get; set; }
    public decimal sellPrice { get; set; }
    public string chineseDesc { get; set; }
    public string englishDesc { get; set; }
    public bool   hasImage { get; set; }
    public byte[] image { get; set; }
    public string id { get; set; }
    public string barCode { get; set; }

    public override string[] GetNeedResetProperties()
    {
        return new string[] { "chineseName", "englishName", "huohao", "xinghao", "chengbenPrice", "sellPrice", "chineseDesc", "englishDesc" };
    }
}