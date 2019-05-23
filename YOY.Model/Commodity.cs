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
    /// 商品信息实体类
    /// </summary>
    [Table("Commodities")]
    [DataContract]
    public class Commodity
    {
        /// <summary>
        /// 商品ID，主键 Cxxxxx
        /// </summary>
        [Key]
        [Column("CommodityID")]
        [DataMember]
        public string CommodityID { get; set; }

        /// <summary>
        /// 店铺ID
        /// </summary>
        [Column("StoreID")]
        [DataMember]
        public string StoreID { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [Column("CommodityName")]
        [DataMember]
        public string CommodityName { get; set; }

        /// <summary>
        /// 商品类型（1：饮品 2：菜肴 3：普通商品）
        /// </summary>
        [Column("CommodityType")]
        [DataMember]
        public int CommodityType { get; set; }

        /// <summary>
        /// 商品价格(单位：RMB)
        /// </summary>
        [Column("CommodityPrice")]
        [DataMember]
        public float CommodityPrice { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        [Column("CommodityInfo")]
        [DataMember]
        public string CommodityInfo { get; set; }

        /// <summary>
        /// 商品图片，即图片的URL
        /// </summary>
        [Column("CommodityPic")]
        [DataMember]
        public string CommodityPic { get; set; }
    }
}
