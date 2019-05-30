using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YOY.BLL;
using YOY.DAL;
using YOY.Model.DB;


namespace YOY.WCFService
{
    /// <summary>
    /// 卡管理接口的实现
    /// </summary>
    public class CardManagement : ICardManagement
    {
        /// <summary>
        /// 卡充值
        /// </summary>
        /// <param name="VisitorID">游客ID</param>
        /// <param name="Amount">充值金额</param>
        /// <param name="PaymentType">付款方式</param>
        /// <returns>生成的OrderID</returns>
        public Stream Recharge(string VisitorID, float Amount, int PaymentType)
        {
            #region 数据完整性检查
            if (VisitorID == null)
                return ResponseHelper.Failure("游客信息缺失！");
            if (Amount <= 0) return ResponseHelper.Failure("充值金额无效！");
            #endregion

            //根据游客ID查询卡ID
            string CardID = "";
            try
            {
                using (var db = new EFDbContext())
                {

                    var card = from v2c in db.Visitor2Cards
                               where v2c.VisitorID == VisitorID
                               select v2c;
                    if (card.Count() == 0 )
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

            #region 新增订单，新增支付信息，新增游客订单映射
            string id = IDHelper.getNextOrderID(DateTime.Now, 0);
            //生成新订单
            Order RechargeOrder = new Order()
            {
                OrderID = id,
                OrderTime = DateTime.Now,
                OrderState = 1,//已支付
                CommodityID = CardID,
                CommodityType = 3,//卡相关
                CommodityNum = (int)Amount,
                DoneTime = Convert.ToDateTime(DateTime.Now.ToString())
            };
            //生成新支付信息
            Payment RechargePay = new Payment()
            {
                OrderID = RechargeOrder.OrderID,
                PaymentType = PaymentType,
                PaymentTime = DateTime.Now,
                PaymentAmount = Amount
            };
            //增加游客订单映射
            Visitor2Order v2o = new Visitor2Order()
            {
                VisitorID = VisitorID,
                CardID = CardID,
                OrderID = id

            };
            #endregion

            #region 修改数据库，返回订单ID
            //在数据库V2C表中修改卡余额
            try
            {
                using (var db = new EFDbContext())
                {
                    
                    //订单、支付、游客订单映射信息——提交数据库
                    db.Orders.Add(RechargeOrder);
                    db.Payments.Add(RechargePay);
                    db.Visitor2Orders.Add(v2o);
                    
                    //修改余额（增加）
                    Visitor2Card new_v2c = db.Visitor2Cards.Where(v => v.VisitorID == VisitorID).Single();
                    new_v2c.Balance += Amount;

                    db.SaveChanges();
                    return ResponseHelper.Success( new List<string>() { RechargeOrder.OrderID });//返回OrderID
                    
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


        }

        /// <summary>
        /// 卡退款
        /// </summary>
        /// <param name="VisitorID">游客ID</param>
        /// <param name="Amount">退款金额</param>
        /// <param name="PaymentType">付款方式</param>
        /// <returns>生成的OrderID</returns>
        public Stream Refund(string VisitorID, float Amount, int PaymentType)
        {
            #region 数据完整性检查
            if (VisitorID == null)
                return ResponseHelper.Failure("游客信息缺失！");
            if (Amount <= 0) return ResponseHelper.Failure("退款金额无效！");
            #endregion

            //根据游客ID查询卡ID
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

            #region 新增订单，新增支付信息，新增游客订单映射
            string id = IDHelper.getNextOrderID(DateTime.Now, 0);
            //生成新订单
            Order RechargeOrder = new Order()
            {
                OrderID = id,
                OrderTime = DateTime.Now,
                OrderState = -1,//已退款
                CommodityID = CardID,
                CommodityType = 3,//卡相关
                CommodityNum = (int)Amount,
                DoneTime = Convert.ToDateTime(DateTime.Now.ToString())
            };
            //生成新支付信息
            Payment RechargePay = new Payment()
            {
                OrderID = RechargeOrder.OrderID,
                PaymentType = PaymentType,
                PaymentTime = DateTime.Now,
                PaymentAmount = Amount
            };
            //增加游客订单映射
            Visitor2Order v2o = new Visitor2Order()
            {
                VisitorID = VisitorID,
                CardID = CardID,
                OrderID = id

            };
            #endregion

            #region 修改数据库，返回订单ID
            //在数据库V2C表中修改卡余额
            try
            {
                using (var db = new EFDbContext())
                {

                    //订单、支付、游客订单映射信息——提交数据库
                    db.Orders.Add(RechargeOrder);
                    db.Payments.Add(RechargePay);
                    db.Visitor2Orders.Add(v2o);
                    
                    //修改余额（减少）
                    Visitor2Card new_v2c = db.Visitor2Cards.Where(v => v.VisitorID == VisitorID).Single();
                    new_v2c.Balance -= Amount;
                    //合法性检查
                    if (new_v2c.Balance < 0)
                    {
                        return ResponseHelper.Failure("退款金额大于余额！退款失败！");
                    }

                    db.SaveChanges();
                    return ResponseHelper.Success(new List<string>() { RechargeOrder.OrderID });//返回OrderID

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

        }


        /// <summary>
        /// 获取卡余额
        /// </summary>
        /// <param name="VisitorID">游客ID/param>
        /// <returns></returns>
        public Stream GetBalance(string VisitorID)
        {
            try
            {
                //数据库余额查询
                using (var db = new EFDbContext())
                {

                    var left = from v2c in db.Visitor2Cards
                               where v2c.VisitorID == VisitorID
                               join v in db.Visitors on v2c.VisitorID equals v.VisitorID
                               select new { v.UID, v2c.Balance, v.Name };
                    
                    return ResponseHelper.Success(left.ToList());//返回OrderID
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

        /// <summary>
        /// 获取卡充值、退款记录
        /// </summary>
        /// <param name="VisitorID">游客ID</param>
        /// <returns></returns>
        public Stream CardRecord(string VisitorID)
        {
            try
            {
                using (var db = new EFDbContext())
                {

                    var record = from v2o in db.Visitor2Orders
                               where v2o.VisitorID == VisitorID
                               join p in db.Payments on v2o.OrderID equals p.OrderID
                               select new { v2o.OrderID , p.PaymentAmount , p.PaymentType , p.PaymentTime};
                    var result = from r in record
                                 join o in db.Orders on r.OrderID equals o.OrderID
                                 where o.CommodityType == 3  //卡相关
                                 select new { o.OrderID, o.OrderState, r.PaymentAmount, r.PaymentType, r.PaymentTime };

                    return ResponseHelper.Success(result.ToList());//返回OrderID
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
