using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
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

        //public Stream ProjectRecom(int RecomType, string VisitorID)//项目推荐
        //{
        //    if (RecomType!=1 && RecomType!=2)
        //        return ResponseHelper.Failure("推荐类型不存在！");

        //    #region 获取游客当前位置坐标并记录到vx，vy(暂时设置为（0，0）
        //    //LocationRecords vlr = GetVisitorLocation(VisitorID);
        //    int vx = 0;
        //    int vy = 0;
        //    DateTime time = DateTime.Now;

        //    LocationRecords vlr = new LocationRecords();
        //    vlr.VisitorID = VisitorID;
        //    vlr.XLocation = vx;
        //    vlr.YLocation = vy;
        //    vlr.LocationTime = time;
        //    vlr.LocatorID = "L00001";

        //    EFHelper.Add<LocationRecords>(vlr);
        //    #endregion

        //    #region 按距离推荐（距离升序）
        //    if (RecomType == 1)
        //    {
        //        try
        //        {
        //            using (var db = new EFDbContext())
        //            {

        //                var query = from p in db.Projects
        //                            join r in db.ProjectRecord on p.ProjectID equals r.ProjectID
        //                            select new { p.ProjectID, p.ProjectName, p.ProjectState, p.ProjectInfo, p.ProjectPic, p.OpeningTime, p.ProjectAttention, p.ProjectForPeople, Record = Math.Sqrt((p.ProjectXLocation - vlr.XLocation) * (p.ProjectXLocation - vlr.XLocation) + (p.ProjectYLocation - vlr.YLocation) * (p.ProjectYLocation - vlr.YLocation)) };
                                    
        //                var result = from q in query
        //                             orderby q.Record
        //                             select q;

        //                if (result.Count() == 0)
        //                    return ResponseHelper.Success(null);

        //                return ResponseHelper.Success(result.ToList());
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            if (ex.InnerException == null)
        //                return ResponseHelper.Failure(ex.Message);
        //            else
        //                return ResponseHelper.Failure(ex.InnerException.Message);
        //        }

        //    }
        //    #endregion

        //    #region 按等待人数推荐（等待人数升序）
        //    else
        //    {
        //        try
        //        {
        //            using (var db = new EFDbContext())
        //            {
        //                var wait = db.ProjectOperation.Where(o => o.PlayState == 0);
        //                var query = from p in db.Projects
        //                            join w in wait on p.ProjectID equals w.ProjectID
        //                            select new { p.ProjectID, p.ProjectName, p.ProjectState, p.ProjectInfo, p.ProjectPic, p.OpeningTime, p.ProjectAttention, p.ProjectForPeople, Record = wait.Count()};

        //                var result = from q in query
        //                             orderby q.Record
        //                             select q;

        //                if (result.Count() == 0)
        //                    return ResponseHelper.Success(null);
        //                return ResponseHelper.Success(result.ToList());
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            if (ex.InnerException == null)
        //                return ResponseHelper.Failure(ex.Message);
        //            else
        //                return ResponseHelper.Failure(ex.InnerException.Message);
        //        }

        //    }
        //    #endregion

            


        //}

        //public Stream GetStoreInfo(string VisitorID)//获取店铺信息
        //{
        //    //获取游客当前位置坐标并记录到vx，vy(暂时设置为（0，0）
        //    LocationRecords vlr = GetVisitorLocation(VisitorID);
            
        //    try
        //    {
        //        using (var db = new EFDbContext())
        //        {

        //            var query = from s in EFHelper.GetAll<Store>()
        //                        select  new { s.StoreID,s.StoreName,s.StorePic,s.StoreAddress,s.StoreInfo,Distance = Math.Sqrt((s.StoreXLocation - vlr.XLocation) * (s.StoreXLocation - vlr.XLocation) + (s.StoreYLocation - vlr.YLocation) * (s.StoreYLocation - vlr.YLocation)) };

        //            var result = from q in query
        //                         orderby  q.Distance
        //                         select q;

        //            if (result.Count() == 0)
        //                return ResponseHelper.Success(null);

        //            return ResponseHelper.Success(result.ToList());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.InnerException == null)
        //            return ResponseHelper.Failure(ex.Message);
        //        else
        //            return ResponseHelper.Failure(ex.InnerException.Message);
        //    }
        //}

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
            if (PaymentType != 4) return ResponseHelper.Failure("目前仅支持卡支付！");
            if (orders == null || orders.Count == 0) return ResponseHelper.Failure("订单信息不完全！");
            #endregion

            #region 新增订单数据
            string id = IDHelper.getNextOrderID(DateTime.Now, 0);
            List<Order> GoodsOrders = new List<Order>();
            foreach (Order item in orders)
            {
                for (int i = 1; i <= item.CommodityNum; i++)
                {
                    GoodsOrders.Add(new Order()
                    {
                        OrderID = id,
                        OrderTime = DateTime.Now,
                        OrderState = 1,
                        CommodityID = item.CommodityID,
                        CommodityType = 2,
                        CommodityNum = item.CommodityNum,
                        DoneTime = Convert.ToDateTime(DateTime.Now.ToString())
                    });
                    id = IDHelper.get4Next(id);
                }
            }
            #endregion

            #region 提交数据库
            try
            {
                for (int i = 0; i < GoodsOrders.Count; i++)
                {
                    EFHelper.Add<Order>(GoodsOrders[i]);
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
            return ResponseHelper.Success(GoodsOrders.Select(t => t.OrderID).ToList());
        }

        //public LocationRecords GetVisitorLocation(string VisitorID)//生成新位置记录并保存到数据库
        //{
        //    int vx = 0;
        //    int vy = 0;
        //    DateTime time = DateTime.Now;

        //    LocationRecords vlr = new LocationRecords();
        //    vlr.VisitorID = VisitorID;
        //    vlr.XLocation = vx;
        //    vlr.YLocation = vy;
        //    vlr.LocationTime = time;
        //    vlr.LocatorID = "L00001";

        //    EFHelper.Add<LocationRecords>(vlr);

        //    return vlr;
        //}

       

    }
}
