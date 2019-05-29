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
    /// 游园通知信息表
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
        /// 游客ID
        /// </summary>
        [Column("VisitorID")]
        [DataMember]
        public string VisitorID { get; set; }

        /// <summary>
        /// 通知类型  0:寻人 1：寻物 2：活动 3：提醒
        /// </summary>
        [Column("NoticeType")]
        [DataMember]
        public int NoticeType { get; set; }

        /// <summary>
        /// 发布时间,YYYY-MM-DD Hh:mm:ss
        /// </summary>
        [Column("NoticeTime")]
        [DataMember]
        public DateTime NoticeTime { get; set; }

        /// <summary>
        /// 事件发生时间
        /// </summary>
        [Column("OccurTime")]
        [DataMember]
        public DateTime ContentTime { get; set; }

        /// <summary>
        /// 事件发生地点
        /// </summary>
        [Column("OccurAddress")]
        [DataMember]
        public string OccurAddress { get; set; }

        /// <summary>
        /// 事件详细描述
        /// </summary>
        [Column("NoticeDetails")]
        [DataMember]
        public string NoticeDetails { get; set; }

        /// <summary>
        /// 通知状态  0:未审核 1：审核通过 2：审核拒绝 3：已失效
        /// </summary>
        [Column("NoticeStatus")]
        [DataMember]
        public int NoticeStatus { get; set; }

        /// <summary>
        /// 审核备注
        /// </summary>
        [Column("Remarks")]
        [DataMember]
        public string Remarks { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        [Column("CheckTime")]
        [DataMember]
        public DateTime CheckTime { get; set; }

    }
}
