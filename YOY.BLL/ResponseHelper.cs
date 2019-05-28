using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YOY.Model;

namespace YOY.BLL
{
    /// <summary>
    /// 返回结果的封装类
    /// </summary>
    public sealed class ResponseHelper
    {
        /// <summary>
        /// 成功时返回给客户端的信息
        /// </summary>
        /// <param name="list">成果时返回的结果集</param>
        /// <returns>返回给客户端的Stream流</returns>
        public static Stream Success(IList list)
        {
            Response response = new Response()
            {
                code = 1,
                errMsg = string.Empty,
                result = list 
            };
            string json = JsonConvert.SerializeObject(response);
            return new MemoryStream(Encoding.UTF8.GetBytes(json));
        }

        /// <summary>
        /// 成功时返回给客户端的信息
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="obj">单个信息实体</param>
        /// <returns>返回给客户端的Stream流</returns>
        public static Stream Success<T>(T obj)
        {
            Response response = new Response()
            {
                code = 1,
                errMsg = string.Empty,
                result = new List<T> { obj }
            };
            string json = JsonConvert.SerializeObject(response);
            return new MemoryStream(Encoding.UTF8.GetBytes(json));
        }

        /// <summary>
        /// 失败时返回给客户端的信息
        /// </summary>
        /// <param name="error">失败时的错误信息</param>
        /// <returns>返回给客户端的Stream流</returns>
        public static Stream Failure(string error)
        {
            Response response = new Response()
            {
                code = 0,
                errMsg = error,
                result = null
            };
            string json = JsonConvert.SerializeObject(response);
            return new MemoryStream(Encoding.UTF8.GetBytes(json));
        }
    }
}
