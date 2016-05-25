using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Web;
using Dapper;
using log4net;

/// <summary>
/// NotificationMananger 的摘要说明
/// </summary>
public class NotificationMananger
{
    private static ILog Logger = LogManager.GetLogger(typeof (NotificationMananger));

    private ApprovalService approvalService = new ApprovalService();
    private NotificationService notificationService = new NotificationService();


    public void CheckAndSendApprovalNotification()
    {
        //加入一个随机睡眠，防止这些线程同时检查执行时间
        Random random = new Random(DateTime.Now.Millisecond);
        int seconds = random.Next(10);
        Logger.DebugFormat("sleep {0} seconds", seconds);
        Thread.Sleep(seconds * 1000);

        if (!IsContinue())
        {
            Logger.Debug("定时任务刚刚执行过，不用执行了");
            return;
        }
        else
        {
            Logger.Debug("继续执行");
        }
        //check tbl_user_device table to find userids
        List<User> users = GetUsers();

        //check the userids has approval notification 

        foreach (var user in users)
        {
            Logger.DebugFormat("check userid = {0}, platform = {1}, badeg = {2} approvals", user.id, user.platform, user.badge);
            List<Approval> approvals = GetApprovalsForNotification(user.id);

            foreach (var approval in approvals)
            {
                if (CheckSend(user, approval))
                {
                    Logger.DebugFormat("userid = {0}, approvalid = {1} has sent", user.id, approval.id);
                    continue;
                }

                user.badge++;
                
                String responseString = notificationService.Send(user.deviceToken, user.badge, GetMessage(approval),
                    approval, user.platform);

                if (isXgSuccess(responseString))
                {
                    Logger.DebugFormat("userid = {0}, approvalId = {1}, 消息发送成功", user.id, approval.id);
                    SetSendNotificationFlag(user, approval);
                }
            }
            updateBadge(user, user.badge);
        }
    }


    class CheckRecord
    {
        public DateTime lastCheckTime { get; set; }
    }

    //检查定时任务是否继续执行，如果是，则返回True，并且更新执行时间, 否则，返回false
    private bool IsContinue()
    {
        using (var conn = ConnectionFactory.GetInstance())
        {
            var result = conn.Query<CheckRecord>("SELECT TOP 1 lastCheckTime FROM tbl_check_record");
            if (result.Any())
            {
                var enumerate = result.GetEnumerator();
                enumerate.MoveNext();
                var lastCheckTime = enumerate.Current.lastCheckTime;
                if (lastCheckTime.AddSeconds(60).CompareTo(DateTime.Now) > 0) //刚刚发生，lastcheckTime在60s之内
                {
                    return false;
                }
                else
                {
                    conn.Execute("UPDATE tbl_check_record set lastCheckTime = @checkTime", new
                    {
                        checkTime = DateTime.Now
                    });
                    return true;
                }
                
            }
            else
            {
                conn.Execute("INSERT INTO tbl_check_record (lastCheckTime) values (@checkTime) ", new
                {
                    checkTime = DateTime.Now
                });
                return true;
            }
        }
    }

    private void updateBadge(User user, int newBadge)
    {
        using (var conn = ConnectionFactory.GetInstance())
        {
            conn.Execute("UPDATE tbl_user_device set badge = @badge WHERE userid = @userid", new
            {
                badge = newBadge,
                userid = user.id
            });
        }
    }

    private bool isXgSuccess(String responseString)
    {
        try
        {
            var xgResp = JSONSerializer<XgResponse>.DeSerialize(responseString);
            if (xgResp.ret_code == "0")
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            Logger.Fatal(ex, ex);
        }
        return false;
    }

    private String GetMessage(Approval approval)
    {
        return string.Format("您有一条来自{0}的审批", approval.reporter);
    }


    public List<User> GetUsers()
    {
        using (var connection = ConnectionFactory.GetInstance())
        {
            var result = connection.Query<User>("Select userid as id, devicetoken as deviceToken, platform, badge From tbl_user_device where devicetoken != ''");
            return new List<User>(result);
        }
    }

    public List<Approval> GetApprovalsForNotification(String userId)
    {
        String endDate = DateTime.Now.ToString("yyyy-MM-dd");
        String startDate = DateTime.Now.AddDays(-1500).ToString("yyyy-MM-dd");
        SearchApprovalResponse resp = approvalService.Search(userId, "", false, true, startDate, endDate, 0, 10000);
        if (resp.status == 0)
        {
            return resp.approvals;
        }
        return new List<Approval>();
    }

    private void SetSendNotificationFlag(User user, Approval approval)
    {
        using (var connection = ConnectionFactory.GetInstance())
        {
            connection.Execute(
                "INSERT INTO tbl_notification_send_records (approvalid, userid, devicetoken, platform, sendTime) " +
                " VALUES (@approvalid, @userid, @devicetoken, @platform, @sendTime)", new
                {
                    approvalid = approval.id,
                    userid = user.id,
                    devicetoken = user.deviceToken,
                    platform = user.platform,
                    sendTime = DateTime.Now

                });
        }
    }

    private bool CheckSend(User user, Approval approval)
    {
        using (var connection = ConnectionFactory.GetInstance())
        {
           var result = connection.Query(
                "SELECT * FROM tbl_notification_send_records WHERE approvalid = @approvalid and userid = @userid", new
                {
                    approvalid = approval.id,
                    userid = user.id
                });
            return result.Any();
        }
    }

    public static class JSONSerializer<TType> where TType : class
    {
        /// <summary>
        /// Serializes an object to JSON
        /// </summary>
        public static string Serialize(TType instance)
        {
            var serializer = new DataContractJsonSerializer(typeof(TType));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, instance);
                return Encoding.Default.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// DeSerializes an object from JSON
        /// </summary>
        public static TType DeSerialize(string json)
        {
            using (var stream = new MemoryStream(Encoding.Default.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof(TType));
                return serializer.ReadObject(stream) as TType;
            }
        }
    }

    [DataContract]
    public class XgResponse
    {
        [DataMember]
        public string ret_code { get; set; }

    }
}