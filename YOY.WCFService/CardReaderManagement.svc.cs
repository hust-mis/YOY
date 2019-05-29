using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using YOY.BLL;
using YOY.DAL;
using YOY.Model.DB;

namespace YOY.WCFService
{
	/// <summary>
    /// 桌面式读卡器连接与RFID读写操作接口的实现
    /// 
    /// </summary>
	public class CardReaderManagement : ICardReaderManagement
	{

        /// <summary>
        /// 获取卡绑定游客id接口的实现
        /// </summary>
        public Stream GetCardUser()
        {
            string v = "GetNoUser";
            
            try
            {
                CardHelper.Connect();
                v = CardHelper.GetCardUser();
                CardHelper.DisConnect();
                return ResponseHelper.Success(v);
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    return ResponseHelper.Failure(ex.Message);
                else
                    return ResponseHelper.Failure(ex.InnerException.Message);
            }
        }

        /// <summary>
        /// 卡绑定游客接口的实现
        /// </summary>
        public Stream BindVisitor()
        {

            try
            {
                CardHelper.Connect();
                bool i = CardHelper.BindVisitor("V00001");
                CardHelper.DisConnect();
                if (i)
                {
                    return ResponseHelper.Success("绑定成功!");
                }
                else
                    return ResponseHelper.Success("绑定失败！");

            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    return ResponseHelper.Failure(ex.Message);
                else
                    return ResponseHelper.Failure(ex.InnerException.Message);
            }
        }

    }
}
