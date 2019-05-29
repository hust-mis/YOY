using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace YOY.WCFService
{
    /// <summary>
    /// 
    /// 通知管理接口的声明
    /// </summary>
    [ServiceContract]
    public interface INoticeManagement
    {
        [OperationContract]
        void DoWork();
    }
}
