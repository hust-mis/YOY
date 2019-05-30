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
    /// 游客信息实体
    /// </summary>
    [Table("Visitors")]
    [DataContract]
    public class Visitor
    {
        /// <summary>
        /// 游客ID
        /// </summary>
        [Key]
        [Column("VisitorID")]
        [DataMember]
        public string VisitorID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Column("Name")]
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [Column("UID")]
        [DataMember]
        public string UID { get; set; }

        /// <summary>
        /// 性别，0为女，1为男
        /// </summary>
        [Column("Gender")]
        [DataMember]
        public int Gender { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [Column("Age")]
        [DataMember]
        public int Age { get; set; }

        /// <summary>
        /// 游客登陆密码，至少6位，最多26位
        /// </summary>
        [Column("Password")]
        [DataMember]
        public string Password { get; set; }

        /// <summary>
        /// 游客激活状态，0：未游玩；1：已游玩
        /// </summary>
        [Column("VisitorState")]
        [DataMember]
        public int VisitorState { get; set; }

        /// <summary>
        /// 游玩时间，超时间后更改激活状态，YYYY-MM-DD
        /// </summary>
        [Column("PlayTime")]
        [DataMember]
        public DateTime PlayTime { get; set; }
    }
}
