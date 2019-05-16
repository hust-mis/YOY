using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using YOY.DAL;
using YOY.Model;
using Newtonsoft.Json;
using System.IO;
using YOY.BLL;

namespace YOY.WCFService
{
    /// <summary>
    /// 购票系统操作接口的实现
    /// </summary>
    public class TicketPurchase : ITicketPurchase
    {
        public Stream GetTickets()
        {
            try
            {
                return ResponseHelper.Success(EFHelper.GetAll<Ticket>());
            }
            catch(Exception ex)
            {
                if (ex.InnerException == null)
                    return ResponseHelper.Failure(ex.Message);
                else 
                    return ResponseHelper.Failure(ex.InnerException.Message);
            }
        }

        public Stream BuyTickets(User user, string doneTime, List<Order> orders)
        {

            #region 用户不存在则添加
            using (var db = new EFDbContext())
            {
                if (db.Users.Where(t => t.PhoneNumber == user.PhoneNumber).ToList().Count == 0)
                {
                    try
                    {
                        if (!EFHelper.Add(user))
                            return ResponseHelper.Failure("新增用户错误");
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException == null)
                            return ResponseHelper.Failure(ex.Message);
                        else
                            return ResponseHelper.Failure(ex.InnerException.Message);
                    }
                }
            }
            #endregion

            #region 新增订单数据，每张票唯一对应一张订单
            string id = IDHelper.getNextOrderID(DateTime.Now, 0);
            List<Order> ticketOrders = new List<Order>();
            foreach ( Order item in orders)
            {
                for( int i = 1; i <= item.CommodityNum; i++)
                {
                    ticketOrders.Add(new Order()
                    {
                        OrderID = id,
                        OrderTime = DateTime.Now,
                        OrderState = 0,
                        CommodityID = item.CommodityID,
                        CommodityType = 0,
                        CommodityNum = 1,
                        DoneTime = Convert.ToDateTime(doneTime)
                    });
                    id = IDHelper.get4Next(id);
                }
            }
            #endregion

            #region 添加用户映射并生成游客
            List<User2Order> user2Orders = new List<User2Order>();
            List<Visitor> visitors = new List<Visitor>();
            id = IDHelper.getNextVisitorID(Convert.ToDateTime(doneTime));
            foreach( Order order in ticketOrders)
            {
                //生成对应的游客ID，默认密码为用户身份证后6位
                visitors.Add(new Visitor()
                {
                    VisitorID = id,
                    Password = user.UID.Substring(user.UID.Length - 6),
                    VisitorState = 0,
                    PlayTime = Convert.ToDateTime(doneTime)
                });
                user2Orders.Add(new User2Order()
                {
                    PhoneNumber = user.PhoneNumber,
                    OrderID = order.OrderID,
                    VisitorID = id
                });

                id = IDHelper.get4Next(id);
            }
            #endregion

            #region 提交数据库
            try
            {
                using (var db = new EFDbContext())
                {
                    for( int i = 0; i < ticketOrders.Count; i++ )
                    {
                        db.Orders.Add(ticketOrders[i]);
                        db.Visitors.Add(visitors[i]);
                        db.User2Orders.Add(user2Orders[i]);
                    }
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                if (ex.InnerException == null)
                    return ResponseHelper.Failure(ex.Message);
                else
                    return ResponseHelper.Failure(ex.InnerException.Message);
            }
            #endregion

            return ResponseHelper.Success(ticketOrders.Select( t => t.OrderID).ToList());
        }

