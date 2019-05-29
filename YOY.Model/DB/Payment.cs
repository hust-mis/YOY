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
    /// 支付信息实体
    /// </summary>
    [Table("Payments")]
    [DataContract]
    public class Payment
    {
        /// <summary>
        /// 订单ID，对应的订单
        /// </summary>
        [Key]
        [Column("OrderID")]
        [DataMember]
        public string OrderID { get; set; }

        /// <summary>
        /// 支付金额，单位：RMB
        /// </summary>
        [Column("PaymentAmount")]
        [DataMember]
        public double PaymentAmount { get; set; }

        /// <summary>
        /// 支付方式，0：支付宝，1：微信，2：银联，3：现金
        /// </summary>
        [Column("PaymentType")]
        [DataMember]
        public int PaymentType { get; set; }

        /// <summary>
        /// 支付时间，YYYY-MM-DD hh:mm:ss
        /// </summary>
        [Column("PaymentTime")]
        [DataMember]
        public DateTime PaymentTime { get; set; }
    }
}
