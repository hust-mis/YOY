using ModuleLibrary;
using ModuleTech;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YOY.ModuleReader
{
    ///
    public partial class ModuleReaderManager : Form
    {
        ///
        Reader enterModulerdr = null;

        private string enterIP = "192.168.0.101";
        private bool isEnterConnected = false;


        private bool isInventory = false;
        private int readtimedur = 0;
        private Thread readThread = null;

        public ModuleReaderManager()
        {
            InitializeComponent();
        }

        private void BtnEnterConnect_Click(object sender, EventArgs e)
        {
            if( enterModulerdr != null || isEnterConnected )
            {
                MessageBox.Show("已连接！");
                return;
            }

            try
            {
                enterModulerdr = Reader.Create(enterIP, ModuleTech.Region.NA, 1);
            }
            catch(Exception ex)
            {
                MessageBox.Show("连接失败：" + ex.Message);
                return;
            }

            MessageBox.Show("连接成功！");
            isEnterConnected = true;
            this.BtnEnterConnect.Enabled = false;
            this.BtnEnterDisconnect.Enabled = true;

            if (readThread != null)
            {
                isInventory = false;
                readThread.Join();
            }

            isInventory = true;
            enterModulerdr.ParamSet("ReadPlan", new SimpleReadPlan(TagProtocol.GEN2, new List<int>() { 1 }.ToArray(), 30));
            readThread = new Thread(() => ReadFunc(enterModulerdr));
            readThread.Start();

        }

        private void BtnEnterDisconnect_Click(object sender, EventArgs e)
        {
            if( isEnterConnected && enterModulerdr != null )
            {
                isEnterConnected = false;
                enterModulerdr.Disconnect();
                enterModulerdr = null;
                this.BtnEnterDisconnect.Enabled = false;
                this.BtnEnterConnect.Enabled = true;
            }
            else
            {
                MessageBox.Show("没有处于连接状态！");
            }
        }

        void ReadFunc(Reader modulerdr)
        {
            int firsttagtime = 0;
            int hassetfirsttime = 0;
            int totalreadtimes = 0;

            int lastrettgtime = Environment.TickCount;


            while (isInventory)
            {
                try
                {
                    if (hassetfirsttime == 0)
                    {
                        firsttagtime = Environment.TickCount;
                        hassetfirsttime = 1;
                    }
                    //                    int ee = Environment.TickCount;

                    TagReadData[] reads = modulerdr.Read(200);

                    foreach (TagReadData read in reads)
                    {
                        MessageBox.Show(read.ToString());
                    }

                    totalreadtimes++;
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }


            }
            //          Debug.WriteLine("5555555555555555");
            readtimedur = Environment.TickCount - firsttagtime;
        }

    }
}