        public Stream Pay(List<Payment> payments)
        {
            if (payments == null || payments.Count == 0)
                return ResponseHelper.Failure("支付列表为空！");

            foreach( Payment payment in payments)
                payment.PaymentTime = DateTime.Now;

            try
            {
                using (var db = new EFDbContext())
                {
                    foreach (Payment payment in payments)
                    {
                        //同时更改订单状态
                        var query = db.Orders.Where(t => t.OrderID == payment.OrderID).Single();
                        query.OrderState = 1;
                        db.Payments.Add(payment);
                    }
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                if (ex.InnerException == null)
                    return ResponseHelper.Failure(ex.Message);
                else
                    return ResponseHelper.Failure(ex.InnerException.Message);
            }

            return ResponseHelper.Success(null);
        }

        public Stream Login(User user)
        {
            if (user == null || string.IsNullOrEmpty(user.PhoneNumber) || string.IsNullOrEmpty(user.UID))
                return ResponseHelper.Failure("用户信息不完全！");

            try
            {
                using (var db = new EFDbContext())
                {
                    var query = db.Users.Where(t => t.PhoneNumber == user.PhoneNumber).Single();
                    if (query == null) return ResponseHelper.Failure("用户不存在！");
                    if (query.UID != user.UID) return ResponseHelper.Failure("身份证号码错误！");
                    return ResponseHelper.Success(new List<User>() { query });
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

        public Stream AvailTickets(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return ResponseHelper.Failure("手机号码不能为空！");

            try
            {
                using(var db = new EFDbContext())
                {
                    var U2O2V = from u2o in db.User2Orders
                                where u2o.PhoneNumber == phoneNumber
                                join v in db.Visitors on u2o.VisitorID equals v.VisitorID
                                where v.VisitorState == 0
                                select new { u2o.OrderID, u2o.VisitorID, v.Password, v.PlayTime };

                    var query = from o in db.Orders
                                join t in db.Tickets on o.CommodityID equals t.TicketID
                                join u2o2v in U2O2V on o.OrderID equals u2o2v.OrderID
                                select new { u2o2v.OrderID, t.TicketName, u2o2v.VisitorID, u2o2v.Password, u2o2v.PlayTime };

                    var result = from q in query.ToList()
                                 select new { q.OrderID, q.TicketName, q.VisitorID, q.Password, PlayTime = q.PlayTime.ToString("yyyy-MM-dd") };

                    if (query == null) return ResponseHelper.Success(null);
                    else return ResponseHelper.Success(result.ToList());
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

        public Stream UnavailTickets(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return ResponseHelper.Failure("手机号码不能为空！");

            try
            {
                using (var db = new EFDbContext())
                {
                    var U2O2V = from u2o in db.User2Orders
                                where u2o.PhoneNumber == phoneNumber
                                join v in db.Visitors on u2o.VisitorID equals v.VisitorID
                                where v.VisitorState == 1 || v.VisitorState == 2
                                select new { u2o.OrderID, u2o.VisitorID, v.Password, v.PlayTime };

                    var query = from o in db.Orders
                                join t in db.Tickets on o.CommodityID equals t.TicketID
                                join u2o2v in U2O2V on o.OrderID equals u2o2v.OrderID
                                select new { u2o2v.OrderID, t.TicketName, u2o2v.VisitorID, u2o2v.Password, u2o2v.PlayTime };

                    var result = from q in query.ToList()
                                 select new { q.OrderID, q.TicketName, q.VisitorID, q.Password, PlayTime = q.PlayTime.ToString("yyyy-MM-dd") };

                    if (query == null) return ResponseHelper.Success(null);
                    else return ResponseHelper.Success(result.ToList());
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    return ResponseHelper.Failure(ex.Message);
                else
                    return ResponseHelper.Failure(ex.InnerException.Message);
            }
        }

        public Stream Refund(string orderID)
        {
            if (string.IsNullOrEmpty(orderID))
                return ResponseHelper.Failure("订单编号不能为空！");

            try
            {
                using (var db = new EFDbContext())
                {
                    var order = db.Orders.Where(t => t.OrderID == orderID).Single();
                    if( order == null ) return ResponseHelper.Failure("该订单不存在！");
                    var u2o = db.User2Orders.Where(t => t.OrderID == orderID).Single();
                    var visitor = db.Visitors.Where(t => t.VisitorID == u2o.VisitorID).Single();

                    //修改数据库中相关记录
                    order.OrderState = -1;
                    u2o.VisitorID = string.Empty;
                    db.Visitors.Remove(visitor);
                    db.SaveChanges();

                    var query = db.Payments.Where(t => t.OrderID == orderID).Select(t => new { t.PaymentAmount, t.PaymentType }).ToList();
                    return ResponseHelper.Success(query);

                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    return ResponseHelper.Failure(ex.Message);
                else
                    return ResponseHelper.Failure(ex.InnerException.Message);
            }
        }
    }
}
