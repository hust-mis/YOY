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
        /// <param name="VisitorID"></param>
        /// <param name="NoticeType"></param>
        /// <param name="OccurTime"></param>
        /// <param name="OccurAddress"></param>
        /// <param name="NoticeDetail"></param>
        /// <returns></returns>
        public Stream PublishNotice(string VisitorID, int NoticeType, DateTime OccurTime, string OccurAddress, string NoticeDetail)
        {



        }

        public Stream GetMyNotice(string VisitorID)
        {

        }

        public Stream DelMyNotice(string VisitorID, string NoticeID)
        {

        }

    }
}
