﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace YOY.Model
{
    /// <summary>
    /// 服务端返回的数据格式
    /// </summary>
    [DataContract]
    public class Response
    {
        /// <summary>
        /// 结果代码：0为失败，1为成功
        /// </summary>
        [DataMember]
        public int code { get; set; }

        /// <summary>
        /// 错误信息，成功为空，失败为异常原因
        /// </summary>
        [DataMember]
        public string errMsg { get; set; }

        /// <summary>
        /// 结果集，返回的实体列表，失败为空
        /// </summary>
        [DataMember]
        public IList result { get; set; }
    }
}
