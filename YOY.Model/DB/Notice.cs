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
        /// 通知标题
        /// </summary>
        [Column("NoticeTitle")]
        [DataMember]
        public string NoticeTitle { get; set; }

        /// <summary>
        /// 发布时间,YYYY-MM-DD Hh:mm:ss
        /// </summary>
        [Column("NoticeTime")]
        [DataMember]
        public DateTime NoticeTime { get; set; }

        /// <summary>
        /// 内容时间，hh:mm:ss-hh:mm:ss
        /// </summary>
        [Column("ContentTime")]
        [DataMember]
        public string ContentTime { get; set; }

        /// <summary>
        /// 通知地点
        /// </summary>
        [Column("NoticeAddress")]
        [DataMember]
        public string NoticeAddress { get; set; }

        /// <summary>
        /// 通知内容
        /// </summary>
        [Column("NoticeInfo")]
        [DataMember]
        public string NoticeInfo { get; set; }
    }
}
