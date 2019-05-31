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
    /// 店铺信息实体类
    /// </summary>
    [Table("Stores")]
    [DataContract]
    public class Store
    {
        /// <summary>
        /// 店铺ID，主键 Sxxxxx
        /// </summary>
        [Key]
        [Column("StoreID")]
        [DataMember]
        public string StoreID { get; set; }

        /// <summary>
        /// 店铺名称
        /// </summary>
        [Column("StoreName")]
        [DataMember]
        public string StoreName { get; set; }

        /// <summary>
        /// 店铺图片，即图片的URL
        /// </summary>
        [Column("StorePic")]
        [DataMember]
        public string StorePic { get; set; }

        /// <summary>
        /// 店铺地址
        /// </summary>
        [Column("StoreAddress")]
        [DataMember]
        public string StoreAddress { get; set; }

        
        /// <summary>
        /// 店铺介绍
        /// </summary>
        [Column("StoreInfo")]
        [DataMember]
        public string StoreInfo { get; set; }

        /// <summary>
        /// 店铺X坐标
        /// </summary>
        [Column("StoreXLocation")]
        [DataMember]
        public int? StoreXLocation { get; set; }

        /// <summary>
        /// 店铺Y坐标
        /// </summary>
        [Column("StoreYLocation")]
        [DataMember]
        public int? StoreYLocation { get; set; }
    }    
}
