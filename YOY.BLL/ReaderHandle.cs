using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YOY.BLL
{
    public class ReaderHandle
    {
        //TODO

        // 端口号
        int Port = -1;

        // 读写器地址
        byte ComAdr = 0xff;


        /*  波特率对应值
            0	9600bps
            1	19200 bps
            2	38400 bps
            5	57600 bps
            6	115200 bps
        */
        byte Baud = 5;

        //波特率 57600 bps
        int lBaud = 57600;

        // 句柄
        int FrmHandle = -1;

        bool isConnect = false;


        private int EraseMaxLen = 50;
        private int WriteMaxLen = 50;
        private int WriteTryMaxCount = 20;
        
    }
}
