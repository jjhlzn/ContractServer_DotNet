using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Newtonsoft.Json;

public partial class sendNotify : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        new NotificationMananger().CheckAndSendApprovalNotification();
        
    }

    
     
}