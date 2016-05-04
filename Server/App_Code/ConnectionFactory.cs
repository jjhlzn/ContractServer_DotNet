using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using log4net;

/// <summary>
/// ConnectionFactory 的摘要说明
/// </summary>
public class ConnectionFactory
{
    private static ILog Logger = LogManager.GetLogger(typeof (ConnectionFactory));
    public static IDbConnection GetInstance()
    {
        Logger.DebugFormat("start get connection");
        IDbConnection conn =  new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        Logger.DebugFormat("end get connection");
        return conn;
    }

    public static IDbConnection GetImageInstance()
    {
        return new SqlConnection(ConfigurationManager.ConnectionStrings["ImageConnection"].ConnectionString);
    }
}