﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using YOY.BLL;
using YOY.DAL;
using YOY.Model.DB;
using YOY.Model;
using ModuleTech;


namespace YOY.WCFService
{
    /// <summary>
    /// 管理员操作接口实现
    /// </summary>
    public class AdminFunctions : IAdminFunctions
    {
        /// <summary>
        /// 获取所有给定日期的通知信息
        /// </summary>
        /// <param name="Date">日期数组  [‘YY-MM-DD hh:mm:ss’,’ YY-MM-DD hh:mm:ss’]</param>
        /// <returns></returns>
        public Stream GetAllNotice(DateTime[] Date)
        {
            List<Notice> result = new List<Notice>();

            for ( int i = 0 ; i < Date.Length ; i++ )
            {
                try
                {
                    using (var db = new EFDbContext())
                    {
                        var query = from n in db.Notices.Where(n1 => n1.NoticeTime > Date[i])
                                    where n.NoticeTime < Date[i].AddDays(1)
                                    select n;
                        
                        foreach (Notice item in query)
                        {
                            result.Add (item);
                        }
                        
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

            return ResponseHelper.Success(result);
        }

        /// <summary>
        /// 审核通过申请
        /// </summary>
        /// <param name="NoticeID">通知ID</param>
        /// <returns></returns>
        public Stream PassNotice(string NoticeID)
        {
            Notice Pass = new Notice();

            try
            {
                using (var db = new EFDbContext())
                {
                    var query = db.Notices.Where(n => n.NoticeID == NoticeID);

                    //合法性检查
                    if (query.Count() == 0)
                        return ResponseHelper.Failure("未找到此通知！");
                    if (query.Single().NoticeStatus == 3)
                        return ResponseHelper.Failure("此通知已失效！");
                    if (query.Single().NoticeStatus == 2)
                        return ResponseHelper.Failure("此通知已被拒绝！");
                    if (query.Single().NoticeStatus == 1)
                        return ResponseHelper.Failure("此通知已通过审核，无需重复操作！");

                    //修改通知状态为已通过审核，并记录审核时间
                    Pass = query.Single();
                    Pass.NoticeStatus = 1;  //通过申请
                    Pass.CheckTime = DateTime.Now;//记录审核通过的时间

                    //提交数据库修改
                    EFHelper.Update<Notice>(Pass);  
                    return ResponseHelper.Success(new List<string>(){ "已审核通过申请！"});
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
        /// 审核拒绝申请
        /// </summary>
        /// <param name="NoticeID">通知ID</param>
        /// <param name="Remarks">审核备注（可以为空）</param>
        /// <returns></returns>
        public Stream RefuseNotice(string NoticeID, string Remarks)
        {
            Notice Pass = new Notice();

            try
            {
                using (var db = new EFDbContext())
                {
                    var query = db.Notices.Where(n => n.NoticeID == NoticeID);

                    //合法性检查
                    if (query.Count() == 0)
                        return ResponseHelper.Failure("未找到此通知！");
                    if (query.Single().NoticeStatus == 3)
                        return ResponseHelper.Failure("此通知已失效！");
                    if (query.Single().NoticeStatus == 2)
                        return ResponseHelper.Failure("此通知已被拒绝,无需重复操作！");
                    if (query.Single().NoticeStatus == 1)
                        return ResponseHelper.Failure("此通知已通过审核!");

                    //修改通知状态为拒绝并记录审核时间和备注
                    Pass = query.Single();
                    Pass.NoticeStatus = 2;  //拒绝申请
                    Pass.CheckTime = DateTime.Now;//记录审核拒绝的时间
                    Pass.Remarks = Remarks;//记录审核备注

                    //提交数据库修改
                    EFHelper.Update<Notice>(Pass);  
                    return ResponseHelper.Success(new List<string>() { "经审核，拒绝发布申请！" });
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
        /// 绑定卡操作的接口
        /// </summary>
        /// <param name="VisitorID">游客ID</param>
        /// <returns>绑定结果</returns>
        public Stream BindingCard(string VisitorID)
        {
            if (string.IsNullOrEmpty(VisitorID))
                return ResponseHelper.Failure("游客ID不能为空！");

            #region 建立读卡器连接并读取卡ID
            Reader modulerdr = null;
            string cardID = null;
            try
            {
                modulerdr = Reader.Create("192.168.0.103", Region.NA, 4);
            }
            catch(Exception ex)
            {
                ResponseHelper.Failure(ex.Message);
            }

            modulerdr.ParamSet("ReadPlan", new SimpleReadPlan(TagProtocol.GEN2, new List<int>() { 4 }.ToArray(), 30));

            while(true)
            {
                int errCnt = 0;   //失败次数
                try
                {
                    TagReadData[] reads = modulerdr.Read(200);
                    if(reads.Count() > 0)
                    {
                        var card = from r in reads
                                   group r by r.EPCString into c
                                   select new { CardID = c.Key };
                        cardID = card.First().CardID;

                        break;
                    }
                }
                catch (Exception ex)
                {
                    if(++errCnt >= 10)
                        return ResponseHelper.Failure(ex.Message);
                }
            }
            if( string.IsNullOrEmpty(cardID) )
                return ResponseHelper.Failure("未能读取到卡ID！");
            #endregion

            #region 将卡ID与游客绑定
            var cards = EFHelper.GetAll<Card>().Where(t => t.CardID == cardID);
            if (cards.Count() == 0) return ResponseHelper.Failure("此卡非法！");
            if( cards.Single().CardState == 1 )
                return ResponseHelper.Failure("此卡已进行过绑定！");

            //在“游客、卡映射表”中新增对应关系
            try
            {
                using(var db = new EFDbContext())
                {
                    db.Visitor2Cards.Add(new Visitor2Card()
                    {
                        CardID = cardID,
                        VisitorID = VisitorID,
                        Balance = 0 
                    });
                    db.Cards.Single(t => t.CardID == cardID).CardState = 1;

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
            #endregion
        }

    }
}
