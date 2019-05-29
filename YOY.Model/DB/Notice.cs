using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace YOY.Model.DB
{
    /// <summary>
    /// 游园信息表
    /// </summary>
    [Table("Notice")]
    [DataContract]
    public class Notice
    {
        /// <summary>
        /// 通知ID
        /// </summary>
        [Key]
        [Column("NoticeID")]
        [DataMember]
        public string NoticeID { get; set; }

        /// <summary>
        /// 若为空，则为管理员发布的信息，若为正常ID，则为游客发布信息
        /// </summary>
        [Column("VisitorID")]
        [DataMember]
        public string VisitorID { get; set; }

        /// <summary>
        /// 通知类型:
        /// 0：寻人通知
        /// 1：寻物通知
        /// 2：活动通知
        /// 3：园内提醒
        /// </summary>
        [Column("NoticeType")]
        [DataMember]
        public int NoticeType { get; set; }

        /// <summary>
        /// 发布时间
        /// YYYY-MM-DD hh:mm:ss
        /// </summary>
        [Column("NoticeTime")]
        [DataMember]
        public DateTime NoticeTime { get; set; }

        /// <summary>
        /// 事件发生时间
        /// hh:mm:ss-hh:mm:ss
        /// </summary>
        [Column("OccurTime")]
        [DataMember]
        public string OccurTime { get; set; }

        /// <summary>
        /// 事件发生地点
        /// </summary>
        [Column("OccurAddress")]
        [DataMember]
        public string OccurAddress { get; set; }

        /// <summary>
        /// 事件详细描述
        /// </summary>
        [Column("NoticeDetail")]
        [DataMember]
        public string NoticeDetail { get; set; }

        /// <summary>
        /// 通知状态
        /// 0：未审核
        /// 1：审核通过
        /// 2：审核拒绝
        /// </summary>
        [Column("NoticeStatus")]
        [DataMember]
        public int NoticeStatus { get; set; }

        /// <summary>
        /// 审核备注:未通过原因
        /// </summary>
        [Column("Remarks")]
        [DataMember]
        public string Remarks { get; set; }

        /// <summary>
        /// 审核时间
        /// YYYY-MM-DD Hh:mm:ss
        /// </summary>
        [Column("CheckTime")]
        [DataMember]
        public DateTime CheckTime { get; set; }

    }
}
