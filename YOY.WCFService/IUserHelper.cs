using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using YOY.Model;

namespace YOY.WCFService
{
    /// <summary>
    /// 用户信息操作接口
    /// </summary>
    [ServiceContract]
    public interface IUserHelper
    {
        /// <summary>
        /// 根据电话号码查询用户
        /// </summary>
        /// <param name="PhoneNumber">电话号码</param>
        /// <returns>成功返回用户，失败返回Null</returns>
        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "GetUserByPhoneNumber/PhoneNumber={PhoneNumber}")]
        User GetUserByPhoneNumber(string PhoneNumber);

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <returns>所有用户的列表</returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "GetAllUsers")]
        List<User> GetAllUsers();

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <returns>成功返回添加的用户，失败返回Null</returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "AddUser")]
        User AddUser(User user);
    }
}
