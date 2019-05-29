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
                    return ResponseHelper.Success("已审核通过申请！");
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
                    return ResponseHelper.Success("经审核，拒绝发布申请！");
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
