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
    /// 队伍信息实体
    /// </summary>
    [Table("Groups")]
    [DataContract]
    public class Group
    {
        /// <summary>
        /// 队伍ID
        /// </summary>
        [Column("GroupID")]
        [DataMember]
        public string GroupID { get; set; }

        /// <summary>
        /// 游客ID
        /// </summary>
        [Key]
        [Column("VisitorID",Order = 1)]
        [DataMember]
        public string VisitorID { get; set; }

        /// <summary>
        /// 游客状态，0：被邀请中，1：在队伍中
        /// </summary>
        [Column("InviteeState")]
        [DataMember]
        public int InviteeState { get; set; }

        /// <summary>
        /// 邀请者ID，邀请该游客的游客ID
        /// </summary>
        [Key]
        [Column("InviterID" , Order = 2)]
        [DataMember]
        public string InviterID { get; set; }
    }
}
