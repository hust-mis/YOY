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
    /// 通知管理接口的实现
    /// </summary>
    public class NoticeManagement : INoticeManagement
    {
        /// <summary>
        /// 发布通知接口
        /// </summary>
        /// <param name="VisitorID">游客ID,如果Admin代表管理员</param>
        /// <param name="NoticeType"></param>
        /// <param name="OccurTime"></param>
        /// <param name="OccurAddress"></param>
        /// <param name="NoticeDetail"></param>
        /// <returns></returns>
        public Stream PublishNotice(string VisitorID, int NoticeType, string OccurTime, string OccurAddress, string NoticeDetail)
        {
            if(VisitorID!="Admin"&&NoticeType==2)//合法性检查
                return ResponseHelper.Failure("只有管理员可以发布活动通知！");

            string id = IDHelper.getNextNoticeID(DateTime.Now);
            Notice notice = new Notice()
            {
                NoticeID = id,
                VisitorID = VisitorID,
                NoticeType = NoticeType,
                NoticeTime = DateTime.Now,
                OccurTime = OccurTime,
                OccurAddress = OccurAddress,
                NoticeDetail = NoticeDetail,
                NoticeStatus = 0,
                Remarks = "",
                CheckTime = null
            };

            try
            {
                EFHelper.Add<Notice>(notice);  //通知信息提交数据库
                return ResponseHelper.Success(new List<string>() { "发布成功，等待审核！" });
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                return ResponseHelper.Failure(ex.Message);
            }
         
        }

        /// <summary>
        /// 获取我发布的通知
        /// </summary>
        /// <param name="VisitorID">游客ID,如果Admin代表管理员</param>
        /// <returns></returns>
        public Stream GetMyNotice(string VisitorID)
        {
            try
            {
                using (var db = new EFDbContext())
                {
                    var query = from q in db.Notices.Where(n => n.VisitorID == VisitorID)
                                select new { q.NoticeID , q.NoticeType , q.OccurTime , q.OccurAddress , q.NoticeDetail , q.NoticeStatus , q.Remarks};

                    if (query.Count() == 0) return ResponseHelper.Success(new List<string>() { "您还未发布通知！" });
                    return ResponseHelper.Success(query.ToList());
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                return ResponseHelper.Failure(ex.Message);
            }



        }

        /// <summary>
        /// 删除我发布的通知
        /// </summary>
        /// <param name="VisitorID">游客ID,如果Admin代表管理员</param>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public Stream DelMyNotice(string VisitorID, string NoticeID)
        {
            Notice Del = new Notice();

            try
            {
                using (var db = new EFDbContext())
                {
                    var query = db.Notices.Where(n => n.NoticeID == NoticeID);
                    
                    //合法性检查
                    if (query.Single().NoticeStatus == 3)
                        return ResponseHelper.Failure("此通知已失效！");
                    if (query.Count() == 0)
                        return ResponseHelper.Failure("未发布过此通知！");

                    //修改通知状态为已失效
                    Del = query.Single();
                    Del.NoticeStatus = 3;  //已失效
                    EFHelper.Update<Notice>(Del);  //提交数据库修改
                    return ResponseHelper.Success(new List<string>() { "删除成功！" });
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                return ResponseHelper.Failure(ex.Message);
            }

        }

    }
}
