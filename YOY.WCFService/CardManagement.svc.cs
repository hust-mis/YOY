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
	
	public class CardManagement : ICardManagement
	{
        public string GetCardUser()
        {
            string v = "GetNoUser";
            CardHelper.Connect();
            CardHelper.GetCardUser();
            CardHelper.DisConnect();
            return v;
        }



    }
}
