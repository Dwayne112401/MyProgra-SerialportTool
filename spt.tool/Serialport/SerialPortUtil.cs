using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace spt.tool.Serialport
{
    public class SerialPortUtil
    {
        private SerialPort _serialPort;
        private readonly ConcurrentQueue<byte[]> _messageQueue;
        private readonly EventWaitHandle _messageWaitHandle;
        private readonly object _mux;
        private long _receiveCount, _sendCount;
        public enum DataType
        {
            /// <summary>
            /// 文本模式
            /// </summary>
            Text,
            /// <summary>
            /// 16进制
            /// </summary>
            Hex,
        }

        public long ReceiveCount
        {
            get => _receiveCount;
        }
        public long SendCount
        {
            get => _sendCount;
        }
        public bool IsOpen
        {
            get => _serialPort.IsOpen;
        }

        public Encoding Encoding { get; set; } = Encoding.Default;


        /// <summary>
        /// 当前使用的发送格式<see cref="DataType"/>
        /// </summary>
        public DataType SendType { get; set; }
        /// <summary>
        /// 当前使用的接受格式<see cref="DataType"/>
        /// </summary>
        public DataType RecieveType { get; set; }

        public SerialPortUtil()
        {
            // initialized
            _mux = new object();
            _receiveCount = 0;
            _sendCount = 0;
            _messageQueue = new ConcurrentQueue<byte[]>();
            _messageWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

            // initialized com
            _serialPort = new SerialPort();

            // Receive byte
            _serialPort.DataReceived += PushMessage;
        }

        public void OpenCom(string com, int baud, int dataBits, Parity parity, StopBits stopBits)
        {
            // Open Com
            if (_serialPort.IsOpen) _serialPort.Close();

            //串口设置
            _serialPort.PortName = com;
            _serialPort.BaudRate = baud;
            _serialPort.DataBits = dataBits;
            _serialPort.Parity = parity;
            _serialPort.StopBits = stopBits;

            // Set the read / write timeouts
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;

            // Set read / write buffer Size，the default of value is 1MB
            _serialPort.ReadBufferSize = 1024 * 1024;
            _serialPort.WriteBufferSize = 1024 * 1024;

            _serialPort.Open();

            // Discard Buffer
            _serialPort.DiscardInBuffer();
            _serialPort.DiscardOutBuffer();

           
        }


        #region Static
        /// <summary>
        /// 获取当前计算机的串行端口名的数组
        /// </summary>
        /// <returns></returns>
        public static string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }
        #endregion

        #region Receive
        private void PushMessage(object sender, SerialDataReceivedEventArgs e)
        {
            lock (_mux)
            {
                if (_serialPort.IsOpen == false) return;
                int length = _serialPort.BytesToRead;
                byte[] buffer = new byte[length];
                if (length <= 0) return;
                _serialPort.Read(buffer, 0, length);
                _receiveCount += length;
                _messageQueue.Enqueue(buffer);
                _messageWaitHandle.Set();
            }
        }

        /// <summary>
        /// 获取串口接受到的内容
        /// </summary>
        /// <param name="millisecondsToTimeout">取消息的超时时间</param>
        /// <returns>返回byte数组</returns>
        public byte[] TryMessage(int millisecondsToTimeout = -1)
        {
            if (_messageQueue.Count > 0 && _messageQueue.TryDequeue(out var message))
            {
                return message;
            }
            else
            {
                Console.WriteLine("1");
            }

            if (_messageWaitHandle.WaitOne(millisecondsToTimeout))
            {
                if (_messageQueue.TryDequeue(out message))
                {
                    return message;
                }
                else
                {
                    Console.WriteLine("2");
                }
            }
            return default;
        }
        #endregion


        #region Send
        /// <summary>
        /// 发送消息（byte数组）
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public void Send(byte[] buffer, int offset, int count)
        {
            lock (_mux)
            {
                _serialPort.Write(buffer, offset, count);
                _sendCount += (count - offset);
            }
        }

        /// <summary>
        /// 发送消息（字符串）
        /// </summary>
        /// <param name="message"></param>
        public void Send(string message)
        {
            lock (_mux)
            {
                var buffer = Encoding.GetBytes(message);
                _serialPort.Write(buffer, 0, buffer.Length);
                _sendCount += buffer.Length;
            }
        }
        #endregion

        /// <summary>
        /// 清空接受/发送总数统计
        /// </summary>
        public void ClearCount()
        {
            lock (_mux)
            {
                _sendCount= 0;
                _receiveCount = 0;
            }
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        public void Close()
        {
            _serialPort.DataReceived -= PushMessage;
            _serialPort.Close();
        }
    }
}
