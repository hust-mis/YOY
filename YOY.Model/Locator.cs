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
    /// 定位器信息实体类
    /// </summary>
    [Table("Locators")]
    [DataContract]
    public class Locator
    {
        /// <summary>
        /// 定位器ID
        /// </summary>
        [Key]
        [Column("LocatorID")]
        [DataMember]
        public string LocatorID { get; set; }

        /// <summary>
        /// 定位器状态：0：未使用；1：使用
        /// </summary>
        [Column("LocatorState")]
        [DataMember]
        public int LocatorState { get; set; }

        /// <summary>
        /// 定位器说明
        /// </summary>
        [Column("LocatorInfo")]
        [DataMember]
        public string LocatorInfo { get; set; }

        /// <summary>
        /// 与定位器绑定的游客
        /// </summary>
        [Column("VisitorID")]
        [DataMember]
        public string VisitorID { get; set; }
    }
}
