using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using YOY.Model;

namespace YOY.WCFService
{
    /// <summary>
    /// 购票系统的操作接口
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
        /// 购买门票
        /// </summary>
        /// <param name="TicketID">门票ID</param>
        /// <param name="CommodityNum">门票数量</param>
        /// <param name="PhoneNumber">手机号码</param>
        /// <param name="UID">身份证号码</param>
        /// <param name="Name">姓名</param>
        /// <param name="DoneTime">消费时间（YYYY-MM-DD）</param>
        /// <returns>订单编号结果的JSON字符串</returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "BuyTickets")]
        Stream BuyTickets(User user , string DoneTime , List<Order> orders );

        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="OrderID">每张门票对应的订单编号</param>
        /// <param name="PaymentAmount">支付的金额</param>
        /// <param name="PaymentType">支付方式</param>
        /// <returns>支付结果的JSON字符串</returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "Pay")]
        Stream Pay(List<Payment> payments);
    }
}
