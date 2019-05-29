using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using YOY.Model.DB;

namespace YOY.WCFService
{
    /// <summary>
    /// 通知管理接口的声明
    /// </summary>
    [ServiceContract]
    public interface INoticeManagement
    {
        /// <summary>
        /// 发布通知接口
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "PublishNotice")]
        Stream PublishNotice(string VisitorID , int NoticeType , DateTime OccurTime , string OccurAddress , string NoticeDetail);
        
        /// <summary>
        /// 查看我发布的通知接口
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "GetMyNotice")]
        Stream GetMyNotice(string VisitorID);

        /// <summary>
        /// 游客删除已发布的通知接口
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>登录结果的JSON</returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "DelMyNotice")]
        Stream DelMyNotice(string VisitorID , string NoticeID);

    }
}
