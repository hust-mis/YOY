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
    /// 门票信息实体类
    /// </summary>
    [Table("Tickets")]
    [DataContract]
    public class Ticket
    {
        /// <summary>
        /// 门票ID，主键
        /// </summary>
        [Key]
        [Column("TicketID")]
        [DataMember]
        public string TicketID { get; set; }

        /// <summary>
        /// 门票名称
        /// </summary>
        [Column("TicketName")]
        [DataMember]
        public string TicketName { get; set; }

        /// <summary>
        /// 门票说明
        /// </summary>
        [Column("TicketInfo")]
        [DataMember ]
        public string TicketInfo { get; set; }

        /// <summary>
        /// 门票价格,单位：RMB
        /// </summary>
        [Column("TicketPrice")]
        [DataMember]
        public double TicketPrice { get; set; }

        /// <summary>
        /// 门票图片，即图片的URL
        /// </summary>
        [Column("TicketPic")]
        [DataMember]
        public string TicketPic { get; set; }
    }
}
