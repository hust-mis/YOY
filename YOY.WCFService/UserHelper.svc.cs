using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using YOY.DAL;
using YOY.Model;

namespace YOY.WCFService
{
    /// <summary>
    /// 用户操作接口的实现
    /// </summary>
    public class UserHelper : IUserHelper
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <returns>成功返回添加的用户，失败返回Null</returns>
        public User AddUser(User user)
        {
            try
            {
                if (EFHelper.Add(user)) return user;
                else return null;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <returns>所有用户的列表</returns>
        public List<User> GetAllUsers()
        {
            return EFHelper.GetAll<User>();
        }

        /// <summary>
        /// 根据电话号码查询用户
        /// </summary>
        /// <param name="PhoneNumber">电话号码</param>
        /// <returns>成功返回用户，失败返回Null</returns>
        public User GetUserByPhoneNumber(string PhoneNumber)
        {
            var db = new EFDbContext();
            List<User> query = db.Users.Where(t => t.PhoneNumber == PhoneNumber).ToList();

            if (query.Count == 0) return null;

            return query[0];
        }
    }
}
