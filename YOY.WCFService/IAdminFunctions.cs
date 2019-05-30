using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using YOY.Model.DB;

namespace YOY.WCFService
{
    
    /// <summary>
    /// 管理员操作接口声明
    /// </summary>
    [ServiceContract]
    public interface IAdminFunctions
    {
        /// <summary>
        /// 获取所有给定日期的通知信息
        /// </summary>
        /// <returns>查询结果的JSON字符串</returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "GetAllNotice")]
        Stream GetAllNotice(DateTime[] Date);

        /// <summary>
        /// 审核通过申请
        /// </summary>
        /// <returns>查询结果的JSON字符串</returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "PassNotice")]
        Stream PassNotice(string NoticeID);

        /// <summary>
        /// 审核拒绝申请
        /// </summary>
        /// <returns>查询结果的JSON字符串</returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "RefuseNotice")]
        Stream RefuseNotice(string NoticeID , string Remarks);

        #region 设备绑定管理
        /// <summary>
        /// 绑定卡操作的接口
        /// </summary>
        /// <param name="VisitorID">游客ID</param>
        /// <returns>绑定结果</returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "BindingCard")]
        Stream BindingCard(string VisitorID);

        #endregion

    }
}
