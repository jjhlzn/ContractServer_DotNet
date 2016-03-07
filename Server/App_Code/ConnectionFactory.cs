using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// ConnectionFactory 的摘要说明
/// </summary>
public class ConnectionFactory
{
    public static IDbConnection GetInstance()
    {
        return new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
    }
}