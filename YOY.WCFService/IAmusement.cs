using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using YOY.Model;

namespace YOY.WCFService
{
    /// <summary>
    /// 游园子系统操作接口
    /// </summary>
    [ServiceContract]
    public interface IAmusement
    {
        /// <summary>
        /// 游园子系统登录接口
        /// </summary>
        /// <param name="visitor">游客实体</param>
        /// <returns>登陆结果的JSON字符串</returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "Login")]
        Stream Login(Visitor visitor);

        /// <summary>
        /// 获取游园通知信息
        /// </summary>
        /// <param name="LastGetTime">上次刷新时间，如果为null，则返回最近5条，如果不为空，则返回在上次刷新之后又新增的通知</param>
        /// <returns>通知信息的JSON字符串</returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "GetNotice")]
        Stream GetNotice(string LastGetTime);

        /// <summary>
        /// 获取组队申请信息
        /// </summary>
        /// <param name="VisitorID">游客ID</param>
        /// <returns>组队申请结果记录JSON字符串</returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "Group/GetApplication/{VisitorID}")]
        Stream GetApplication(string VisitorID);

        /// <summary>
        /// 同意组队
        /// </summary>
        /// <param name="VisitorID">游客ID</param>
        /// <param name="InviterID">邀请者ID</param>
        /// <returns>结果JSON</returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "Group/Agree")]
        Stream Agree(string VisitorID, string InviterID);

        /// <summary>
        /// 拒绝组队
        /// </summary>
        /// <param name="VisitorID">游客ID</param>
        /// <param name="InviterID">邀请者ID</param>
        /// <returns>结果JSOn</returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "Group/Refuse")]
        Stream Refuse(string VisitorID, string InviterID);

        /// <summary>
        /// 邀请游客组队
        /// </summary>
        /// <param name="InviterID">发出邀请的游客ID</param>
        /// <param name="InviteeID">被邀请的游客ID</param>
        /// <returns>邀请结果JSON字符串</returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "Group/Invite")]
        Stream Invite(string InviterID, string InviteeID);

        /// <summary>
        /// 查找游客
        /// </summary>
        /// <param name="SearchKey">可能为游客ID或名字</param>
        /// <returns>游客相关信息JSON字符串</returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "Group/SearchVisitor?SearchKey={SearchKey}")]
        Stream SearchVisitor(string SearchKey);

        /// <summary>
        /// 退出队伍
        /// </summary>
        /// <param name="VisitorID">退出队伍的游客ID</param>
        /// <returns>结果JSON字符串</returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "Group/Exit/{VisitorID}")]
        Stream VisitorID(string VisitorID);

    }
}
