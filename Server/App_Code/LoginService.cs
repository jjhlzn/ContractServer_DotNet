using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using Dapper;
using log4net;

/// <summary>
/// LoginService 的摘要说明
/// </summary>
public class LoginService
{
    private ILog Logger = LogManager.GetLogger(typeof(LoginService));

	public LoginService()
	{
	}

    private static String letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public RegisterDeviceResponse RegisterDevice(string userName, string platform, string deviceToken)
    {
        RegisterDeviceResponse resp = new RegisterDeviceResponse();

        //check the userName exist in tbl_user_device
        //1. if not, add a new record
        //2. if exist, update the record
        if (string.IsNullOrEmpty(userName))
        {
            resp.status = -1;
            return resp;
        }

        using (var conn = ConnectionFactory.GetInstance())
        {
            string sql = "SELECT * FROM tbl_user_device WHERE userid = @userName";
            if ( conn.Query(sql, new {userName}).Any())
            {
                conn.Execute(
                    "UPDATE tbl_user_device set deviceToken = @deviceToken, platform = @platform  WHERE userid = @userName",
                    new {deviceToken, platform, userName});
            }
            else
            {
                conn.Execute(
                    "INSERT INTO tbl_user_device (userid, platform, deviceToken) values(@userName, @platform, @deviceToken)",
                    new {deviceToken, platform, userName});
            }
        }

        return resp;
    }

    public ResetBadgeResponse ResetBadge(string userName)
    {
        ResetBadgeResponse resp = new ResetBadgeResponse();

        //execute update sql
        using (var conn = ConnectionFactory.GetInstance())
        {
            conn.Execute(
                "UPDATE tbl_user_device set badge = 0  WHERE userid = @userName", new { userName });
        }

        return resp;
    }

   

    public LoginResponse Login(string userName, string password)
    {
        Logger.DebugFormat("login start");
        LoginResponse resp = new LoginResponse();
        LoginResult loginResult = new LoginResult();
        loginResult.success = false;
        resp.result = loginResult;
        string sql = @"SELECT e_no as  id, e_name as name, d_name as department, password FROm t_operator where e_no = @userName";
        Logger.Debug(sql);
        using (var conn = ConnectionFactory.GetInstance())
        {
            var user = conn.Query<User>(sql, new { userName }).FirstOrDefault();
            if (user == null)
            {
               
                loginResult.errorMessage = "用户名不存在";
                Logger.DebugFormat("login end");
                return resp;
            }

            String decrptPassword = Decrpt(userName, user.password);
            Logger.DebugFormat("originPassword = {0}, dbPassword = {1} decrptPassword = {2}", password, user.password, decrptPassword);
            if (decrptPassword != password.ToUpper())
            {
                loginResult.errorMessage = "密码错误";
            }
            else
            {
                loginResult.success = true;
                loginResult.name = user.name;
                loginResult.department = user.department;
            }
            Logger.DebugFormat("login end");
            return resp;
        }
    }

    private String Decrpt(string userName, string password)
    {
        if (String.IsNullOrEmpty(userName) || String.IsNullOrEmpty(password))
            return "";

        String result = "";
        userName = userName.ToUpper();
        password = password.ToUpper();
        int j = 9999;
        for (int i = 0; i < password.Length; i ++)
        {
            j++;
            if (j >  userName.Length - 1)
            {
                j = 0;
            }
           // Logger.Debug("j = " + j);
            String decodeString = LoadDecode(userName[j]);
           // Logger.DebugFormat("decodeString = {0}", decodeString);
            for (int k = 0; k < letters.Length; k++)
            {
                if (password[i] == decodeString[k])
                {
                   // Logger.DebugFormat("i = {0}, {1}", i, letters[k]);
                    result += letters[k];
                    break;
                }
            }
        }
        //79NC8L9
        
        return Reverse(result);
    }

    private String Reverse(String str)
    {
        if (string.IsNullOrEmpty(str) || str.Length == 1)
            return str;


        return Reverse(str.Substring(1, str.Length - 1)) + str[0];
    }

    private String LoadDecode(char arg)
    {
        String decodeString = "";
        int start = 0;
        for(int i = 0; i < letters.Length; i++)
        {
            if (letters[i] == arg)
            {
                start = i;
                break;
            }
        }
      //  Logger.DebugFormat("start = {0}", start);
       // int nbyte = 0;
        for (int i = start; i < letters.Length; i++)
        {
            
            decodeString += letters[i];
            //nbyte++;
        }

        for (int i = 0; i < start; i ++)
        {
           
            decodeString += letters[i];
           // nbyte++;
        }

        return decodeString;
    }
}