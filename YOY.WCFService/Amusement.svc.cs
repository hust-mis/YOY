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
                                        .Where(t => t.NoticeTime >= DateTime.Today)
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

        public Stream GetApplication(string VisitorID)
        {
            if (string.IsNullOrEmpty(VisitorID)) return ResponseHelper.Failure("游客ID不能为空！");

            try
            {
                var groups = EFHelper.GetAll<Group>();
                var visitors = EFHelper.GetAll<Visitor>();

                var query = from g in groups
                            where g.VisitorID == VisitorID && g.InviteeState == 0
                            join v in visitors on g.InviterID equals v.VisitorID
                            select new { g.InviterID, InviterName = v.Name };

                return ResponseHelper.Success(query.ToList());
            }
            catch(Exception ex)
            {
                if (ex.InnerException == null)
                    return ResponseHelper.Failure(ex.Message);
                else
                    return ResponseHelper.Failure(ex.InnerException.Message);
            }
        }

        public Stream Agree(string VisitorID, string InviterID)
        {
            if (string.IsNullOrEmpty(VisitorID) || string.IsNullOrEmpty(VisitorID))
                return ResponseHelper.Failure("VisitorID或InviterID信息不完全！");

            try
            {
                using (var db = new EFDbContext())
                {
                    var groups = db.Groups.Where(t => t.VisitorID == VisitorID );
                    if (groups.Count() == 0) return ResponseHelper.Failure("该游客没有邀请记录！");
                    if (groups.Where(t => t.InviterID == InviterID).Count() == 0)
                        return ResponseHelper.Failure("没有该条邀请记录！");
                    if (groups.Where(t => t.InviteeState == 1).Count() > 0)
                        return ResponseHelper.Failure("该游客已拥有队伍！");

                    var query = db.Groups.Where(t => t.VisitorID == VisitorID && t.InviterID == InviterID).Single();
                    query.InviteeState = 1;
                    db.SaveChanges();
                }

                return ResponseHelper.Success(null);
            }
            catch(Exception ex)
            {
                if (ex.InnerException == null)
                    return ResponseHelper.Failure(ex.Message);
                else
                    return ResponseHelper.Failure(ex.InnerException.Message);
            }
        }

        public Stream Refuse(string VisitorID, string InviterID)
        {
            if (string.IsNullOrEmpty(VisitorID) || string.IsNullOrEmpty(VisitorID))
                return ResponseHelper.Failure("VisitorID或InviterID信息不完全！");

            try
            {
                var groups = EFHelper.GetAll<Group>().Where(t => t.VisitorID == VisitorID);
                if (groups.Count() == 0) return ResponseHelper.Failure("该游客没有邀请记录！");

                var query = groups.Where(t => t.InviterID == InviterID);
                if (query.Count() == 0) return ResponseHelper.Failure("没有该条邀请记录！");
                EFHelper.Delete(query.Single());

                return ResponseHelper.Success(null);
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    return ResponseHelper.Failure(ex.Message);
                else
                    return ResponseHelper.Failure(ex.InnerException.Message);
            }
        }

        public Stream Invite(string InviterID, string InviteeID)
        {
            if (string.IsNullOrEmpty(InviterID) || string.IsNullOrEmpty(InviteeID))
                return ResponseHelper.Failure("InviterID或InviteeID信息不完全！");

            try
            {
                if (EFHelper.GetAll<Visitor>().Where(t => t.VisitorID == InviterID).Count() == 0)
                    return ResponseHelper.Failure("Inviter游客不存在！");
                if (EFHelper.GetAll<Visitor>().Where(t => t.VisitorID == InviteeID).Count() == 0)
                    return ResponseHelper.Failure("Invitee游客不存在！");

                using (var db = new EFDbContext())
                {
                    //判断邀请者是否已拥有队伍
                    var inviterGroup = db.Groups.Where(t => t.VisitorID == InviterID && t.InviteeState == 1);
                    //若没有，则新建队伍
                    if (inviterGroup.Count() == 0)
                    {
                        string groupID = IDHelper.getNextGroupID(DateTime.Now);
                        //自己邀请自己即为建立队伍
                        db.Groups.Add(new Group()
                        {
                            GroupID = groupID,
                            VisitorID = InviterID,
                            InviteeState = 1,
                            InviterID = InviterID
                        });
                        db.Groups.Add(new Group()
                        {
                            GroupID = groupID,
                            VisitorID = InviteeID,
                            InviteeState = 0,
                            InviterID = InviterID
                        });

                    }
                    else  //否则以邀请者队伍进行邀请
                    {
                        var group = inviterGroup.Single();
                        db.Groups.Add(new Group()
                        {
                            GroupID = group.GroupID,
                            VisitorID = InviteeID,
                            InviteeState = 0 ,
                            InviterID = InviterID
                        });
                    }

                    db.SaveChanges();
                    return ResponseHelper.Success(null);
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

        public Stream SearchVisitor(string SearchKey)
        {
            if (string.IsNullOrEmpty(SearchKey) )
                return ResponseHelper.Failure("关键字信息不完全！");

            try
            {
                var visitors = EFHelper.GetAll<Visitor>();
                var query = visitors.Where(t => t.Name == SearchKey);
                if (query.Count() > 0) return ResponseHelper.Success(query.Select(t => new { t.VisitorID, t.Name }).ToList());
                query = visitors.Where(t => t.VisitorID == SearchKey);
                return ResponseHelper.Success(query.Select(t => new { t.VisitorID, t.Name }).ToList());
            }
            catch(Exception ex)
            {
                if (ex.InnerException == null)
                    return ResponseHelper.Failure(ex.Message);
                else
                    return ResponseHelper.Failure(ex.InnerException.Message);
            }
        }

        public Stream VisitorID(string VisitorID)
        {
            if (string.IsNullOrEmpty(VisitorID))
                return ResponseHelper.Failure("游客ID不能为空！");

            try
            {
                var group = EFHelper.GetAll<Group>().Where(t => t.VisitorID == VisitorID && t.InviteeState == 1);
                if (group.Count() == 0) return ResponseHelper.Failure("没有该游客的组队信息！");
                EFHelper.Delete(group.Single());
                return ResponseHelper.Success(null);
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
