﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using YOY.Model;

namespace YOY.WCFService
{
    /// <summary>
    /// 游园子系统卡管理接口声明
    /// </summary>
    [ServiceContract]
    public interface ICardManagement
    {
        /// <summary>
        /// 测试接口
        /// </summary>
        /// <returns>卡绑定的用户id</returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "GetCardUser")]
        string GetCardUser();
    }
}