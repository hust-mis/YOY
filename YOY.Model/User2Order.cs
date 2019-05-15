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
    /// 用户订单映射实体
    /// </summary>
    [Table("User2Order")]
    [DataContract]
    public class User2Order
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        [Column("PhoneNumber")]
        [DataMember]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        [Key]
        [Column("OrderID")]
        [DataMember]
        public string OrderID { get; set; }

        /// <summary>
        /// 游客ID，每个订单产生对应的游客ID
        /// </summary>
        [Column("VisitorID")]
        [DataMember]
        public string VisitorID { get; set; }
    }
}
