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
    }
}
