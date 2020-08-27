using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spt.tool.Util
{
    /// <summary>
    /// 显示时进制转换
    /// </summary>
    public static  class ScaleUtil
    {
        /// <summary>
        /// 16进制显示转换成文本显示
        /// </summary>
        /// <param name="hexText">16进制文本(16进制显示时元素之间加空格)</param>
        /// <returns></returns>
        public static string HexToText(string  hexText)
        {

            try
            {
                string[] ds = hexText.Trim().Split(' ');
                byte[] buffer = new byte[ds.Length];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = Convert.ToByte(ds[i], 16);
                }
                return System.Text.Encoding.Default.GetString(buffer);
            }
            catch (Exception)
            {
                return hexText;
            }
        }

        /// <summary>
        /// 文本显示转换成16进制显示
        /// </summary>
        /// <param name="text">16进制显示时元素之间加空格</param>
        /// <returns></returns>
        public static string TextToHex(string text)
        {
            try
            {
                if (text.Trim() == "") return "";
                byte[] buffer = System.Text.Encoding.Default.GetBytes(text);
                string display = "";
                for (int i = 0; i < buffer.Length; i++)
                {
                    display += buffer[i].ToString("X") + " ";
                }
                return display;
            }
            catch (Exception)
            {
                return text;
            }
        }

        /// <summary>
        /// 16进制文本转换成byte数组
        /// </summary>
        /// <param name="hexText"></param>
        /// <returns></returns>
        public static byte[] GetHexArray(string hexText)
        {
            try
            {
                string[] ds = hexText.Trim().Split(' ');
                byte[] buffer = new byte[ds.Length];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = Convert.ToByte(ds[i], 16);
                }
                return buffer;
            }
            catch (Exception)
            {
                return  System.Text.Encoding.Default.GetBytes(hexText);
            }
        }

    }
}
