using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using YOY.Model;

namespace YOY.BLL
{
    /// <summary>
    /// LocalSense获取位置的工具类
    /// </summary>
    public sealed class LocalSense
    {
        private static readonly string IP_Adr = "ws://192.168.0.151:9001/";//localsense 定位地址
        private static readonly int readSize = 60;  //读取数据长度
        private Mutex socMutex = new Mutex();
        private byte[] data = new byte[readSize];//接收数据存放处
        private int CpLen = 21; //数据帧中的标签信息的长度
        private WebSocket socket = null;
        public List<Location> locations = new List<Location>();

        public void Run()
        {
            if (socket == null)
                socket = new WebSocket(IP_Adr, "localSensePush-protocol");//连接localSensePush

            socket.OnMessage += (sender, e) =>
            {
                Transform(sender, e);
            };
            socket.Connect();
        }

        public void Stop()
        {
            if (socket != null) socket.Close();
        }


        private string[] copyStrArWithLen(string[] preStr, int start)
        {
            string[] retunAr = new string[CpLen];
            for (int i = start; i < start + CpLen; i++)
            {
                retunAr[i - start] = preStr[i];
            }
            return retunAr;
        }

        private void Transform(object sender , MessageEventArgs e)
        {
            try
            {
                socMutex.WaitOne();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //CC-5F-01-01-  3A-46-00-00-02-A4-00-00-02-C7-00-E1-01-03-00-04-15-3C-52-01 -01-C8-1D-AA-BB
            string[] temp = e.Data.Split('-');
            if (temp[0] == "CC" && temp[1] == "5F" && temp[2] == "01")
            {
                int num = Convert.ToInt32(temp[3], 16);
                for (int i = 0; i < num; i++)
                {
                    string[] subTemp = copyStrArWithLen(temp, 4 + i * CpLen);
                    locations.Add(new Location()
                    {
                        ID = Convert.ToInt32(subTemp[0] + subTemp[1], 16).ToString(),
                        X = Convert.ToInt32(subTemp[2] + subTemp[3] + subTemp[4] + subTemp[5], 16).ToString(),
                        Y = Convert.ToInt32(subTemp[6] + subTemp[7] + subTemp[8] + subTemp[9], 16).ToString(),
                        Timestamp = Convert.ToInt32(subTemp[15] + subTemp[16] + subTemp[17] + subTemp[18], 16).ToString()
                    });
                }
            }
            socMutex.ReleaseMutex();
        }
    }
}
