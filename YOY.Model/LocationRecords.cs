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
    /// 位置信息记录实体类
    /// </summary>
    [Table("LocationRecord")]
    [DataContract]
    public class LocationRecords
    {
        /// <summary>
        /// 定位器ID
        /// </summary>
        [Key]
        [Column("LocatorID", Order = 1 )]
        [DataMember]
        public string LocatorID { get; set; }

        /// <summary>
        /// 游客ID
        /// </summary>
        [Column("VisitorID")]
        [DataMember]
        public string VisitorID { get; set; }

        /// <summary>
        /// 时间 YYYY-MM-DD hh:mm:ss
        /// </summary>
        [Key]
        [Column("LocationTime", Order = 2)]
        [DataMember]
        public DateTime LocationTime{ get; set; }

        /// <summary>
        /// X坐标
        /// </summary>
        [Column("XLocation")]
        [DataMember]
        public int XLocation { get; set; }

        /// <summary>
        /// Y坐标
        /// </summary>
        [Column("YLocation")]
        [DataMember]
        public int YLocation { get; set; }

    }
}
