using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using YOY.Model.DB;

namespace YOY.WCFService
{
    /// <summary>
    /// 购票子系统的操作接口
    /// </summary>
    [ServiceContract]
    public interface ITicketPurchase
    {
        /// <summary>
        /// 获取所有的门票信息
        /// </summary>
        /// <returns>查询结果的JSON字符串</returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "GetTickets")]
        Stream GetTickets();

        /// <summary>
        /// 购票接口
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="DoneTime">消费完成时间</param>
        /// <param name="orders">订单信息</param>
        /// <param name="PaymentType">支付方式</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "BuyTickets")]
        Stream BuyTickets(User user , string DoneTime , List<Order> orders, int PaymentType );

        /// <summary>
        /// 用户登录接口
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>登录结果的JSON</returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "Login")]
        Stream Login(User user);

        /// <summary>
        /// 未出行票查询接口
        /// </summary>
        /// <param name="phoneNumber">用户手机号码</param>
        /// <returns>门票相关结果</returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "AvailTickets/{phoneNumber}")]
        Stream AvailTickets(string phoneNumber);

        /// <summary>
        /// 已出行票查询接口
        /// </summary>
        /// <param name="phoneNumber">用户手机号码</param>
        /// <returns>门票相关结果</returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "UnavailTickets/{phoneNumber}")]
        Stream UnavailTickets(string phoneNumber);

        /// <summary>
        /// 退票接口
        /// </summary>
        /// <param name="OrderID">需退票的门票对应的订单编号</param>
        /// <returns>退票结果</returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "Refund/{orderID}")]
        Stream Refund(string orderID);
    }
}
