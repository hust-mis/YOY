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
using YOY.DAL;
using YOY.Model.DB;

namespace YOY.ModuleReader
{
    public partial class ModuleReaderManager : Form
    {

        /// <summary>
        /// 该项目对应的项目ID
        /// </summary>
        private string ProjectID = "P00001";

        #region 入口处属性
        /// <summary>
        /// 入口处读取RFID
        /// </summary>
        Reader enterModulerdr = null;
        /// <summary>
        /// 是否在进行读取
        /// </summary>
        private bool isEnterInventory = false;
        /// <summary>
        /// 读取操作的线程
        /// </summary>
        private Thread enterRead = null;
        /// <summary>
        /// 入口处读写器IP地址
        /// </summary>
        private string enterIP = "192.168.0.101";
        /// <summary>
        /// 入口处读写器是否连接
        /// </summary>
        private bool isEnterConnected = false;
        /// <summary>
        /// 入口处读取数据临时列表
        /// </summary>
        private List<TagReadData> enterTemp = new List<TagReadData>();
        #endregion

        #region 出口处属性
        /// <summary>
        /// 出口处读取RFID
        /// </summary>
        Reader exitModulerdr = null;
        /// <summary>
        /// 是否在进行读取
        /// </summary>
        private bool isExitInventory = false;
        /// <summary>
        /// 读取操作的线程
        /// </summary>
        private Thread exitRead = null;
        /// <summary>
        /// 出口处读写器IP地址
        /// </summary>
        private string exitIP = "192.168.0.102";
        /// <summary>
        /// 出口处读写器是否连接
        /// </summary>
        private bool isExitConnected = false;
        /// <summary>
        /// 出口处读取数据临时列表
        /// </summary>
        private List<TagReadData> exitTemp = new List<TagReadData>();
        #endregion

        #region 项目处属性
        /// <summary>
        /// 项目处读取RFID
        /// </summary>
        Reader projectModulerdr = null;
        /// <summary>
        /// 是否在进行读取
        /// </summary>
        private bool isProjectInventory = false;
        /// <summary>
        /// 读取操作的线程
        /// </summary>
        private Thread projectRead = null;
        /// <summary>
        /// 项目处读写器IP地址
        /// </summary>
        private string projectIP = "192.168.0.103";
        /// <summary>
        /// 项目处读写器是否连接
        /// </summary>
        private bool isProjectConnected = false;
        /// <summary>
        /// 项目处读取数据临时列表
        /// </summary>
        private List<TagReadData> projectTemp = new List<TagReadData>();
        #endregion


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
                //建立读取连接
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

            if (enterRead != null)
            {
                isEnterInventory = false;
                enterRead.Join();
            }

            isEnterInventory = true;
            //设置读取计划
            enterModulerdr.ParamSet("ReadPlan", new SimpleReadPlan(TagProtocol.GEN2, new List<int>() { 1 }.ToArray(), 30));
            //运行读取线程
            enterRead = new Thread(() => ReadFunc(enterModulerdr, enterTemp, isEnterInventory));
            enterRead.Start();

        }

