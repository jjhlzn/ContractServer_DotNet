using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class log : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DownloadFile(Server.MapPath("Log/log_root.txt"));
    }


    public void DownloadFile(string fileRpath)
    {

        Response.ClearHeaders();
        Response.Clear();
        Response.Expires = 0;
        Response.Buffer = true;
        Response.AddHeader("Accept-Language", "zh-tw");
        string name = System.IO.Path.GetFileName(fileRpath);
        System.IO.FileStream files = new FileStream(fileRpath, FileMode.Open, FileAccess.Read, FileShare.Read);
        byte[] byteFile = null;
        if (files.Length == 0)
        {
            byteFile = new byte[1];
        }
        else
        {
            byteFile = new byte[files.Length];
        }
        files.Read(byteFile, 0, (int)byteFile.Length);
        files.Close();

        Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(name, System.Text.Encoding.UTF8));
        Response.ContentType = "application/octet-stream;charset=gbk";
        Response.BinaryWrite(byteFile);
        Response.End();

    }
}