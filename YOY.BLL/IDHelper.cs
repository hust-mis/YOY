using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YOY.DAL;
using YOY.Model.DB;

namespace YOY.BLL
{
    /// <summary>
    /// 系统各编码的工具类
    /// </summary>
    public sealed class IDHelper
    {
        /// <summary>
        /// 获取后5位顺序码的下一个编码
        /// </summary>
        /// <param name="current">当前编码</param>
        /// <returns>下一个编码</returns>
        public static string get5Next(string current)
        {
            string code = current.Substring(current.Length - 5);
            int nextRank = int.Parse(code) + 1;

            if (nextRank > 99999) return null;

            return current.Remove(current.Length - 5) + nextRank.ToString().PadLeft(5, '0');
        }

        /// <summary>
        /// 获取后4位顺序码的下一个编码
        /// </summary>
        /// <param name="current">当前编码</param>
        /// <returns>下一个编码</returns>
        public static string get4Next(string current)
        {
            string code = current.Substring(current.Length - 4);
            int nextRank = int.Parse(code) + 1;

            if (nextRank > 9999) return null;

            return current.Remove(current.Length - 4) + nextRank.ToString().PadLeft(4, '0');
        }

        /// <summary>
        /// 获取当前最大的TicketID
        /// </summary>
        /// <returns>Tickets表中最大的TicketID</returns>
        public static string getMaxTicketID()
        {
            var db = new EFDbContext();
            var query = db.Tickets.Max(t => t.TicketID )  ;

            if (string.IsNullOrEmpty(query)) return "T00000";
            else return query;
        }

        /// <summary>
        /// 获取下一位TicketID
        /// </summary>
        /// <returns>下一位TicketID</returns>
        public static string getNextTicketID()
        {
            return get5Next(getMaxTicketID());
        }

        /// <summary>
        /// 获取指定日期和商品类型的OrderID最大值
        /// </summary>
        /// <param name="date">订单生成日期</param>
        /// <param name="type">商品类型</param>
        /// <returns>当前OrderID的最大值</returns>
        public static string getMaxOrderID(DateTime date , int type )
        {
            var db = new EFDbContext();
            var list = db.Orders.Where(t => t.CommodityType == type).ToList();
            var query = list.Where(t => t.OrderTime.ToShortDateString() == date.ToShortDateString())
                            .Max(t => t.OrderID);
            
            if (string.IsNullOrEmpty(query))
                return string.Format("O{0}{1}{2}", date.ToString("yyyyMMdd"), type.ToString(), "0000");
            else return query;
        }

        /// <summary>
        /// 获取指定日期和商品类型的OrderID下一值
        /// </summary>
        /// <param name="date">订单生成日期</param>
        /// <param name="type">商品类型</param>
        /// <returns>下一位OrderID</returns>
        public static string getNextOrderID(DateTime date, int type)
        {
            return get4Next(getMaxOrderID(date,type));
        }

        /// <summary>
        /// 获取指定日期的VisitorID的最大值
        /// </summary>
        /// <param name="date">游客游玩日期（订单中的DoneTime）</param>
        /// <returns>当前VisitorID的最大值</returns>
        public static string getMaxVisitorID(DateTime date)
        {
            var db = new EFDbContext();
            var list = db.Visitors.ToList();
            var query = list.Where(t => t.VisitorID.Substring(1,8) == date.ToString("yyyyMMdd"))
                            .Max(t => t.VisitorID);

            if (string.IsNullOrEmpty(query))
                return string.Format("V{0}{1}", date.ToString("yyyyMMdd"), "0000");
            else return query;
        }

        /// <summary>
        /// 获取指定日期的下一位VisitorID
        /// </summary>
        /// <param name="date">游客游玩日期（订单中的DoneTime）</param>
        /// <returns>下一位VisitorID</returns>
        public static string getNextVisitorID(DateTime date)
        {
            return get4Next(getMaxVisitorID(date));
        }

        /// <summary>
        /// 获取指定日期的GroupID的最大值
        /// </summary>
        /// <param name="date">队伍建立日期</param>
        /// <returns>当前GroupID的最大值</returns>
        public static string getMaxGroupID(DateTime date)
        {
            var groups = EFHelper.GetAll<Group>();
            var query = groups.Where(t => t.GroupID.Substring(1, 8) == date.ToString("yyyyMMdd")).Max(t => t.GroupID);

            if (string.IsNullOrEmpty(query))
                return string.Format("G{0}{1}", date.ToString("yyyyMMdd"), "0000");
            else return query;
        }

        /// <summary>
        /// 获取指定日期的下一位GroupID
        /// </summary>
        /// <param name="date">队伍建立日期</param>
        /// <returns>下一位GroupID</returns>
        public static string getNextGroupID(DateTime date)
        {
            return get4Next(getMaxGroupID(date));
        }

        /// <summary>
        /// 获取指定日期的NoticeID的最大值
        /// </summary>
        /// <param name="date">发布通知日期</param>
        /// <returns>当前NoticeID的最大值</returns>
        public static string getMaxNoticeID(DateTime date)
        {
            var db = new EFDbContext();
            var list = db.Notices.ToList();
            var query = list.Where(t => t.NoticeID.Substring(1, 8) == date.ToString("yyyyMMdd"))
                            .Max(t => t.NoticeID);

            if (string.IsNullOrEmpty(query))
                return string.Format("V{0}{1}", date.ToString("yyyyMMdd"), "0000");
            else return query;
        }

        /// <summary>
        /// 获取指定日期的下一NoticeID
        /// </summary>
        /// <param name="date">发布通知日期</param>
        /// <returns>下一NoticeID</returns>
        public static string getNextNoticeID(DateTime date)
        {
            return get4Next(getMaxNoticeID(date));
        }

    }
}