        private void BtnEnterDisconnect_Click(object sender, EventArgs e)
        {
            isEnterInventory = false;
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

        /// <summary>
        /// 线程内的读取操作
        /// </summary>
        /// <param name="modulerdr">建立的读取连接</param>
        /// <param name="readDatas">要储存的临时列表</param>
        /// <param name="isInventory">是否继续读取</param>
        void ReadFunc(Reader modulerdr , List<TagReadData> readDatas, bool isInventory)
        {
            int firsttagtime = 0;
            int hassetfirsttime = 0;

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
                    readDatas.AddRange(reads);
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //定时进行数据上传
        private void TimerUpdate_Tick(object sender, EventArgs e)
        {
            if( isEnterInventory )
            {
                try
                {
                    var updateItems = from enter in enterTemp
                                       group enter by enter.EPCString into up
                                       select new { CardID = up.Key, Time = up.Max(t => t.Time) };
                    enterTemp.Clear();

                    var v2c = from v in EFHelper.GetAll<Visitor2Card>()
                              join u in updateItems on v.CardID equals u.CardID
                              select new { v.VisitorID, u.Time };

                    if (v2c.Count() > 0 )
                    {
                        using (var db = new EFDbContext())
                        {
                            foreach (var v in v2c)
                            {
                                db.ProjectOperation.Add(new Operation()
                                {
                                    ProjectID = ProjectID,
                                    VisitorID = v.VisitorID,
                                    PlayState = 0
                                });
                                db.ProjectRecord.Add(new ProRecord()
                                {
                                    ProjectID = ProjectID,
                                    VisitorID = v.VisitorID,
                                    Timestamp = v.Time,
                                    PlayState = 0
                                });
                            }
                            db.SaveChanges();
                        }
                    }

                }
                catch(Exception ex)
                {
                }


            }

            if (isExitInventory)
            {
                try
                {
                    var updateItems = from exit in exitTemp
                                      group exit by exit.EPCString into up
                                      select new { CardID = up.Key, Time = up.Max(t => t.Time) };
                    enterTemp.Clear();

                    var v2c = from v in EFHelper.GetAll<Visitor2Card>()
                              join u in updateItems on v.CardID equals u.CardID
                              select new { v.VisitorID, u.Time };

                    if (v2c.Count() > 0)
                    {
                        using (var db = new EFDbContext())
                        {
                            foreach (var v in v2c)
                            {
                                var operation = db.ProjectOperation.First(t => t.ProjectID == ProjectID && t.VisitorID == v.VisitorID);
                                if (operation != null) db.ProjectOperation.Remove(operation);
                                db.ProjectRecord.Add(new ProRecord()
                                {
                                    ProjectID = ProjectID,
                                    VisitorID = v.VisitorID,
                                    Timestamp = v.Time,
                                    PlayState = 2
                                });
                            }

                            db.SaveChanges();
                        }
                    }

                }
                catch (Exception ex)
                {
                }

            }
        }

        private void BtnExitConnect_Click(object sender, EventArgs e)
        {
            if (exitModulerdr != null || isExitConnected)
            {
                MessageBox.Show("已连接！");
                return;
            }

            try
            {
                //建立读取连接
                exitModulerdr = Reader.Create(exitIP, ModuleTech.Region.NA, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接失败：" + ex.Message);
                return;
            }

            MessageBox.Show("连接成功！");
            isExitConnected = true;
            this.BtnExitConnect.Enabled = false;
            this.BtnExitDisconnect.Enabled = true;

            if (exitRead != null)
            {
                isExitInventory = false;
                exitRead.Join();
            }

            isExitInventory = true;
            //设置读取计划
            exitModulerdr.ParamSet("ReadPlan", new SimpleReadPlan(TagProtocol.GEN2, new List<int>() { 1 }.ToArray(), 30));
            //运行读取线程
            exitRead = new Thread(() => ReadFunc(exitModulerdr, exitTemp, isExitInventory));
            exitRead.Start();

        }

        private void BtnExitDisconnect_Click(object sender, EventArgs e)
        {
            isExitInventory = false;
            if (isExitConnected && exitModulerdr != null)
            {
                isExitConnected = false;
                exitModulerdr.Disconnect();
                exitModulerdr = null;
                this.BtnExitDisconnect.Enabled = false;
                this.BtnExitConnect.Enabled = true;
            }
            else
            {
                MessageBox.Show("没有处于连接状态！");
            }
        }

        private void BtnProjectConnect_Click(object sender, EventArgs e)
        {
            if (projectModulerdr != null || isProjectConnected)
            {
                MessageBox.Show("已连接！");
                return;
            }

            try
            {
                //建立读取连接
                projectModulerdr = Reader.Create(projectIP, ModuleTech.Region.NA, 4);
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接失败：" + ex.Message);
                return;
            }

            MessageBox.Show("连接成功！");
            isProjectConnected = true;
            this.BtnProjectConnect.Enabled = false;
            this.BtnProjectDisconnect.Enabled = true;

            if (projectRead != null)
            {
                isProjectInventory = false;
                projectRead.Join();
            }

            isProjectInventory = true;
            //设置读取计划
            projectModulerdr.ParamSet("ReadPlan", new SimpleReadPlan(TagProtocol.GEN2, new List<int>() { 1 }.ToArray(), 30));
            //运行读取线程
            projectRead = new Thread(() => ReadFunc(projectModulerdr, projectTemp, isProjectInventory));
            projectRead.Start();

        }

        private void TimeProject_Tick(object sender, EventArgs e)
        {
            if (isProjectInventory)
            {
                try
                {
                    var updateItems = from project in projectTemp
                                      group project by project.EPCString into up
                                      select new { CardID = up.Key, Time = up.Max(t => t.Time) };
                    projectTemp.Clear();

                    var v2c = from v in EFHelper.GetAll<Visitor2Card>()
                              join u in updateItems on v.CardID equals u.CardID
                              select new { v.VisitorID, u.Time };

                    if (v2c.Count() > 0)
                    {
                        using (var db = new EFDbContext())
                        {
                            foreach (var v in v2c)
                            {
                                var operation = db.ProjectOperation.First(t => t.ProjectID == ProjectID && t.VisitorID == v.VisitorID);
                                if (operation != null) operation.PlayState = 1;
                                db.ProjectRecord.Add(new ProRecord()
                                {
                                    ProjectID = ProjectID,
                                    VisitorID = v.VisitorID,
                                    Timestamp = v.Time,
                                    PlayState = 1
                                });
                            }

                            db.SaveChanges();
                        }
                    }

                }
                catch (Exception ex)
                {
                }
            }
        }

        private void BtnProjectDisconnect_Click(object sender, EventArgs e)
        {
            isProjectInventory = false;
            if (isProjectConnected && projectModulerdr != null)
            {
                isProjectConnected = false;
                projectModulerdr.Disconnect();
                projectModulerdr = null;
                this.BtnProjectDisconnect.Enabled = false;
                this.BtnProjectConnect.Enabled = true;
            }
            else
            {
                MessageBox.Show("没有处于连接状态！");
            }
        }
    }
}
