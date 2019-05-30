using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading;
using YOY.BLL;
using YOY.DAL;
using YOY.Model.DB;

namespace YOY.WCFService
{
    
    /// <summary>
    /// 游园子系统推荐接口的实现
    /// </summary>
    public class Recommendation : IRecommendation
    {
        
        public Stream GetRoute(string RouteID)//路线推荐（目前写死在前端）
        {
            List<Project> Route = new List<Project>{ };
            return ResponseHelper.Success(Route);
        }

        public Stream ProjectRecom(int RecomType, string VisitorID)//项目推荐
        {
            if (RecomType != 1 && RecomType != 2)
                return ResponseHelper.Failure("推荐类型不存在！");

            

            #region 按距离推荐（距离升序）
            if (RecomType == 1)
            {
                #region 查询游客当前位置
                var locators = EFHelper.GetAll<Locator>().Where(t => t.VisitorID == VisitorID && t.LocatorState == 1);
                if (locators.Count() == 0) return ResponseHelper.Failure("该游客没有定位信息！");

                var localsense = new LocalSense();
                localsense.Run();
                Thread.Sleep(100);
                new Thread(() => { localsense.Stop(); }).Start();

                var locator = locators.Single();
                var q = localsense.locations
                            .Where(t => t.ID == locator.LocatorID)
                            .OrderByDescending(t => t.Timestamp)
                            .Select(t => new { t.X, t.Y }).First();

                if (q == null) return ResponseHelper.Failure("没有查询到位置信息！");
                int vx = int.Parse(q.X);
                int vy = int.Parse(q.Y);
                #endregion
                try
                {
                    using (var db = new EFDbContext())
                    {

                        var query = from p in db.Projects
                                    join r in db.ProjectRecord on p.ProjectID equals r.ProjectID
                                    select new { p.ProjectID, p.ProjectName, p.ProjectState, p.ProjectInfo, p.ProjectPic, p.OpeningTime, p.ProjectAttention, p.ProjectForPeople, Record = Math.Sqrt((p.ProjectXLocation - vx) * (p.ProjectXLocation - vx) + (p.ProjectYLocation - vy) * (p.ProjectYLocation - vy)) };

                        var result = from qu in query
                                     orderby qu.Record
                                     select qu;

                        if (result.Count() == 0)
                            return ResponseHelper.Success(null);

                        return ResponseHelper.Success(result.ToList());
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
            #endregion

            #region 按等待人数推荐（等待人数升序）
            else
            {
                try
                {
                    using (var db = new EFDbContext())
                    {
                        var wait = db.ProjectOperation.Where(o => o.PlayState == 0);
                        var query = from p in db.Projects
                                    join w in wait on p.ProjectID equals w.ProjectID
                                    select new { p.ProjectID, p.ProjectName, p.ProjectState, p.ProjectInfo, p.ProjectPic, p.OpeningTime, p.ProjectAttention, p.ProjectForPeople, Record = wait.Count() };

                        var result = from q in query
                                     orderby q.Record
                                     select q;

                        if (result.Count() == 0)
                            return ResponseHelper.Success(null);
                        return ResponseHelper.Success(result.ToList());
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
            #endregion




        }

        public Stream GetStoreInfo(string VisitorID)//获取店铺信息
        {

            #region 查询游客当前位置
            var locators = EFHelper.GetAll<Locator>().Where(t => t.VisitorID == VisitorID && t.LocatorState == 1);
            if (locators.Count() == 0) return ResponseHelper.Failure("该游客没有定位信息！");

            var localsense = new LocalSense();
            localsense.Run();
            Thread.Sleep(100);
            new Thread(() => { localsense.Stop(); }).Start();

            var locator = locators.Single();
            var q = localsense.locations
                        .Where(t => t.ID == locator.LocatorID)
                        .OrderByDescending(t => t.Timestamp)
                        .Select(t => new { t.X, t.Y }).First();
            
            if (q == null) return ResponseHelper.Failure("没有查询到位置信息！");
            int vx = int.Parse(q.X);
            int vy = int.Parse(q.Y);
            #endregion

            try
            {
                using (var db = new EFDbContext())
                {

                    var query = from s in EFHelper.GetAll<Store>()
                                select new { s.StoreID, s.StoreName, s.StorePic, s.StoreAddress, s.StoreInfo, Distance = Math.Sqrt((s.StoreXLocation - vx) * (s.StoreXLocation - vx) + (s.StoreYLocation - vy) * (s.StoreYLocation - vy)) };

                    var result = from qu in query
                                 orderby qu.Distance
                                 select qu;

                    if (result.Count() == 0)
                        return ResponseHelper.Success(null);

                    return ResponseHelper.Success(result.ToList());
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

        public Stream GetMenu(string StoreID)//获取店铺菜单
        {
            try
            {
                using (var db = new EFDbContext())
                {

                    var result = from c in EFHelper.GetAll<Commodity>()
                                where c.StoreID == StoreID
                                select c;
                    if (result.Count() == 0)
                        return ResponseHelper.Success(null);

                    return ResponseHelper.Success(result.ToList());
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
        public Stream BuyCommodity(string VisitorID, List<Order> orders, int PaymentType)//购买商品
        {
            #region 数据完整性检查
            if (VisitorID == null)
                return ResponseHelper.Failure("游客信息缺失！");
            if (PaymentType != 4)
                return ResponseHelper.Failure("目前仅支持卡支付！");
            if (orders == null || orders.Count == 0)
                return ResponseHelper.Failure("订单信息不完全！");
            #endregion

            #region 根据游客ID查询卡ID
            string CardID = "";
            try
            {
                using (var db = new EFDbContext())
                {

                    var card = from v2c in db.Visitor2Cards
                               where v2c.VisitorID == VisitorID
                               select v2c;

                    if (card.Count() == 0)
                        return ResponseHelper.Failure("该游客没有绑定游园卡！");

                    CardID = card.Single().CardID;//记录卡ID
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    return ResponseHelper.Failure(ex.Message);
                else
                    return ResponseHelper.Failure(ex.InnerException.Message);
            }
            #endregion

            #region 新增订单、支付、游客订单映射信息并提交数据库
            string id = IDHelper.getNextOrderID(DateTime.Now, 2);
            List<Order> GoodsOrders = new List<Order>();
            List<Payment> GoodsPays = new List<Payment>();
            List<Visitor2Order> v2o = new List<Visitor2Order>();
            List<Commodity> commodities = EFHelper.GetAll<Commodity>();

            foreach (Order order in orders)
            {
                GoodsOrders.Add(new Order()
                {
                    OrderID = id,
                    OrderTime = DateTime.Now,
                    OrderState = 1,
                    CommodityID = order.CommodityID,
                    CommodityType = 2,
                    CommodityNum = order.CommodityNum,
                    DoneTime = Convert.ToDateTime(DateTime.Now)
                });

                GoodsPays.Add(new Payment()
                {
                    OrderID = id,
                    PaymentType = PaymentType,
                    PaymentTime = DateTime.Now,
                    PaymentAmount = order.CommodityNum * (commodities.Where(c => c.CommodityID == order.CommodityID).Single().CommodityPrice)
                });

                v2o.Add(new Visitor2Order()
                {
                    VisitorID = VisitorID,
                    OrderID = id,
                    CardID = CardID

                });

                id = IDHelper.get4Next(id);


            }
            #endregion
            
            #region 完成扣款，订单、支付、游客订单映射信息提交数据库
            //计算订单总价,查询余额并判断，完成扣款
            float sum = 0;
            foreach (Payment item in GoodsPays)
            {
                sum += (float)item.PaymentAmount;
            }
            try
            {
                using (var db = new EFDbContext())
                {
                    //数据库余额修改
                    Visitor2Card new_v2c = db.Visitor2Cards.Where(v => v.VisitorID == VisitorID).Single();
                    if (new_v2c.Balance < sum)
                        return ResponseHelper.Failure("余额不足！");
                    new_v2c.Balance -= sum;//完成扣款
                    
                    //订单、支付、游客订单映射信息——提交数据库
                    for (int i = 0; i < GoodsOrders.Count; i++)
                    {
                        
                        db.Orders.Add(GoodsOrders[i]);
                        db.Payments.Add(GoodsPays[i]);
                        db.Visitor2Orders.Add(v2o[i]);
                    }

                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    return ResponseHelper.Failure(ex.Message);
                else
                    return ResponseHelper.Failure(ex.InnerException.Message);
            }
            
            #endregion

            return ResponseHelper.Success(GoodsOrders.Select(t => t.OrderID).ToList());//返回OrderID
        }

    }
}
