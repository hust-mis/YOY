using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YOY.Model.DB;
using YOY.BLL;

namespace YOY.DAL
{
    public sealed class CardHelper
    {
        public static ReaderHandle reader;

        // 连接读写器
        public static bool Connect()
        {
            reader = new ReaderHandle();
            bool i = reader.Connect();
            if (i == false)
            {
                return false;//连接失败
            }
            return true;//连接成功

        }

        // 关闭读写器
        public static bool DisConnect()
        {
            if (reader != null)
            {
                bool i = reader.DisConnect();
                reader = null;
                if (i == false)
                {
                    return false;//未关闭
                }
                return true;
            }
            return true;//已关闭
        }


        // 初始化卡（EPC区写入卡号）
        public static bool InitCard(Card c)
        {
            c.CardState = 1;
            bool writeStatus = reader.WriteEpc("K00001");
            if (writeStatus == false)
            {
                return false;
            }
            return true;

        }

        //VisitorID写入用户区
        public static bool BindVisitor(string VisitorID)
        {
            string EPC = reader.ReadEpc();
            //起始位置0,写入6位,不足位用0填充
            VisitorID = String.Format("{0:000000}", Int32.Parse(VisitorID));
            if (reader.WriteUserData(EPC, 0, VisitorID))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        //读取当前卡用户区VisitorID
        public static string ReadVisitor()
        {
            string EPC = reader.ReadEpc();
            //读取位置0，读取6位
            string ss = reader.ReadUserData(EPC, "0", "6");
            ss = ss.Substring(0, 6);
            return ss;
        }


        //读取当前RFID卡信息
        public static Card GetCardInfo()
        {
            //List<Card> c = new List<Card>();
            Card c = new Card();
            c.CardID = reader.ReadEpc();
            c.CardState = 1;
            return c;
        }

        //获取卡中游客id
        public static string GetCardUser()
        {
            //List<Card> c = new List<Card>();
            string VisitorID;
            VisitorID = ReadVisitor();
            return VisitorID;
        }

    }
}
