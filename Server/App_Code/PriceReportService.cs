using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

    public SearchPriceReportResponse Search(string userId, string keyword, string startDate, string endDate, int pageNo, int pageSize)
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
                    @" yw_quotation.bb_flag='Y' 
                        and bjrq >= @startDate and bjrq <= @endDate and (bjdh like '%{0}%') ", keyword);

            string sql = @"select top " + pageSize + @" bjdh as id, (select top 1 name from rs_employee b where yw_quotation.zdr = b.e_no) as reporter, bjrq as date,
                                                        yxts as validDays, state as status, wbbb as moneyType
                                            from yw_quotation 
                                            where " + whereClause + @" and bjdh not in ( select top " + skipCount +
                         " bjdh from yw_quotation  where"
                         + whereClause + " order by bjrq desc) order by bjrq desc";

            Logger.DebugFormat(sql);


            var result = conn.Query<PriceReport>(sql, new { startDate, endDate });

            var count = conn.Query<int>("select COUNT(*) from yw_quotation where " + whereClause, new { startDate, endDate }).First();

            response.reports = new List<PriceReport>(result);
            response.totalNumber = count;
            return response;
        }
    }



    public GetPriceReportResponse GetPriceReport(string reportId, int pageNo, int pageSize)
    {
        var response = new GetPriceReportResponse();

        using (IDbConnection conn = ConnectionFactory.GetInstance())
        {
            int skipCount = pageNo * pageSize;

            string whereClause =
                string.Format(
                    @"  bjdh = 'Q12HZ0002' and bbh = 1 ", keyword);

            string sql = @"select top " + pageSize + @" bjdh as id, (select top 1 name from rs_employee b where yw_quotation.zdr = b.e_no) as reporter, bjrq as date,
                                                        yxts as validDays, state as status, wbbb as moneyType
                                            from yw_quotation 
                                            where " + whereClause + @" and bjdh not in ( select top " + skipCount +
                         " bjdh from yw_quotation  where"
                         + whereClause + " order by bjrq desc) order by bjrq desc";

            Logger.DebugFormat(sql);


            var result = conn.Query<PriceReport>(sql, new { startDate, endDate });

            var count = conn.Query<int>("select COUNT(*) from yw_quotation where " + whereClause, new { startDate, endDate }).First();

            response.reports = new List<PriceReport>(result);
            response.totalNumber = count;
            return response;
        }
    }

    public SearchProductResponse SearchProducts(string userId, string codes)
    {
        return new SearchProductResponse();
    }

    public CreatePriceReportResponse CreatePriceReport(string userId, string codes)
    {
        return new CreatePriceReportResponse();
    }
}