using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace YOY.Model
{
    /// <summary>
    /// LocalSense获取的位置信息实体
    /// </summary>
    [DataContract]
    public class Location
    {
        /// <summary>
        /// 标签ID
        /// </summary>
        [DataMember]
        public string ID { get; set; }

        /// <summary>
        /// X坐标
        /// </summary>
        [DataMember]
        public string X { get; set; }

        /// <summary>
        /// Y坐标
        /// </summary>
        [DataMember]
        public string Y { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        [DataMember]
        public string Timestamp { get; set; }
    }
}
