﻿using System;
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
    /// 项目实时运行信息实体类
    /// </summary>
    [Table("ProjectRecord")]
    [DataContract]
    public class ProRecord
    {
        /// <summary>
        /// 项目ID  主键
        /// </summary>
        [Key]
        [Column("ProjectID", Order = 1)]
        [DataMember]
        public string ProjectID { get; set; }

        /// <summary>
        /// 游客ID  主键
        /// </summary>
        [Key]
        [Column("VisitorID", Order = 2)]
        [DataMember]
        public string VisitorID { get; set; }

        /// <summary>
        /// 游玩状态
        /// 0：等待
        /// 1：游玩
        /// -1：退出
        /// </summary>
        [Key]
        [Column("PlayState",Order = 3)]
        [DataMember]
        public int PlayState { get; set; }

        /// <summary>
        /// 时间戳
        /// 游玩状态改变的发生时间节点
        /// YYYY-MM-DD hh:mm:ss
        /// </summary>
        [Column("Timestamp")]
        [DataMember]
        public DateTime Timestamp { get; set; }
    }
}
