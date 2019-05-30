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
    /// 付费项目信息实体类
    /// </summary>
    [Table("ChargeProjects")]
    [DataContract]
    public class ChargeProject
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        [Key]
        [Column("ProjectID")]
        [DataMember]
        public string ProjectID { get; set; }

        /// <summary>
        /// 项目价格
        /// 单位：RMB
        /// </summary>
        [Column("ProjectPrice")]
        [DataMember]
        public double ProjectPrice { get; set; }

        /// <summary>
        /// 价格说明
        /// </summary>
        [Column("PriceDescription")]
        [DataMember]
        public string PriceDescription { get; set; }
    }
}
