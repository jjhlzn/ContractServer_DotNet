using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Dapper;
using log4net;

/// <summary>
/// PriceReportService 的摘要说明
/// </summary>
public class PriceReportService
{
    private static ILog Logger = LogManager.GetLogger(typeof (PriceReportService));

	public PriceReportService()
	{
	}

    public SearchPriceReportResponse Search(string userid, string keyword, string startDate, string endDate, int pageNo, int pageSize)
    {
        var response = new SearchPriceReportResponse();
        if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
        {
            Logger.Debug("startdate or enddate is empty");
            return response;
        }

        Logger.DebugFormat("startDate = {0}, endDate = {1}", startDate, endDate);

        using (IDbConnection conn = ConnectionFactory.GetInstance())
        {
            int skipCount = pageNo * pageSize;

            string whereClause =
                string.Format(
                    @" yw_quotation.bb_flag='Y' and  yw_quotation.ywy in (select scope from t_m_o_scope where t_m_o_scope.m_no = 'yw' and t_m_o_scope.e_no = @userid) 
                        and bjrq >= @startDate and bjrq <= @endDate and (bjdh like '%{0}%') ", keyword);

            string sql = @"select top " + pageSize + @" bjdh as id,  (select top 1 name from rs_employee b where yw_quotation.zdr = b.e_no) as reporter,
                                                        CONVERT(varchar(100), bjrq, 23) as date,
                                                        yxts as validDays, state as status, wbbb as moneyType, bbh as version
                                            from yw_quotation 
                                            where " + whereClause + @" and bjdh not in ( select top " + skipCount +
                         " bjdh from yw_quotation  where"
                         + whereClause + " order by bjrq desc) order by bjrq desc";

            Logger.DebugFormat(sql);


            var result = conn.Query<PriceReport>(sql, new { startDate, endDate, userid });
            foreach (var report in result)
            {
                report.date = report.date.Substring(0, 10);
            }

            var count = conn.Query<int>("select COUNT(*) from yw_quotation where " + whereClause, new { startDate, endDate, userid }).First();

            response.reports = new List<PriceReport>(result);
            setDetailInfo(response.reports);
            response.totalNumber = count;
            return response;
        }
    }


    class Dummy
    {
        public string id { get; set; }
        public String name { get; set; }
    }
    private void setDetailInfo(List<PriceReport> reports)
    {
        using (IDbConnection conn = ConnectionFactory.GetInstance())
        {
            string sql = @" select bjdh as id, (select top 1 spzwmc from yw_commodity c where c.yw_spbm = yw_quotation_cmd.spbm) as name from yw_quotation_cmd  ";

            string where = "";

            foreach (var report in reports)
            {
                where += string.Format(" (bjdh = '{0}' and bbh = {1}) or", report.id, report.version);
            }

            if (string.IsNullOrEmpty(where))
            {
                return;
            }

            where = where.Substring(0, where.Length - 2);

            sql = sql + " where " + where + " order by  bjdh, cxh ";

            var result = conn.Query<Dummy>(sql);

            foreach (var item in result)
            {
                foreach (var report in reports)
                {
                    if (report.id.Equals(item.id))
                    {
                        report.detailInfo += string.Format("{0}, ", item.name);
                    }
                }
            }

            //去掉多余", "
            foreach (var report in reports)
            {
                if (!string.IsNullOrEmpty(report.detailInfo))
                {
                    report.detailInfo = report.detailInfo.Substring(0, report.detailInfo.Length - 2);
                }
            }
            
        }
    } 


    private String GetVersion(String reportId)
    {
        using (IDbConnection conn = ConnectionFactory.GetInstance())
        {
            string sql = "select top 1 bbh from yw_quotation where bjdh = @reportId and bb_flag = 'Y' order by bbh desc";

            var result = conn.Query<String>(sql, new {reportId});
            if (result.Count() == 0)
            {
                return string.Empty;
            }

            return result.First();
        }
    }


    public GetPriceReportResponse GetPriceReport(string reportId)
    {
        var response = new GetPriceReportResponse();

        using (IDbConnection conn = ConnectionFactory.GetInstance())
        {
            String version = GetVersion(reportId);

            if (string.IsNullOrEmpty(version))
            {
                response.status = -1;
                response.errorMessage = "报价单号不存在";
                return response;
            }

            string whereClause =
                string.Format(
                    @"  bjdh = @reportId and bbh = @version ");

            string sql = @"select sphh as id, sphh as huohuo, spgg as specificaton, spywmc as englishName , bhsdj as price ,  str(jjsl) + '/' + jjdw as unit, 
                                                        (select top 1 spzwmc from yw_commodity c where c.yw_spbm = yw_quotation_cmd.spbm) as name,
                                                        (select top 1 wbbb from yw_quotation b where b.bjdh = 'Q12HZ0002' and b.bbh = 1 ) as moneyType
                                            from yw_quotation_cmd 
                                            where " + whereClause + " order by cxh ";

            Logger.DebugFormat(sql);


            var result = conn.Query<PriceReportProduct>(sql, new { reportId,version });
            foreach (var order in result)
            {
                if (order.moneyType == "RMB")
                {
                    order.moneyType = "¥";
                }
                else if (order.moneyType == "USD")
                {
                    order.moneyType = "$";
                }
            }

            var count = result.Count();

            response.products = new List<PriceReportProduct>(result);
            response.totalNumber = count;
            return response;
        }
    }

    public SearchProductResponse SearchProducts(string userId, string codeString)
    {
        SearchProductResponse resp = new SearchProductResponse();

        List<PriceReportProduct> products = new List<PriceReportProduct>();

        PriceReportProduct product = new PriceReportProduct();
        product.id = "3007000001";
        product.huohuo = "ES-2810";
        product.name = "边框枕头";
        product.price = 100;
        product.englishName = "CONTINENTAL PILLOW CASE";
        product.barCode = "6162003134527";
        product.specification = "10cm * 20cm";
        product.unit = "1/PCS";
        products.Add(product);

        product = new PriceReportProduct();
        product.id = "3007000003";
        product.huohuo = "ES-2811";
        product.name = "毛巾环";
        product.price = 100;
        product.englishName = "HAND TOWEL RING";
        product.barCode = "6162003136101";
        product.specification = "10cm * 20cm";
        product.unit = "1/PCS";
        products.Add(product);
        resp.products = products;
        return resp;
    }

    public SearchProductResponse SearchProducts2(string userId, string codeString)
    {
        SearchProductResponse resp = new SearchProductResponse();
        List<String> codes = parseCode(codeString);
        if (codes.Count == 0) {
            return resp;
        }

        string sql = @"select spzwmc as name,
                        spywmc as englishName,
                        sphh as huohao,
                        spggms as chineseDesc,
                        spggywms as englishDesc, 
                        spgg as specification,
                        yw_spbm as id,
                        txm as barCode
                        
                        from yw_commodity ";

        string where = " where  txm in (";

        foreach (var code  in codes)
        {
            where += string.Format(" '{0}',", code);
        }

        where = where.Substring(0, where.Length - 1);

        sql += where + ")";

        Logger.Debug(sql);
        using (var conn = ConnectionFactory.GetInstance())
        {
            var result = conn.Query<PriceReportProduct>(sql);
           
            resp.products = new List<PriceReportProduct>(result);
        }
        return resp;
    }

    private String getName(string userId)
    {
        using (var conn = ConnectionFactory.GetInstance())
        {
            var sql = " select top 1 name from rs_employee b where e_no = @userId";
            var result = conn.Query<String>(sql, new {userId});
            if (result.Count() == 0)
            {
                return "";
            }
            return new List<String>(result)[0];
        }
    }

    public CreatePriceReportResponse CreatePriceReport(string userId, string codeString, string companyName, string contactPerson, string contactPhone)
    {
        //TODO: mocck
        codeString = "____6162003134527____6162003136101";
        var resp = new CreatePriceReportResponse();
        List<String> codes = parseCode(codeString);

        if (codes.Count == 0)
        {
            resp.status = -1;
            resp.errorMessage = "参数没有任何商品";
            return resp;
        }

        List<PriceReportProduct> products = SearchProducts(userId, codeString).products;
        if (products.Count != codes.Count)
        {
            resp.status = -1;
            resp.errorMessage = "有些扫描到的商品不能在数据库中找到";
            return resp;
        }

        using (var conn = ConnectionFactory.GetInstance())
        {
            conn.Open();
            var trasaction = conn.BeginTransaction();

            try
            {
                //插入主表
                string sql = @"INSERT INTO yw_quotation (bjdh, zdr, bjrq, yxts, state, wbbb, bbh, ywy, bb_flag, tt_no, khmc, lxr, tel) 
                               VALUES (@id, @userId, @date, @validDays, @status, @moneyType, @version, @ywy, @flag, @ttno, @companyName, @contactPerson, @contactPhone)";

                string id = getNextComputeId();
                Logger.DebugFormat("newid = {0}", id);
                Logger.Debug(sql);
                int rowCount = conn.Execute(sql,
                    new
                    {
                        id,
                        userId,
                        date = DateTime.Now,
                        validDays = 31,
                        status = "新制",
                        moneyType = "USD",
                        version = 1,
                        ywy = userId,
                        flag = "Y",
                        ttno = "01’",
                        companyName,
                        contactPerson,
                        contactPhone
                    }, trasaction);

                //插入分表
                if (rowCount != 1)
                {
                    throw new Exception("主表插入失败");
                }

                var index = 1;
                var detailInfo = "";
                foreach (var product in products)
                {
                    sql = @"INSERT INTO yw_quotation_cmd (bjdh, bbh, cxh, spbm, spywmc, spgg, sphh, bhsdj) 
                          VALUES (@id, @version, @index, @productId, @englishName, @specification, @huohao, @price)";

                    rowCount = conn.Execute(sql, new
                    {
                        id,
                        version = 1,
                        index,
                        productId = product.id,
                        product.englishName,
                        product.specification,
                        huohao = product.huohuo,
                        price = 0
                    }, trasaction);
                    Logger.Debug(sql);
                    if (rowCount != 1)
                    {
                        throw new Exception("主表插入失败");
                    }

                    index++;
                    detailInfo += string.Format("{0}, ", product.name);
                }
                detailInfo = detailInfo.Substring(0, detailInfo.Length - 2);
                trasaction.Commit();
                var report = new PriceReport();
                report.id = id;
                report.date = DateTime.Now.ToString("yyyy-MM-dd");
                report.reporter = getName(userId);
                report.products = products;
                report.detailInfo = detailInfo;
                report.status = "新制";
                resp.report = report;

            }
            catch (Exception ex)
            {
                resp.status = -1;
                resp.errorMessage = "服务器出错";
                Logger.Fatal(ex, ex);
                Logger.Fatal("回滚事务");
                trasaction.Rollback();
            }

        }
        return resp;
    }

    private string getNextComputeId()
    {
        DateTime today = DateTime.Now.Date;
        string prefix = "BJD" + today.ToString("yyMMdd") ;
        return prefix + getNextSequence(prefix);
    }

    private string getNextSequence(string prefix)
    {
        using (var conn = ConnectionFactory.GetInstance())
        {
            DateTime yesterday = DateTime.Now.Date.AddDays(-1);
            string sql = "SELECT top 1 bjdh from yw_quotation where bjrq > @yesterday and bjdh like '" + prefix + "[0-9][0-9][0-9][0-9][0-9]' order by bjdh desc";
            var result = conn.Query<String>(sql, new { yesterday});
            if (result.Count() == 0)
            {
                return "00001";
            }
            else
            {
                return getSequencePart(new List<String>(result)[0]);
            }
        }
    }


    static string getSequencePart(String id)
    {
        string suffix = id.Substring(9);

        int value = int.Parse(suffix);

        return (value + 1).ToString("D5");
    }

    private List<String> parseCode(string codeString)
    {
        string[] codes = Regex.Split(codeString, "____");
        List<String> result = new List<string>();
        foreach (var code  in codes)
        {
            if (!string.IsNullOrEmpty(code))
            {
                result.Add(code);
            }
        }
        return result;
    }
}