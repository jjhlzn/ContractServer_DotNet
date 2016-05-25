<%@ Application Language="C#" %>
<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="Server" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="log4net" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        // 在应用程序启动时运行的代码
        BundleConfig.RegisterBundles(BundleTable.Bundles);
        AuthConfig.RegisterOpenAuth();
        RouteConfig.RegisterRoutes(RouteTable.Routes);
        Thread thread = new Thread(new ThreadStart(ThreadFunc));
        thread.IsBackground = true;
        thread.Name = "ThreadFunc";
        thread.Start();
    }
    

    protected void ThreadFunc()
    {
        System.Timers.Timer t = new System.Timers.Timer();
        t.Elapsed += new System.Timers.ElapsedEventHandler(TimerWorker);
        t.Interval = 3 * 60 * 1000;
        t.Enabled = true;
        t.AutoReset = true;
        t.Start();
    }

    protected void TimerWorker(object sender, System.Timers.ElapsedEventArgs e)
    {
        ILog logger = LogManager.GetLogger("Global");
        logger.Debug("TimerWorker");
        new NotificationMananger().CheckAndSendApprovalNotification();
    }

    
    void Application_End(object sender, EventArgs e)
    {
        //  在应用程序关闭时运行的代码

    }

    void Application_Error(object sender, EventArgs e)
    {
        // 在出现未处理的错误时运行的代码

    }

</script>
