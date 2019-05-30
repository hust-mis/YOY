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
        /// 将要绑定的卡放在指定位置
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

        /// <summary>
        /// 解绑卡
        /// 将要解绑的卡放在指定位置
        /// </summary>
        /// <returns>解绑结果</returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "UnbindingCard")]
        Stream UnbindingCard();

        /// <summary>
        /// 绑定LocalSense
        /// </summary>
        /// <param name="VisitorID">游客ID</param>
        /// <param name="LocatorID">LocalSense定位设备的ID</param>
        /// <returns>绑定结果</returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "BindingLocators")]
        Stream BindingLocators(string VisitorID, string LocatorID);

        /// <summary>
        /// 取消定位设备绑定
        /// </summary>
        /// <param name="LocatorID">LocalSense定位设备的ID</param>
        /// <returns>绑定结果</returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "UnbindingLocators")]
        Stream UnbindingLocators(string LocatorID);

        #endregion

    }
}
