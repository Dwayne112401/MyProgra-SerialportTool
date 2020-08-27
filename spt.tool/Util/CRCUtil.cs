using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spt.tool.Util
{
    public class CRCUtil
    {
        /// <summary>
        /// 计算并得到包含16位CRC校验的数组
        /// </summary>
        /// <param name="data">数组</param>
        /// <returns>包含16位CRC校验的数组(数组+16位CRC)</returns>
        public static byte[] GetCRC16(byte[] data)
        {
            ushort crc = 0xFFFF;

            for (int i = 0; i < data.Length; i++)
            {
                crc = (ushort)(crc ^ (data[i]));
                for (int j = 0; j < 8; j++)
                {
                    crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
                }
            }
            byte hi = (byte)((crc & 0xFF00) >> 8);  //高位
            byte lo = (byte)(crc & 0x00FF);         //低位
            byte[] buffer = new byte[] { hi, lo };

            return data.Concat(buffer).ToArray();
        }


        /// <summary>
        /// 检查数组是否通过16位CRC校验
        /// </summary>
        /// <param name="data">包含16位CRC校验的数组</param>
        /// <returns>true:通过;false:不通过</returns>
        public static bool CheckCRC16(byte[] data)
        {
            byte[] buffer=GetCRC16(data.Skip(0).Take(data.Length - 2).ToArray());
            for (int i = buffer.Length-2; i < buffer.Length; i++)
            {
                if (buffer[i] != data[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
