using spt.tool.Util;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace spt.tool.Serialport
{
    /// <summary>
    /// 自定义串口
    /// </summary>
    public class MySerialport
    {
        private SerialPort serialport;

        //TODO:加锁保证串口的连接状态，但会死锁？？？
        private object _muxLock = new object();
        public enum DataModel
        {
            Text, //文本
            Hex,  //16进制
        }
        public MySerialport()
        {
            serialport = new SerialPort();
        }

        #region Property
        /// <summary>
        /// 获取当前计算机的串行端口名的数组
        /// </summary>
        public string[] PortNameList
        {
            get {return SerialPort.GetPortNames();}
        }

        public bool IsOpen
        {
            get => serialport.IsOpen;
        }
        /// <summary>
        /// 当前使用的发送数据格式
        /// </summary>
        public DataModel SendModel { get; set; }

        /// <summary>
        /// 当前使用的接受数据格式
        /// </summary>
        public DataModel RecieveModel { get; set; }

        /// <summary>
        /// 发送的总字节数
        /// </summary>
        public long SendByteCount { get; set; }

        /// <summary>
        /// 接受的总字节数
        /// </summary>
        public long ReceiveByteCount { get; set; }
        #endregion


        #region Function
        public void Open(string poatName,int baudRate,int dataBits, Parity parity, StopBits stopBits)
        {
            lock (_muxLock)
            {
                //检测到串口打开时，需关闭
                if (serialport.IsOpen) serialport.Close();

                //串口设置
                serialport.PortName = poatName;
                serialport.BaudRate = baudRate;
                serialport.DataBits = dataBits;
                serialport.Parity = parity;
                serialport.StopBits = stopBits;

                //打开串口
                serialport.Open();
            }
        }

        public void Close()
        {
            serialport.Close();
        }

        public void Send(string msg)
        {
            if (SendModel == DataModel.Text)
            {
                byte[] buffer = System.Text.Encoding.Default.GetBytes(msg);

                SendByteCount += buffer.Length;

                serialport.Write(buffer, 0, buffer.Length);
            }
            else if(SendModel==DataModel.Hex)
            {
                byte[] buffer = ScaleUtil.GetHexArray(msg);

                SendByteCount += buffer.Length;

                serialport.Write(buffer, 0, buffer.Length);
            }
           
        }

        public void ReceiveAsync(Action<string> readBuffer)
        {
            Task.Run(() =>
            {
                while (true)
                {

                    try
                    {
                        lock (_muxLock)
                        {
                            if (serialport.IsOpen)
                            {
                                byte[] buffer = new byte[serialport.BytesToRead];
                                ReceiveByteCount += buffer.Length;
                                for (int i = 0; i < buffer.Length; i++)
                                {
                                  int bt= serialport.ReadByte();
                                    if (bt == -1)
                                    {
                                        break;
                                    }
                                    buffer[i] = (byte)bt;
                                }

                                if (buffer.Length > 0)
                                {
                                    string text = null;

                                    if (RecieveModel == DataModel.Hex)
                                    {
                                        for (int i = 0; i < buffer.Length; i++)
                                        {
                                            text += buffer[i].ToString("X") + " ";
                                        }
                                        readBuffer?.Invoke(text);
                                    }
                                    else
                                    {
                                        text = System.Text.Encoding.Default.GetString(buffer);
                                        readBuffer?.Invoke(text);
                                    }
                                }
                            }
                        }

                        System.Threading.Thread.Sleep(5);
                    }
                    catch (Exception)
                    {
                        System.Threading.Thread.Sleep(5);
                        continue;
                        
                    }
                }
            });
        }

        public void ClearCount()
        {
            SendByteCount = 0;
            ReceiveByteCount = 0;
        }
        #endregion
    }
}
