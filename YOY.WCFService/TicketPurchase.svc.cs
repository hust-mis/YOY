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
    }
}
