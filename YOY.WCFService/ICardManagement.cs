using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using YOY.Model.DB;

namespace YOY.WCFService
{
    /// <summary>
    /// 卡管理接口声明
    /// </summary>
    [ServiceContract]
    public interface ICardManagement
    {
        /// <summary>
        /// 卡充值
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "Recharge")]
        Stream Recharge(string VisitorID, float Amount, int PaymentType);

        /// <summary>
        /// 卡退款
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "Refund")]
        Stream Refund(string VisitorID, float Amount, int PaymentType);

        /// <summary>
        /// 查询卡余额
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "GetBalance")]
        Stream GetBalance(string VisitorID);

        
        /// <summary>
        /// 获取卡充值、退款记录
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "CardRecord")]
        Stream CardRecord(string VisitorID);


        


    }
}
