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
    /// 项目信息实体类
    /// </summary>
    [Table("Projects")]
    [DataContract]
    public class Project
    {
        /// <summary>
        /// 项目ID  主键
        /// </summary>
        [Key]
        [Column("ProjectID")]
        [DataMember]
        public string ProjectID { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [Column("ProjectName")]
        [DataMember]
        public string ProjectName { get; set; }

        /// <summary>
        /// 项目状态 （0：未启用  1：启用中）
        /// </summary>
        [Column("ProjectState")]
        [DataMember]
        public int ProjectState { get; set; }

        /// <summary>
        /// 项目描述
        /// </summary>
        [Column("ProjectInfo")]
        [DataMember]
        public string ProjectInfo { get; set; }

        /// <summary>
        /// 项目图片URL
        /// </summary>
        [Column("ProjectPic")]
        [DataMember]
        public string ProjectPic { get; set; }

        /// <summary>
        /// 项目开放时间  hh:mm:ss-hh:mm:ss
        /// </summary>
        [Column("OpeningTime")]
        [DataMember]
        public string OpeningTime { get; set; }

        /// <summary>
        /// 注意事项
        /// </summary>
        [Column("ProjectAttention")]
        [DataMember]
        public string ProjectAttention { get; set; }

        /// <summary>
        /// 适用人群
        /// </summary>
        [Column("ProjectForPeople")]
        [DataMember]
        public string ProjectForPeople { get; set; }

        /// <summary>
        /// 项目X坐标
        /// </summary>
        [Column("ProjectXLocation")]
        [DataMember]
        public int ProjectXLocation { get; set; }

        /// <summary>
        /// 项目Y坐标
        /// </summary>
        [Column("ProjectYLocation")]
        [DataMember]
        public int ProjectYLocation { get; set; }
    }
}
