using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using YOY.Model.DB;

namespace YOY.WCFService
{
    
    /// <summary>
    /// 管理员操作接口声明
    /// </summary>
    [ServiceContract]
    public interface IAdminFunctions
    {
        [OperationContract]
        void DoWork();
    }
}
