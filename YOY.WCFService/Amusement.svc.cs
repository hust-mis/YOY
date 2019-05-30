using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using YOY.BLL;
using YOY.DAL;
using YOY.Model.DB;
using YOY.Model;

namespace YOY.WCFService
{
    public class Amusement : IAmusement
    {

        #region 基本功能
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
                    if ( DateTime.Now < result.PlayTime ) return ResponseHelper.Failure("未到游玩时间！");
                    if( DateTime.Now > result.PlayTime.AddDays(1) ) return ResponseHelper.Failure("已超过游玩时间！");

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
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                return ResponseHelper.Failure(ex.Message);
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
                                        .Where(t => t.NoticeStatus == 1)
                                        .Select(t => new { t.NoticeID , t.NoticeType ,t.OccurTime , t.OccurAddress , t.NoticeDetail, CheckTime = t.CheckTime?.ToString("yyyy-MM-dd HH:mm:ss") })
                                        .ToList();
                    return ResponseHelper.Success(query);
                }
                else
                {
                    var query = notices.Where(t => t.NoticeTime > Convert.ToDateTime(LastGetTime) && t.NoticeStatus == 1)
                                       .OrderByDescending(t => t.NoticeTime)
                                       .Select(t => new { t.NoticeID, t.NoticeType, t.OccurTime, t.OccurAddress, t.NoticeDetail, CheckTime = t.CheckTime?.ToString("yyyy-MM-dd HH:mm:ss") })
                                       .ToList();
                    return ResponseHelper.Success(query);
                }
            }
            catch(Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                return ResponseHelper.Failure(ex.Message);
            }
        }
        #endregion

        #region 队伍管理
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
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                return ResponseHelper.Failure(ex.Message);
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
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                return ResponseHelper.Failure(ex.Message);
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
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                return ResponseHelper.Failure(ex.Message);
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
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                return ResponseHelper.Failure(ex.Message);
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
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                return ResponseHelper.Failure(ex.Message);
            }
        }

        public Stream Exit(string VisitorID)
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
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                return ResponseHelper.Failure(ex.Message);
            }
        }

        public Stream GetTeamerInfo(string VisitorID)
        {
            if (string.IsNullOrEmpty(VisitorID))
                return ResponseHelper.Failure("游客ID不能为空！");

            try
            {
                var group = EFHelper.GetAll<Group>().Where(t => t.VisitorID == VisitorID && t.InviteeState == 1);
                if (group.Count() == 0) return ResponseHelper.Success(null);

                var groups = EFHelper.GetAll<Group>().Where(t => t.GroupID == group.Single().GroupID && t.InviteeState == 1);

                var visitors = from g in groups
                               join v in EFHelper.GetAll<Visitor>() on g.VisitorID equals v.VisitorID
                               select new { g.GroupID, v.VisitorID, v.Name };

                var operations = EFHelper.GetAll<Operation>();
                List<Operation> status = new List<Operation>();
                foreach( var g in groups )
                {
                    var operation = operations.Where(t => t.VisitorID == g.VisitorID);
                    if (operation.Count() == 0)
                        status.Add(new Operation()
                        {
                            ProjectID = null,
                            PlayState = -1,
                            VisitorID = g.VisitorID
                        });
                    else
                        status.Add(operation.Single());
                }

                var v2p = from s in status
                          join p in EFHelper.GetAll<Project>() on s.ProjectID equals p.ProjectID into visitorStauts
                          from vs in visitorStauts.DefaultIfEmpty()
                          select new { s.VisitorID, s.PlayState, s.ProjectID, vs?.ProjectName };

                var query = from v in visitors
                            join p in v2p on v.VisitorID equals p.VisitorID
                            select new
                            {
                                v.GroupID,
                                TeamerID = v.VisitorID,
                                TeamerName = v.Name,
                                TeamerStatus = p.PlayState,
                                p.ProjectID,
                                p.ProjectName
                            };

                return ResponseHelper.Success(query.ToList());
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                return ResponseHelper.Failure(ex.Message);
            }
        }

        public Stream GetLocation(string VisitorID)
        {
            if (string.IsNullOrEmpty(VisitorID))
                return ResponseHelper.Failure("游客ID不能为空！");

            try
            {
                var locators = EFHelper.GetAll<Locator>().Where(t => t.VisitorID == VisitorID && t.LocatorState == 1);
                if (locators.Count() == 0) return ResponseHelper.Failure("该游客没有定位信息！");

                var localsense = new LocalSense();
                localsense.Run();
                Thread.Sleep(100);
                new Thread(() => { localsense.Stop(); }).Start();

                var locator = locators.Single();
                var query = localsense.locations
                            .Where(t => t.ID == locator.LocatorID)
                            .OrderByDescending(t => t.Timestamp)
                            .Select(t => new { t.X, t.Y }).First();

                if (query == null) return ResponseHelper.Failure("没有查询到位置信息！");
                else return ResponseHelper.Success( new List<dynamic> { query });
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                return ResponseHelper.Failure(ex.Message);
            }
        }

        public Stream GroupLocation(string GroupID)
        {
            if (string.IsNullOrEmpty(GroupID))
                return ResponseHelper.Failure("队伍ID不能为空！");

            try
            {
                var visitors = EFHelper.GetAll<Visitor>().ToList();
                var groups = EFHelper.GetAll<Group>().ToList();
                var locators = EFHelper.GetAll<Locator>().ToList();
                var team = from g in groups
                           where g.GroupID == GroupID && g.InviteeState == 1
                           join v in visitors on g.VisitorID equals v.VisitorID
                           join l in locators on v.VisitorID equals l.VisitorID
                           where l.LocatorState == 1
                           select new { v.VisitorID, v.Name, l.LocatorID };

                if (team.Count() == 0) return ResponseHelper.Failure("该队伍没有定位信息！");

                var localsense = new LocalSense();
                localsense.Run();
                Thread.Sleep(500);
                new Thread(() => { localsense.Stop(); }).Start();

                var location = new List<Location>();
                foreach( var t in team )
                    location.Add(localsense.locations.Where(q => q.ID == t.LocatorID).OrderByDescending(q => q.Timestamp).FirstOrDefault());

                var query = from t in team
                            join l in location on t.LocatorID equals l.ID
                            orderby t.VisitorID
                            select new { t.VisitorID, t.Name, l.X, l.Y };

                if (query.Count() == 0) return ResponseHelper.Failure("没有查询到位置信息！");
                else return ResponseHelper.Success(query.ToList());
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                return ResponseHelper.Failure(ex.Message);
            }
        }
        #endregion

        #region 订单查询
        public Stream AvailCommodity(string VisitorID)
        {
            if (string.IsNullOrEmpty(VisitorID))
                return ResponseHelper.Failure("游客ID不能为空！");

            try
            {
                var v2os = EFHelper.GetAll<Visitor2Order>().Where(t => t.VisitorID == VisitorID).ToList();
                if (v2os.Count() == 0) return ResponseHelper.Success(null);

                var orders = EFHelper.GetAll<Order>().ToList();
                var commodies = EFHelper.GetAll<Commodity>().ToList();

                var query = from v2o in v2os
                            join o in orders on v2o.OrderID equals o.OrderID
                            where o.CommodityType == 2 && o.DoneTime == null
                            join c in commodies on o.CommodityID equals c.CommodityID
                            select new
                            {
                                o.OrderID,
                                o.CommodityID,
                                o.CommodityNum,
                                c.CommodityName,
                                CommodityPrice = c.CommodityPrice * o.CommodityNum,
                                c.CommodityInfo,
                                c.CommodityPic
                            };

                return ResponseHelper.Success(query.ToList());
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                return ResponseHelper.Failure(ex.Message);
            }
        }

        public Stream UnavailCommodity(string VisitorID)
        {
            if (string.IsNullOrEmpty(VisitorID))
                return ResponseHelper.Failure("游客ID不能为空！");

            try
            {
                var v2os = EFHelper.GetAll<Visitor2Order>().Where(t => t.VisitorID == VisitorID).ToList();
                if (v2os.Count() == 0) return ResponseHelper.Success(null);

                var orders = EFHelper.GetAll<Order>().ToList();
                var commodies = EFHelper.GetAll<Commodity>().ToList();

                var query = from v2o in v2os
                            join o in orders on v2o.OrderID equals o.OrderID
                            where o.CommodityType == 2 && o.DoneTime != null
                            join c in commodies on o.CommodityID equals c.CommodityID
                            select new
                            {
                                o.OrderID,
                                o.CommodityID,
                                o.CommodityNum,
                                c.CommodityName,
                                CommodityPrice = c.CommodityPrice * o.CommodityNum,
                                c.CommodityInfo,
                                c.CommodityPic
                            };

                return ResponseHelper.Success(query.ToList());
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                return ResponseHelper.Failure(ex.Message);
            }
        }

        public Stream ReturnCommodity(Order order)
        {
            if (string.IsNullOrEmpty(order.OrderID))
                return ResponseHelper.Failure("订单ID不能为空！");
            if (string.IsNullOrEmpty(order.CommodityID) )
                return ResponseHelper.Failure("退货商品信息不全！");

            try
            {
                using (var db = new EFDbContext())
                {
                    var orders = db.Orders.Where(t => t.OrderID == order.OrderID);
                    if (orders.Count() == 0) return ResponseHelper.Failure("该订单不存在！");
                    var v2o = db.Visitor2Orders.First(t => t.OrderID == order.OrderID);
                    var v2c = db.Visitor2Cards.First(t => t.VisitorID == v2o.VisitorID);
                    var query = db.Payments.Where(t => t.OrderID == order.OrderID).Select(t => new { t.PaymentAmount, t.PaymentType }).ToList();
                    //修改数据库中相关记录
                    orders.Single().OrderState = -1;
                    v2c.Balance += query.Single().PaymentAmount;

                    db.SaveChanges();
                    return ResponseHelper.Success(query);

                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                return ResponseHelper.Failure(ex.Message);
            }
        }
        #endregion

    }
}
