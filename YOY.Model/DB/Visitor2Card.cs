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
    /// 用户订单映射实体
    /// </summary>
    [Table("Visitor2Card")]
    [DataContract]
    public class Visitor2Card
    {
        /// <summary>
        /// 游客ID
        /// </summary>
        [Key]
        [Column("VisitorID")]
        [DataMember]
        public string VisitorID { get; set; }

        /// <summary>
        /// 卡ID
        /// </summary>
        [Column("CardID")]
        [DataMember]
        public string CardID { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        [Column("Balance")]
        [DataMember]
        public double Balance { get; set; }

        /// <summary>
        /// 绑卡时间
        /// </summary>
        [Column("BindTime")]
        [DataMember]
        public DateTime BindTime { get; set; }

        /// <summary>
        /// 退卡时间
        /// </summary>
        [Column("UnbindTime")]
        [DataMember]
        public DateTime UnbindTime { get; set; }

    }
}
