using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace YOY.Model.DB
{
    /// <summary>
    /// 用户信息实体类
    /// </summary>
    [Table("Users")]
    [DataContract]
    public class User
    {
        /// <summary>
        /// 手机号码，11位
        /// </summary>
        [Key]
        [Column("PhoneNumber")]
        [DataMember]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 身份证号码，18位
        /// </summary>
        [Column("UID")]
        [DataMember]
        public string UID { get; set; }
        
        /// <summary>
        /// 姓名，最多五个汉字
        /// </summary>
        [Column("Name")]
        [DataMember]
        public string Name { get; set; }
    }
}
