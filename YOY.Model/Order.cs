using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace YOY.Model
{
    /// <summary>
    /// 订单信息实体
    /// </summary>
    [Table("Orders")]
    [DataContract]
    public class Order
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        [Column("OrderID")]
        [DataMember]
        public string OrderID { get; set; }

        /// <summary>
        /// 生成时间，YYYY-MM-DD hh:mm:ss
        /// </summary>
        [Column("OrderTime")]
        [DataMember]
        public string OrderTime { get; set; }

        /// <summary>
        /// 订单状态，0：未支付，1：已支付
        /// </summary>
        [Column("OrderState")]
        [DataMember]
        public int OrderState { get; set; }

        /// <summary>
        /// 商品ID，订单中包含的商品
        /// </summary>
        [Column("CommodityID")]
        [DataMember]
        public string CommodityID { get; set; }

        /// <summary>
        /// 商品类型，0：门票，1：游乐项目，2：纪念品/餐饮
        /// </summary>
        [Column("CommodityType")]
        [DataMember]
        public int CommodityType { get; set; }

        /// <summary>
        /// 商品数量，对应商品的数量
        /// </summary>
        [Column("CommodityNum")]
        [DataMember]
        public int CommodityNum { get; set; }

        /// <summary>
        /// 消费完成时间，YYYY-MM-DD hh:mm:ss
        /// </summary>
        [Column("DoneTime")]
        [DataMember]
        public string DoneTime { get; set; }
    }
}
