using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spt.model
{
    /// <summary>
    /// 串口参数
    /// </summary>
    public class SerialportParameterList
    {
        /// <summary>
        /// 可用的串口列表
        /// </summary>
         public List<string> PortNameList { get; set; }

        /// <summary>
        /// 可用的波特率列表
        /// </summary>
        public List<int> BaudRateList { get; set; }

        /// <summary>
        /// 可用的数组位列表
        /// </summary>
        public List<int> DataBitsList { get; set; }

        /// <summary>
        /// 可用的校验位列表
        /// </summary>
        public List<Parity> ParityList { get; set; }

        /// <summary>
        /// 可用的停止位列表
        /// </summary>
        public List<StopBits> StopBitsList { get; set; }

    }
}
