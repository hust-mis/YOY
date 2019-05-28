using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace YOY.Model
{
    /// <summary>
    /// 游客订单映射表
    /// </summary>
    [Table("Visitor2Order")]
    [DataContract]
    public class Visitor2Order
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        [Key]
        [Column("OrderID")]
        [DataMember]
        public string OrderID { get; set; }

        /// <summary>
        /// 游客ID
        /// </summary>
        [Column("VisitorID")]
        [DataMember]
        public string VisitorID { get; set; }

        /// <summary>
        /// 卡ID
        /// </summary>
        [Column("CardID")]
        [DataMember]
        public string CardID { get; set; }
    }
}
