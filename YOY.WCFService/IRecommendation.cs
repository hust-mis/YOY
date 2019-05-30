using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using YOY.Model.DB;

namespace YOY.WCFService
{
    /// <summary>
    /// 游园子系统推荐接口声明
    /// </summary>
    [ServiceContract]
    public interface IRecommendation
    {
        /// <summary>
        /// 线路推荐接口
        /// </summary>
        /// <param name="RouteID">路径ID</param>
        /// <returns>路径中各项目信息</returns>
        /// <returns>查询结果的JSON字符串</returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "GetRoute/{RouteID}")]
        Stream GetRoute(string RouteID);

        /// <summary>
        /// 项目推荐接口
        /// </summary>
        /// <param name="RecomType">推荐项目依据1：距离；2：等待时间</param>
        /// <param name="VisitorID">游客ID</param>
        /// <returns>查询结果的JSON字符串</returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "ProjectRecom")]
        Stream ProjectRecom(int RecomType, string VisitorID);

        /// <summary>
        /// 获取店铺信息接口
        /// </summary>
        /// <param name="VisitorID">游客ID</param>
        /// <returns>查询结果的JSON字符串</returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "GetStoreInfo")]
        Stream GetStoreInfo();

        /// <summary>
        /// 获取商品菜单接口
        /// </summary>
        /// <returns>查询结果的JSON字符串</returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "GetMenu/{StoreID}")]
        Stream GetMenu(string StoreID);

        /// <summary>
        /// 商品购买接口
        /// </summary>
        /// <returns>查询结果的JSON字符串</returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "BuyCommodity")]
        Stream BuyCommodity(string VisitorID , List<Order> orders, int PaymentType);

       
    }
}
