using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using YOY.BLL;
using YOY.DAL;
using YOY.Model;

namespace YOY.WCFService
{
    public class Amusement : IAmusement
    {
        public Stream Login(Visitor visitor)
        {
            if (visitor == null || string.IsNullOrEmpty(visitor.VisitorID) || string.IsNullOrEmpty(visitor.Password))
                return ResponseHelper.Failure("游客ID与密码缺失！");

            try
            {
                using (var db = new EFDbContext())
                {
                    var query = db.Visitors.Where(t => t.VisitorID == visitor.VisitorID);
                    if (query.Count() == 0) return ResponseHelper.Failure("游客不存在！");

                    Visitor result = query.Single();
                    if (result.Password != visitor.Password) return ResponseHelper.Failure("密码错误！");
                    result.Name = visitor.Name;
                    result.Age = visitor.Age;
                    result.Gender = visitor.Gender;
                    result.UID = visitor.UID;

                    db.SaveChanges();
                    return ResponseHelper.Success(new List<int>() { result.VisitorState });
                }
            }
            catch(Exception ex)
            {
                if (ex.InnerException == null)
                    return ResponseHelper.Failure(ex.Message);
                else
                    return ResponseHelper.Failure(ex.InnerException.Message);
            }
        }

        public Stream GetNotice(string LastGetTime)
        {
            try
            {
                var notices = EFHelper.GetAll<Notice>();
                if( string.IsNullOrEmpty(LastGetTime) )
                {
                    var query = notices.OrderByDescending(t => t.NoticeTime)
                                        .Take(5)
                                        .Select(t => new { t.NoticeID , t.NoticeTitle , NoticeTime = t.NoticeTime.ToString("yyyy-MM-dd HH:mm:ss") , t.ContentTime , t.NoticeAddress , t.NoticeInfo})
                                        .ToList();
                    return ResponseHelper.Success(query);
                }
                else
                {
                    var query = notices.Where(t => t.NoticeTime > Convert.ToDateTime(LastGetTime))
                                       .OrderByDescending(t => t.NoticeTime)
                                       .Select(t => new { t.NoticeID, t.NoticeTitle, NoticeTime = t.NoticeTime.ToString("yyyy-MM-dd HH:mm:ss"), t.ContentTime, t.NoticeAddress, t.NoticeInfo })
                                       .ToList();
                    return ResponseHelper.Success(query);
                }
            }
            catch(Exception ex)
            {
                if (ex.InnerException == null)
                    return ResponseHelper.Failure(ex.Message);
                else
                    return ResponseHelper.Failure(ex.InnerException.Message);
            }
        }

    }
}
