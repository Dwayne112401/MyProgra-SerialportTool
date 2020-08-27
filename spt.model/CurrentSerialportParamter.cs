using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spt.model
{
    public class CurrentSerialportParamter
    {
        /// <summary>
        /// 可用的串口列表
        /// </summary>
        public string PortName { get; set; }

        /// <summary>
        /// 可用的波特率列表
        /// </summary>
        public int BaudRate { get; set; }

        /// <summary>
        /// 可用的数组位列表
        /// </summary>
        public int DataBits { get; set; }

        /// <summary>
        /// 可用的校验位列表
        /// </summary>
        public Parity Parity { get; set; }

        /// <summary>
        /// 可用的停止位列表
        /// </summary>
        public StopBits StopBits { get; set; }
    }
}
