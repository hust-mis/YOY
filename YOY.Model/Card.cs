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
    /// 卡信息实体
    /// </summary>
    [Table("Cards")]
    [DataContract]
    public class Card
    {
        /// <summary>
        /// 卡ID
        /// </summary>
        [Key]
        [Column("CardID")]
        [DataMember]
        public string CardID { get; set; }

        /// <summary>
        /// 卡状态，0：未使用，1：使用中
        /// </summary>
        [Column("CardState")]
        [DataMember]
        public int CardState { get; set; }
    }
}
